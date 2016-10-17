using System;

using TextualDBD.Networking;

namespace TextualDBD.Events
{
    public class ClientMessageRecievedEventArgs : EventArgs
    {
        public Client Client { get; set; }
        public string Message { get; set; }
    }
}

