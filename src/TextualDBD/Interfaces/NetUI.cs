using System;
using System.Collections.Generic;
using System.IO;
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
        private static List<Client> authenticatedClients = new List<Client>();
        private static Dictionary<string, string> accounts = new Dictionary<string, string>();

        public static void StartNetUI(string file, string accounts, int port)
        {
            interpreter = new CommandEvaluator(file);
            readAccounts(accounts);
            ConnectionListener listener = new ConnectionListener(port);
            listener.ClientConnected += listener_ClientConnected;
            listener.ClientDisconnected += listener_ClientDisconnected;
            listener.ClientMessageRecieved += listener_ClientMessageRecieved;

            listener.Start();
            Thread.Sleep(Timeout.Infinite);
        }

        private static void readAccounts(string file)
        {
            foreach (string line in File.ReadAllLines(file))
            {
                string[] parts = line.Split(' ');
                if (parts.Length >= 2)
                    accounts.Add(parts[0], parts[1]);
            }
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
            if (authenticatedClients.Contains(e.Client))
                authenticatedClients.Remove(e.Client);
            e.Client = null;
        }
        private static void listener_ClientMessageRecieved(object sender, ClientMessageRecievedEventArgs e)
        {
            if (e.Message.Trim().StartsWith("auth"))
            {
                string[] parts = e.Message.Trim().Split(' ');
                if (parts.Length >= 3)
                {
                    if (!accounts.ContainsKey(parts[1]))
                    {
                        e.Client.WriteLine("Invalid username! Disconnecting...");
                        Thread.Sleep(100);
                        listener_ClientDisconnected(sender, new ClientDisconnectedEventArgs() { Client = e.Client });
                    }
                    else
                    {
                        if (accounts[parts[1]] != parts[2])
                        {
                            e.Client.WriteLine("Invalid password! Disconnecting...");
                            Thread.Sleep(100);
                            listener_ClientDisconnected(sender, new ClientDisconnectedEventArgs() { Client = e.Client });
                        }
                        else
                        {
                            e.Client.WriteLine(string.Format("Authenticated as {0}", parts[1]));
                            authenticatedClients.Add(e.Client);
                            return;
                        }
                    }
                }
            }

            if (!authenticatedClients.Contains(e.Client))
            {
                e.Client.WriteLine("Must authenticate using 'auth <USER> <PASS>' syntax! Disconnecting...");
                Thread.Sleep(100);
                listener_ClientDisconnected(sender, new ClientDisconnectedEventArgs() { Client = e.Client });
            }
            else
                e.Client.WriteLine(interpreter.Execute(parser.Parse(tokenizer.Scan(e.Message))));
        }
    }
}