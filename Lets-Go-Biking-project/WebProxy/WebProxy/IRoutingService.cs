using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WebProxy
{
    [ServiceContract]
    public interface IRoutingService
    {
        [OperationContract]
        string getAllStations();

        [OperationContract]
        string getStationInfo(string contrat, string num);
    }
}
