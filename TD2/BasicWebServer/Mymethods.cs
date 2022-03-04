using System;
using System.Diagnostics;
using System.IO;

namespace BasicWebServer
{
    internal class Mymethods
    {
        public string MyMethod1(string param1, string param2)
        {
            return "<HTML><BODY> MyMethod1 : " + param1 + " et " + param2 + "</BODY></HTML>";
        }

        public string MyMethod2(string param1, string param2, string param3)
        {
            return "<HTML><BODY> MyMethod2 : " + param1 + ", " + param2 + " et " + param3 +"</BODY></HTML>";
        }

        public string MyMethod3(string parametres)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"../../../ExecTest/bin/Debug/ExecTest.exe";

            /*string mesParameters = "";

            foreach (string param in parametres)
            {
                mesParameters += param + " ";
            }
            
            start.Arguments = mesParameters;
            */

            start.Arguments = parametres;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            
            // Start the process.
            string result = "";
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
    }
}
