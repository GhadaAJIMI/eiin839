using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.JCDcauxModels
{
    [DataContract(Name = "Contract")]
    public class Contract
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "commercial_name")]
        public string commercial_name { get; set; }

        [DataMember(Name = "country_code")]
        public string country_code { get; set; }

        [DataMember(Name = "cities")]
        public string[] cities { get; set; }

        public string toString()
        {
            string result = "name: "+name+ "\ncommercial_name: "+commercial_name+"\ncountry_code: "+country_code +"\n";
            if (cities!=null)
            {
                result += "Cities:\n";
                foreach (string city in cities)
                {
                    result += "  - " + city + "\n";
                }
            }
            return result;
        }
    }
}
