using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WebProxy
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Uri httpUrl = new Uri("http://localhost:8083/Design_Time_Addresses/WebProxyService/RoutingProxy/");
            ServiceHost host = new ServiceHost(typeof(RoutingProxy), httpUrl);

            //Add a service endpoint
            host.AddServiceEndpoint(typeof(IRoutingService), new WSHttpBinding(), "");

            //Enable metadata exchange
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            //Start the Service
            host.Open();

            Console.WriteLine("Proxy is hosted at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");
            Console.ReadLine();
        }
    }
}
