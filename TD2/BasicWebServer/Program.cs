using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Reflection;
using BasicWebServer;
using System.Collections;

namespace BasicServerHTTPlistener
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }

            HttpListener listener = new HttpListener();
            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                    Console.WriteLine("Listening for connections on " + s);
                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();

            Console.CancelKeyPress += delegate {
                listener.Stop();
                listener.Close();
                Environment.Exit(0);
            };

            while (true)
            {
                // Pour la question 1 :
                // http://localhost:8080/jhfkjhqjksfhjksd/MyMethod1?param1=test&param2=1
                // http://localhost:8080/jhfkjhqjksfhjksd/MyMethod2?param1=test2&param2=2&param3=3

                // Pour la question 2 :
                // http://localhost:8080/jhfkjhqjksfhjksd/MyMethod3?param1=test3

                // Pour la question 3 :
                // 

                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }

                // parse path in url 
                foreach (string str in request.Url.Segments)
                {
                    Console.WriteLine(str);
                }

                ArrayList mesValeurs = new ArrayList() {};
                
                string result = "";
                string myMethode = request.Url.Segments[request.Url.Segments.Length - 1];
                Console.WriteLine("myMethode : " + myMethode);

                if (myMethode != null && myMethode != "favicon.ico")
                {
                    foreach (string str in HttpUtility.ParseQueryString(request.Url.Query))
                    {
                        mesValeurs.Add(HttpUtility.ParseQueryString(request.Url.Query).Get(str));
                        Console.WriteLine(HttpUtility.ParseQueryString(request.Url.Query).Get(str));
                    }
                    Type type = typeof(Mymethods);
                    MethodInfo method = type.GetMethod(myMethode);
                    Mymethods c = new Mymethods();
                    result = (string)method.Invoke(c, mesValeurs.ToArray());
                    Console.WriteLine(result);
                }
                else if(myMethode.Equals("favicon.ico"))
                {
                    result = "";
                    Console.WriteLine(result);
                }
                else
                {
                    result = "<HTML><BODY> Veuillez choisir une méthode ! </BODY></HTML>";
                    Console.WriteLine(result);
                }

                if(result != "")
                {
                    HttpListenerResponse response = context.Response;
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(result);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }

        private int incr(int x)
        {
            return x+1;
        }
    }
}