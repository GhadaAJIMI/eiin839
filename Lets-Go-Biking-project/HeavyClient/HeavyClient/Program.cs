using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HeavyClient.RoutingWithBikes;

namespace HeavyClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            IRouting client = new RoutingClient() ;
            string input;

            do
            {
                Console.Clear();
                Console.WriteLine("Les choix possibles :");
                Console.WriteLine("\t- path : Établir un itinéraire entre 2 adresses");
                Console.WriteLine("\t- quit : Quitter");
                Console.Write("\nChoix : ");
                input = Console.ReadLine().ToLower().Trim();

                if (input.Equals("path"))
                {
                    SearchRoute(client);
                }
                else if (input.Equals("quit"))
                {
                    Console.WriteLine("Fermeture ...");
                    exit();
                }
                else
                {
                    Console.WriteLine("Choix invalide, réessayez");
                    Console.WriteLine("\nAppuyer sur une touche pour revenir au menu principal.");
                    Console.ReadLine();
                }
            } while (!input.Equals("quit"));
        }

        private static void SearchRoute(IRouting client)
        {
            do
            {
                Console.Write("Adresse de départ : ");
                string fromAddress = Console.ReadLine().Trim().Replace(" ", "+");

                Console.Write("Destination : ");
                string toAddress = Console.ReadLine().Trim().Replace(" ", "+");

                if (fromAddress.Equals("null") || toAddress.Equals("null") || fromAddress.Equals(string.Empty) || toAddress.Equals(string.Empty))
                {
                    Console.WriteLine("\nAdresse invalide");
                }
                else
                {
                    Position p1 = client.GetPosition(fromAddress);
                    Position p2 = client.GetPosition(toAddress);

                    float px1 = p1.lat;
                    float py1 = p1.lng;

                    float px2 = p2.lat;
                    float py2 = p2.lng;

                    Double d1 = Math.Sqrt((Math.Pow(px1 - px2, 2) + Math.Pow(py1 - py2, 2)));

                    Station s1 = client.getStation1(px1, py1);
                    Station s2 = client.getStation2(px2, py2);

                    float sx1 = s1.position.lat;
                    float sy1 = s1.position.lng;

                    float sx2 = s2.position.lat;
                    float sy2 = s2.position.lng;

                    Double d2 = Math.Sqrt((Math.Pow(px1 - sx1, 2) + Math.Pow(py1 - sy1, 2)));
                    Double d3 = Math.Sqrt((Math.Pow(px2 - sx2, 2) + Math.Pow(py2 - sy2, 2)));

                    Position[] positions;

                    if (d2 > d1 || d3 > d1)
                    {
                        // parcour à pied
                        positions = new Position[] { p1, p2 };
                    }
                    else
                    {
                        // parcour à velo
                        positions = new Position[] { p1, s1.position, s2.position, p2 };
                    }

                    Stopwatch sw = Stopwatch.StartNew();
                    GeoJson[] paths = client.GetPath(positions);
                    sw.Stop();

                    Console.WriteLine("Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);

                    for(int i = 0; i < paths.Length; i++)
                    {
                        if(i == 0 || i == paths.Length - 1)
                        {
                            Console.WriteLine("\nParcours à Pied:\n");
                        }
                        else
                        {
                            Console.WriteLine("\nParcours à Vélo:\n");
                        }
                        writeSteps(paths[i].features.ToList());
                    }
                }
                Console.WriteLine("\nFélicitation vous êtes arrivé à votre destination !");
                Console.WriteLine("\nAppuyer sur 'Entrée' pour revenir au menu principal.");
            } while (Console.ReadKey().Key != ConsoleKey.Enter);
        }

        private static void writeSteps(List<Feature> features)
        {
            foreach (Feature feature in features)
            {
                foreach (Segment segment in feature.properties.segments)
                {
                    foreach (Step step in segment.steps)
                    {
                        Console.WriteLine(step.instruction);
                    }
                }
            }
        }

        private static void exit()
        {
            Process[] AllProcesses = Process.GetProcessesByName("excel");

            foreach (Process ExcelProcess in AllProcesses)
            {
                ExcelProcess.Kill();
            }

            AllProcesses = null;
        }            
    }
}
