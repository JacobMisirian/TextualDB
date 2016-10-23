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
        public BinaryReader BinaryReader { get; private set; }
        public BinaryWriter BinaryWriter { get; private set; }

        public Thread SendThread{ get; set; }
        public Thread ListenThread { get; set; }

        public int Ping { get; set; }

        public Client(TcpClient client)
        {
            TcpClient = client;
            BinaryReader = new BinaryReader(client.GetStream());
            BinaryWriter = new BinaryWriter(client.GetStream());
        }

        public void WriteLine(string text)
        {
            BinaryWriter.Write(text);
            BinaryWriter.Flush();
        }

        public string ReadLine()
        {
            return BinaryReader.ReadString();
        }
    }
}

