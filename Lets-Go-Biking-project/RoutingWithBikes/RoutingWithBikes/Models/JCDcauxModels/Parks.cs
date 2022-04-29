using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.JCDcauxModels
{
    [DataContract(Name = "Parks")]
    internal class Parks
    {
        [DataMember(Name = "contractName")]
        public string contractName { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "number")]
        public int number { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "position")]
        public Position position { get; set; }

        [DataMember(Name = "accessType")]
        public string accessType { get; set; }

        [DataMember(Name = "lockerType")]
        public string lockerType { get; set; }

        [DataMember(Name = "hasSurveillance")]
        public Boolean hasSurveillance { get; set; }

        [DataMember(Name = "isFree")]
        public Boolean isFree { get; set; }

        [DataMember(Name = "address")]
        public string address { get; set; }

        [DataMember(Name = "zipCode")]
        public string zipCode { get; set; }

        [DataMember(Name = "city")]
        public string city { get; set; }

        [DataMember(Name = "isOffStreet")]
        public Boolean isOffStreet { get; set; }

        [DataMember(Name = "hasElectricSupport")]
        public Boolean hasElectricSupport { get; set; }

        [DataMember(Name = "hasPhysicalReception")]
        public Boolean hasPhysicalReception { get; set; }
    }
}
