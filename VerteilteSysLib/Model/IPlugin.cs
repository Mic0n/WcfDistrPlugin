using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VerteilteSysLib
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IPlugin
    {
        [OperationContract]
        String DisplayPlugins();

        [OperationContract]
        String DoOperation( String command);

        [OperationContract]
        int[] nodeAmount( String command );
    }
}
