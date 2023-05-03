using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary1
{
    public class NodeService : INodeService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ServiceManager serviceManager = ServiceManager.Instance;

        public void registerNode(Guid id)
        {
            log.Debug("received node registration from " + id.ToString());
            serviceManager.registerNode(id);
        }
    }
}
