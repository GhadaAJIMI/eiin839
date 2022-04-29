using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RoutingWithBikes
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Routing)))
            {
                host.Open();
                Console.WriteLine("Routing with bikes is self hosted at {0}", host.BaseAddresses[0]);
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
