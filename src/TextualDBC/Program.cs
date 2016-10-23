using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using TextualDBC.Events;
using TextualDBC.Networking;

namespace TextualDBC
{
    class Program
    {
        static void Main(string[] args)
        {
            TextualDBConnection connection = new TextualDBConnection(args[0], Convert.ToInt32(args[1]));
            connection.ServerConnected += connection_ServerConnected;
            connection.ServerDisconnected += connection_ServerDisconnected;
            connection.ServerMessageRecieved += connection_ServerMessageRecieved;
            connection.Connect();

            while (true)
            {
                Console.Write(">");
                connection.Send(Console.ReadLine());
                Thread.Sleep(750);
            }
        }

        static void connection_ServerConnected(object sender, ServerConnectedEventArgs e)
        {
            Console.WriteLine("Connected to {0}:{1}!", e.Connection.IP, e.Connection.Port);
        }
        static void connection_ServerDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            Console.WriteLine("Lost connection to {0}:{1}!", e.Connection.IP, e.Connection.Port);
        }
        static void connection_ServerMessageRecieved(object sender, ServerMessageRecievedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
