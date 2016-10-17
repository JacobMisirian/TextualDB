using System;

using TextualDBD.Networking;

namespace TextualDBD.Events
{
    public class ClientDisconnectedEventArgs : EventArgs
    {
        public Client Client { get; set; }
    }
}