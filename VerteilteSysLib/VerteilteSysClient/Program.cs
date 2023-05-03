using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerteilteSysClient.ServiceReference1;

namespace VerteilteSysClient
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            PluginServiceClient client = new PluginServiceClient();
            log.Debug("setting up clientService connection");
            run(client);
            
        }

        static void run(PluginServiceClient client)
        {
            
            Console.WriteLine(client.DisplayAllPlugins());
            Console.WriteLine("Enter Plugin name to use functions");
            Console.WriteLine("Enter upload to upload a Plugin");
            string command = Console.ReadLine();
            log.Debug("got " + command + " as command");
            if (command == "upload")
            {
                log.Debug("selected Upload");
                Console.WriteLine("Enter path to Plugin you want to Upload");
                string path = Console.ReadLine();
                var size = new FileInfo(path).Length;
                Stream str = File.OpenRead(path);
                BinaryReader br = new BinaryReader(str);
                var con = br.ReadBytes((int)size);
                client.UploadFile(con);
            }
            else
            {
                log.Debug("using Plugin");
                Console.WriteLine(client.UsePlugin(command));
            }
            run(client );
        }
    }
}
