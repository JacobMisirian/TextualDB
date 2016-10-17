using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TextualDBD.Networking
{
    public class Client
    {
        public TcpClient TcpClient { get; private set; }
        public StreamReader StreamReader { get; private set; }
        public StreamWriter StreamWriter { get; private set; }

        public Thread SendThread{ get; set; }
        public Thread ListenThread { get; set; }

        public int Ping { get; set; }

        public Client(TcpClient client)
        {
            TcpClient = client;
            StreamReader = new StreamReader(client.GetStream());
            StreamWriter = new StreamWriter(client.GetStream());
        }

        public void WriteLine(string text)
        {
            StreamWriter.WriteLine(text);
            StreamWriter.Flush();
        }

        public string ReadLine()
        {
            return StreamReader.ReadLine();
        }
    }
}

