using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerteilteSysLib;
using WcfServiceLibrary1;

namespace WcfServiceLibrary1
{
    public class PluginService : IPluginService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServiceManager serviceManager = ServiceManager.Instance;

        public string DisplayAllPlugins()
        {
            log.Debug("requesting all plugin names");
            return serviceManager.DisplayAllPlugins();
        }

        public String UploadFile(byte[] data)
        {
            log.Debug("received upload");
            return serviceManager.UploadFile(data);
        }

        public string UsePlugin(string pluginName)
        {
            log.Debug("received usePlugin request");
            return serviceManager.UsePlugin(pluginName);
        }
    }
}