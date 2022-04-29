using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.JCDcauxModels
{
    [DataContract(Name = "Position")]
    public class Position
    {
        [DataMember(Name = "lat")]
        public float lat { get; set; }

        [DataMember(Name = "lng")]
        public float lng { get; set; }

        public Position()
        {
            this.lat = 0;
            this.lng = 0;
        }

        public Position(double latitude, double longitude)
        {
            this.lat = (float)latitude;
            this.lng = (float)longitude;
        }

        public Position(float latitude, float longitude)
        {
            this.lat = latitude;
            this.lng = longitude;
        }

        public Position(string latitude, string longitude)
        {
            this.lat = float.Parse(latitude);
            this.lng = float.Parse(longitude);
        }

        public string toString()
        {
            return "latitude: "+lat+ ", longitude: "+lng+"\n";
        }
    }
}
