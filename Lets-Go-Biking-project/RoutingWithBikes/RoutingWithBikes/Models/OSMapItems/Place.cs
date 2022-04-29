using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.OSMapItems
{
    [DataContract]
    public class Place
    {
        public string display_name { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }

        public double importance { get; set; }
    }
}
