using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.JCDcauxModels
{
    [DataContract(Name = "Availabilitie")]
    public class Availabilitie
    {
        [DataMember(Name = "bikes")]
        public int bikes { get; set; }

        [DataMember(Name = "stands")]
        public int stands { get; set; }

        [DataMember(Name = "mechanicalBikes")]
        public int mechanicalBikes { get; set; }

        [DataMember(Name = "electricalBikes")]
        public int electricalBikes { get; set; }

        [DataMember(Name = "electricalInternalBatteryBikes")]
        public int electricalInternalBatteryBikes { get; set; }

        [DataMember(Name = "electricalRemovableBatteryBikes")]
        public int electricalRemovableBatteryBikes { get; set; }

        public string toString()
        {
            string result = "bikes: "+bikes+ "\nstands: "+stands+"\nmechanicalBikes: "+mechanicalBikes +"\n";
            result += "electricalBikes: "+electricalBikes+ "\nelectricalInternalBatteryBikes: "+electricalInternalBatteryBikes+"\nelectricalRemovableBatteryBikes: "+electricalRemovableBatteryBikes +"\n";
            return result;
        }
    }
}
