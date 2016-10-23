using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using TextualDBC.Events;

namespace TextualDBC.Networking
{
    public class TextualDBConnection
    {
        public event EventHandler<ServerConnectedEventArgs> ServerConnected;
        public event EventHandler<ServerDisconnectedEventArgs> ServerDisconnected;
        public event EventHandler<ServerMessageRecievedEventArgs> ServerMessageRecieved;

        public string IP { get; private set; }
        public int Port { get; private set; }

        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;

        public TextualDBConnection(string ip, int port)
        {
            IP = ip;
            Port = port;
        }

        public void Connect()
        {
            client = new TcpClient(IP, Port);
            while (!client.Connected) Thread.Sleep(10);

            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());

            new Thread(() => listenThread()).Start();
        }

        public void Send(string command)
        {
            writer.Write(command);
            writer.Flush();
        }

        private void listenThread()
        {
            while (true)
            {
                string msg = reader.ReadString();
                if (msg.Trim() == "PING")
                    Send("PONG");
                else
                    OnServerMessageRecieved(new ServerMessageRecievedEventArgs { Connection = this, Message = msg });
            }
        }

        public virtual void OnServerConnected(ServerConnectedEventArgs e)
        {
            var handler = ServerConnected;
            if (handler != null)
                handler(this, e);
        }
        public virtual void OnServerDisconnected(ServerDisconnectedEventArgs e)
        {
            var handler = ServerDisconnected;
            if (handler != null)
                handler(this, e);
        }
        public virtual void OnServerMessageRecieved(ServerMessageRecievedEventArgs e)
        {
            var handler = ServerMessageRecieved;
            if (handler != null)
                handler(this, e);
        }
    }
}
