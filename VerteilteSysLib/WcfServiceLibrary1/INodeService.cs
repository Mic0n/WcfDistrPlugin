using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary1
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples", SessionMode = SessionMode.Required,
                 CallbackContract = typeof(INodeServiceCallback))]
    public interface INodeService
    {
        [OperationContract(IsOneWay = true)]
        void registerNode(Guid id);
    }
}
