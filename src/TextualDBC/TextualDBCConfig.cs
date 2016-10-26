using System;

namespace TextualDBC
{
    public class TextualDBCConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }

        public TextualDBCConfig()
        {
            Server = "127.0.0.1";
            Port = 1337;
        }
    }
}

