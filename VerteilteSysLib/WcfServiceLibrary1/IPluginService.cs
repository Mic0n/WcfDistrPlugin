using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VerteilteSysLib;

namespace WcfServiceLibrary1
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IPluginService
    {
        [OperationContract]
        String DisplayAllPlugins();

        [OperationContract]
        String UploadFile(byte[] data);

        [OperationContract]
        String UsePlugin(String pluginName);
    }
}

