using System;

using TextualDBD.Networking;

namespace TextualDBD.Events
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public Client Client { get; set; }
    }
}

