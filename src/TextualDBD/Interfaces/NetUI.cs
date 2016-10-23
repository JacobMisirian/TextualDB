using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using TextualDB;
using TextualDB.Networking;

using TextualDBD.Events;
using TextualDBD.Exceptions;
using TextualDBD.Interpreter;
using TextualDBD.Networking;

namespace TextualDBD.Interfaces
{
    public class NetUI
    {
        private static Tokenizer tokenizer = new Tokenizer();
        private static Parser parser = new Parser();
        private static CommandEvaluator interpreter;

        public static void StartNetUI(string file, string ip, int port)
        {
            interpreter = new CommandEvaluator(file);
            ConnectionListener listener = new ConnectionListener(ip, port);
            listener.ClientConnected += listener_ClientConnected;
            listener.ClientDisconnected += listener_ClientDisconnected;
            listener.ClientMessageRecieved += listener_ClientMessageRecieved;

            listener.Start();
            Thread.Sleep(Timeout.Infinite);
        }

        private static void listener_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("Client connected!");
        }
        private static void listener_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("Client disconnected!");
            e.Client.SendThread.Abort();
            e.Client.ListenThread.Abort();
            e.Client.TcpClient.Close();
            e.Client = null;
        }
        private static void listener_ClientMessageRecieved(object sender, ClientMessageRecievedEventArgs e)
        {
            Console.WriteLine(e.Message);
            e.Client.WriteLine(interpreter.Execute(parser.Parse(tokenizer.Scan(e.Message))));
        }
    }
}
