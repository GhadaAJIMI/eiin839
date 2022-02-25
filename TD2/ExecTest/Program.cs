using System;

namespace ExeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
                Console.WriteLine("<HTML><BODY> Appel d'un executable extérieur avec un paramétre "+args[0]+" </BODY></HTML>");
            else
                Console.WriteLine("<HTML><BODY> Appel d'un executable extérieur sans paramétre </BODY></HTML>");
        }
    }
}
