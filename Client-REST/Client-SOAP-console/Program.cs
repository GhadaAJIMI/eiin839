using System;
using Client_SOAP_console.ServiceReference1;

namespace Client_SOAP_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalculatorSoapClient c = new CalculatorSoapClient();
            int s = c.Add(10, 2);
            Console.WriteLine(s);
            Console.ReadLine();
        }
    }
}
