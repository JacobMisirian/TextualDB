using System;

namespace TextualDBC
{
    public class TextualDBCArgumentParser
    {
        private string[] args;
        private int position;

        public TextualDBCConfig Parse(string[] args)
        {
            if (args.Length <= 0)
                displayHelp();
            this.args = args;
            TextualDBCConfig config = new TextualDBCConfig();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position])
                {
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-p":
                    case "--port":
                        config.Port = Convert.ToInt32(expectData("port"));
                        break;
                    case "-s":
                    case "--server":
                        config.Server = expectData("server");
                        break;
                    default:
                        die(string.Format("Unknown floating data or flag {0}!", args[position]));
                        break;
                }
            }
            return config;
        }

        private string expectData(string type)
        {            
            if (position + 1 >= args.Length)
                die(string.Format("Expected data type {0}.", type));
            if (args[++position].StartsWith("-"))
                die(string.Format("Expected data type {0}, instead got flag {1}!", type, args[position]));
            return args[position];
        }

        private void displayHelp()
        {
            Console.WriteLine("-h --help                Displays this help and exits.");
            Console.WriteLine("-p --port [PORT]         Specifies the port to connect to.");
            Console.WriteLine("-s --server [HOST]       Specifies the host or IP to connect to.");
            die(string.Empty);
        }

        private void die(string msg)
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }
    }
}

