using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDBC.Networking;

namespace TextualDBC.Events
{
    public class ServerMessageRecievedEventArgs : EventArgs
    {
        public TextualDBConnection Connection { get; set; }
        public string Message { get; set; }
    }
}
