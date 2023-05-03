using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VerteilteSysLib;
using WcfServiceLibrary1;

namespace VerteilteSysHost
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address.
            Uri baseAddress = new Uri("http://localhost:8000/VerteilteSysLib/");
            Uri baseAddress2 = new Uri("http://localhost:8000/VerteilteSysLib/test/");
            log.Info("Starting server");
            log.Debug(baseAddress.ToString() + " is Clientservice URL");
            log.Debug(baseAddress.ToString() + " is Nodeservice URL");

            // Step 2: Create a ServiceHost instance.
            ServiceHost selfHost = new ServiceHost(typeof(PluginService), baseAddress);
            ServiceHost nodeHost = new ServiceHost(typeof(NodeService), baseAddress2);
            WSHttpBinding binding = new WSHttpBinding();
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

            WSDualHttpBinding duplexBinding = new WSDualHttpBinding();
            duplexBinding.OpenTimeout = new TimeSpan(0, 10, 0);
            duplexBinding.CloseTimeout = new TimeSpan(0, 10, 0);
            duplexBinding.SendTimeout = new TimeSpan(0, 10, 0);
            duplexBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);


            try
            {
                // Step 3: Add a service endpoint.
                selfHost.AddServiceEndpoint(typeof(IPluginService), binding, "PluginService");
                nodeHost.AddServiceEndpoint(typeof(INodeService), duplexBinding, "NodeService");
                //DuplexChannelFactory<INodeService> channelFactory = new DuplexChannelFactory<INodeService>(new InstanceContext(this), binding, address);

                // Step 4: Enable metadata exchange.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);
                nodeHost.Description.Behaviors.Add(smb);

                // Step 5: Start the service.
                selfHost.Open();
                log.Debug("clientService is Open");
                nodeHost.Open();
                log.Debug("nodeService is Open");
                Console.WriteLine("Service is ready");
                Console.ReadLine();

                // Close the ServiceHost to stop the service.
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.ReadLine();
                selfHost.Close();
                nodeHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
