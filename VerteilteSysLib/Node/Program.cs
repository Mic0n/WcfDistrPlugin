using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ServiceModel;
using System.ServiceModel.Description;
using VerteilteSysLib;
using Node.ServiceReference1;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

namespace VerteilteSysNode
{
    public class NodeCallback : INodeServiceCallback
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [ImportMany(AllowRecomposition = true)]
        IEnumerable<Lazy<IPlugin>> plugins;
        AggregateCatalog catalog;
        private CompositionContainer _container;
        string plugindir = "";

        public NodeCallback() {
            Directory.CreateDirectory("./Extensions");
            log.Debug("Created Node and loading plugins");
            Log("Created Node and loading plugins");
            try
            {
                catalog = new AggregateCatalog();
                var directories = Directory.GetDirectories("./Extensions");
                foreach (var directory in directories)
                {
                    log.Debug("got Plugin from directory" + directory);
                    catalog.Catalogs.Add(new DirectoryCatalog(directory));
                }
                _container = new CompositionContainer(catalog);
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                log.Error(compositionException);
            }
        }

        private void reload(string path)
        {
            log.Debug("discovering Plugins");
            catalog.Catalogs.Add(new DirectoryCatalog(path));
            _container.ComposeParts(this);
        }

        private void savePlugin(string name, byte[] data)
        {
            log.Debug("Creating directory for new Plugin");
            string fileName = String.Format(@"{0}\{1}.dll", "./Extensions/" + name, name);
            Directory.CreateDirectory("./Extensions/" + name);
            log.Debug("Writing dll file for Plugin");
            var writer = new BinaryWriter(File.OpenWrite(fileName));
            writer.Write(data);
            writer.Close();
            reload("./Extensions/" + name);
        }

        public string DoWork( string pluginName)
        {
            
            string name = pluginName.Split(' ')[0];
            string lower = pluginName.Split(' ')[1];
            string upper = pluginName.Split(' ')[2];
            string output = "No matching plugin found";
            log.Debug("using "+ name + " plugin");
            foreach (Lazy<IPlugin> i in plugins)
            {
                if (name.Equals(i.Value.DisplayPlugins()))
                {
                    log.Debug("found Plugin " + name);
                    output = i.Value.DoOperation(lower + " " + upper);
                }
            }
            return output;
        }

        public void SendPlugin( string name, byte[] data)
        {
            log.Debug("received plugin");
            savePlugin(name, data);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string s)
        {
            Console.WriteLine(s);
        }

    }
    class Program 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Guid id = Guid.NewGuid();
        static void Main(string[] args)
        {
            log.Info("Node id is " + id.ToString());
            Log("Node id is " + id.ToString());
            var callback = new NodeCallback();
            var context = new InstanceContext(callback);

            NodeServiceClient serviceClient = new NodeServiceClient(context);
            log.Debug("created serviceClient");
            Log("created serviceClient");
            serviceClient.registerNode(id);
            log.Info("registered node with service" + id.ToString());
            Log("registered node with service" + id.ToString());
            Console.ReadLine();
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string s)
        {
            Console.WriteLine(s);
        }

    }

    
}
