using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    internal class Program
    {
        static TcpClient clientSocket;
        public static void Write()
        {
            BinaryWriter writer = new BinaryWriter(clientSocket.GetStream());

            while (true)
            {
                string str = Console.ReadLine();
                writer.Write(str);
            }
        }
        public static void Read()
        {
            BinaryReader reader = new BinaryReader(clientSocket.GetStream());

            while (true)
            {
                string str = "response: ";
                str = str + reader.ReadString();
                Console.WriteLine(str);
            }

        }

        static void Main(string[] args)
        {
            clientSocket = new TcpClient("localhost", 8080);
            Thread ctThreadWrite = new Thread(Write);
            Thread ctThreadRead = new Thread(Read);
            ctThreadRead.Start();
            ctThreadWrite.Start();
        }
    }
}
