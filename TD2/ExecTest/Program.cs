using System;

namespace ExeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length >= 1)
            {
                string arguments = "";
                foreach(string arg in args)
                {
                    arguments += arg + " ";
                }
                Console.WriteLine("<HTML><BODY> Appel d'un executable extérieur avec un paramétre "+arguments+"</BODY></HTML>");
            }
            else
                Console.WriteLine("<HTML><BODY> Appel d'un executable extérieur sans paramétre </BODY></HTML>");
        }
    }
}
