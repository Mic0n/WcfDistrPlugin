using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary1
{
    public interface INodeServiceCallback
    {
        [OperationContract]
        string DoWork(string message);
        [OperationContract(IsOneWay = true)]
        void SendPlugin(string name, byte[] plugin);
    }
}
