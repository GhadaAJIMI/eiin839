using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.JCDcauxModels
{
    [DataContract(Name = "Station")]
    public class Station
    {
        [DataMember(Name = "number")]
        public int number { get; set; }

        [DataMember(Name = "contract_name")]
        public string contract_name { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "address")]
        public string address { get; set; }

        [DataMember(Name = "position")]
        public Position position { get; set; }

        [DataMember(Name = "banking")]
        public bool banking { get; set; }

        [DataMember(Name = "bonus")]
        public bool bonus { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        /*[DataMember(Name = "last_update")]
        public DateTime last_update { get; set; }*/

        [DataMember(Name = "connected")]
        public bool connected { get; set; }

        [DataMember(Name = "overflow")]
        public bool overflow { get; set; }

        [DataMember(Name = "shape")]
        public object shape { get; set; }

        [DataMember(Name = "totalStands")]
        public TotalStands totalStands { get; set; }

        [DataMember(Name = "mainStands")]
        public MainStands mainStands { get; set; }

        [DataMember(Name = "overflowStands")]
        public object overflowStands { get; set; }

        [DataMember(Name = "available_bike_stands")]
        public int available_bike_stands { get; set; }

        [DataMember(Name = "available_bikes")]
        public int available_bikes { get; set; }

        [DataMember(Name = "bike_stands")]
        public int bike_stands { get; set; }

        public Station() { }

        public string toString()
        {
            string result = "number: "+number+ "\ncontract_name: "+contract_name+"\nname: "+name +"\n";
            result += "address: "+address+ "\nposition: "+position+"\nbanking: "+banking +"\n";
            result += "bonus: "+bonus+ "\nstatus: "+status+"\n";
            result += "connected: "+connected+ "\noverflow: "+overflow+"\nshape: "+shape +"\noverflowStands: "+overflowStands +"\n";
            result += "available_bike_stands: "+available_bike_stands+ "\navailable_bikes: "+available_bikes+"\n";
            return result;
        }
    }
}
