﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes.Models.JCDcauxModels
{
    [DataContract(Name = "MainStands")]
    public class MainStands
    {
        [DataMember(Name = "availabilities")]
        public Availabilitie availabilities { get; set; }

        [DataMember(Name = "capacity")]
        public int capacity { get; set; }
    }
}
