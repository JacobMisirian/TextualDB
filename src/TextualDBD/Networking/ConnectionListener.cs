using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using TextualDBD.Events;

namespace TextualDBD.Networking
{
    public class ConnectionListener
    {
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<ClientMessageRecievedEventArgs> ClientMessageRecieved;

        private TcpListener listener;

        public ConnectionListener(string ip, int port)
        {
            listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        private Thread listenForConnectionsThread;

        public void Start()
        {
            listener.Start();
            listenForConnectionsThread = new Thread(() => listenForConnections());
            listenForConnectionsThread.Start();
        }

        private void listenForConnections()
        {
            while (true)
            {
                try
                {
                    Client client = new Client(listener.AcceptTcpClient());
                    client.SendThread = new Thread(() => sendPing(client));
                    client.SendThread.Start();
                    client.ListenThread = new Thread(() => listenForMessagesFromClient(client));
                    client.ListenThread.Start();

                    OnClientConnected(new ClientConnectedEventArgs { Client = client });
                }
                catch {}
            }
        }

        private void listenForMessagesFromClient(Client client)
        {
            try
            {
                while (true)
                {
                    string message = client.ReadLine();
                    if (message.Trim() == "PONG")
                        client.Ping = 0;
                    else
                        OnClientMessageRecieved(new ClientMessageRecievedEventArgs { Client = client, Message = message });
                    Thread.Sleep(20);
                }
            }
            catch (IOException)
            {
                OnClientDisconnected(new ClientDisconnectedEventArgs { Client = client });
            }
        }

        private void sendPing(Client client)
        {
            try
            {
                while (client.Ping <= 10000)
                {
                    client.WriteLine("PING");
                    client.Ping += 1000;
                    Thread.Sleep(1000);
                }
            }
            catch (IOException)
            {
                OnClientDisconnected(new ClientDisconnectedEventArgs { Client = client });
            }
            OnClientDisconnected(new ClientDisconnectedEventArgs { Client = client } );
        }

        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            var handler = ClientConnected;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnClientDisconnected(ClientDisconnectedEventArgs e)
        {
            var handler = ClientDisconnected;
            if (handler != null)
                handler(this, e);
        }
        protected virtual void OnClientMessageRecieved(ClientMessageRecievedEventArgs e)
        {
            var handler = ClientMessageRecieved;
            if (handler != null)
                handler(this, e);
        }

        public void Stop()
        {
            listener.Stop();
            listenForConnectionsThread.Abort();
        }
    }
}