using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VerteilteSysLib;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;

namespace WcfServiceLibrary1
{
    
    sealed class ServiceManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ServiceManager Instance { get { return instance.Value; } }
        private static readonly Lazy<ServiceManager> instance = new Lazy<ServiceManager>(() => new ServiceManager());
        private int _registeredNodes;
        private static List<INodeServiceCallback> _nodes = new List<INodeServiceCallback>();
        private static List<INodeServiceCallback> _availableNodes = new List<INodeServiceCallback>();
        String pluginDir = "C:/Users/micon/Desktop/Michael Sievers 690593/VerteilteSysLib/VerteilteSysHost/ExternalPlugins";
        //String pluginDir = "../../ExternalPlugins";
        [ImportMany(AllowRecomposition = true)]
        IEnumerable<Lazy<IPlugin>> plugins;
        AggregateCatalog catalog;
        private CompositionContainer _container;

        private ServiceManager()
        {
            try
            {
                log.Debug("loading all plugins");
                catalog = new AggregateCatalog();
                var directories = Directory.GetDirectories(pluginDir);
                foreach (var directory in directories)
                {
                    log.Debug("adding plugin from "+ directory);
                    var name = Path.GetFileName(directory);
                    string[]files = Directory.GetFiles(directory);
                    var path = files[0];
                    var size = new FileInfo(path).Length;
                    Stream str = File.OpenRead(path);
                    BinaryReader br = new BinaryReader(str);
                    var con = br.ReadBytes((int)size);
                    br.Close();
                    log.Debug("Converted plugin to dll");
                    sendToNodes(name, con);
                    log.Debug("sent plugin to nodes");
                    catalog.Catalogs.Add(new DirectoryCatalog(directory));
                    log.Debug("Added Plugins to server");
                }
                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                log.Error(compositionException); 
            }
        }

        public void registerNode(Guid id)
        {
            log.Debug("registering node " + id.ToString());
            var callback = OperationContext.Current.GetCallbackChannel<INodeServiceCallback>();
            _nodes.Add(callback);
            _availableNodes.Add(callback);
        }

        private void sendToNodes(string name, byte[] data)
        {
            foreach (var node in _nodes)
            {
                log.Debug("sent " + name + "to node");
                node.SendPlugin(name, data);
            }
        }

        private void reload(string path)
        {
            log.Debug("refreshing loaded plugins");
            catalog.Catalogs.Add(new DirectoryCatalog(path));
            _container.ComposeParts(this);
        }

        public string DisplayAllPlugins()
        {
            log.Debug("display plugins");
            string allPlugins = "Available plugins: ";
            foreach (Lazy<IPlugin> i in plugins)
            {
                allPlugins += i.Value.DisplayPlugins();
                allPlugins += "; ";
                log.Debug("plugins til now: " +  allPlugins);
            }
            return allPlugins;
        }

        public String UploadFile(byte[] data)
        {
            string name = Guid.NewGuid().ToString();
            log.Debug("received plugin and named " + name);
            string fileName = String.Format(@"{0}\{1}.dll", pluginDir + "/" + name, name);
            Directory.CreateDirectory(pluginDir + "/" + name);
            log.Debug("created Directory for Plugin at " + fileName);
            var writer = new BinaryWriter(File.OpenWrite(fileName));
            writer.Write(data);
            writer.Close();
            log.Debug("wrote plugin dll in directory");
            reload(pluginDir + "/" + name);
            log.Debug("reloaded Plugins");
            sendToNodes(name, data);
            log.Debug("sent plugin out to nodes");

            return fileName;
        }


        
        public string UsePlugin(string pluginName)
        {
            string name = pluginName.Split(' ')[0];
            string input = pluginName.Split(' ')[1];
            log.Debug("using Plugin" + name);
            int nodes = 1;
            IPlugin plugin = null;
            int split = 0;
            foreach (Lazy<IPlugin> i in plugins)
            {
                if (name.Equals(i.Value.DisplayPlugins()))
                {
                    log.Debug("found plugin");
                    plugin = i.Value;
                    int[] temp = i.Value.nodeAmount(input);
                    nodes = temp[0];
                    split = temp[1];
                    log.Debug("got the nodes required for plugin: " + nodes);
                    
                }
            }
            if(plugin == null)
            {
                return "Plugin not found";
            }

            List<INodeServiceCallback> _usedNodes = new List<INodeServiceCallback>();
            if(_availableNodes.Count < nodes)
            {
                return "Not enough Nodes";
            }

            for (int i = 0; i < nodes; i++)
            {
                try
                {
                    _usedNodes.Add(_availableNodes[0]);
                    _availableNodes.RemoveAt(0);
                }
                catch(Exception e)
                {
                    log.Error(e.ToString());
                }
                
            }
            log.Debug("selected availabe nodes");

            SynchronizedCollection <String> outputs = new SynchronizedCollection <String>();
            

            try
            {
                int lower = 0;
                int upper = split;
                int count = -1;
                int index = 0;
                string[] commands = new string[nodes];
                foreach(string command in commands) {
                    commands[index] = name + " " + lower + " " + upper;
                    lower += split;
                    upper += split;
                    index++;
                }
                Parallel.ForEach(_usedNodes, node =>
                {
                    try
                    {
                        var currentCount = Interlocked.Increment(ref count);
                        outputs.Add(node.DoWork(commands[count]));
                        currentCount = Interlocked.Increment(ref count);
                    }
                    catch (Exception e) { 
                        log.Error(e.ToString());
                    }
                    
                }
                            );
                log.Debug("distributed work to nodes");
            }catch (Exception e)
            {
                log.Error(e.ToString());
            }

            foreach (INodeServiceCallback callback in _usedNodes)
            {
                _availableNodes.Add(callback);
            }
            log.Debug("made used nodes available again");
            string result = "";
            foreach(string s in outputs)
            {
                result += s + " ";
            }
            return result;

        }

    }
    
}
