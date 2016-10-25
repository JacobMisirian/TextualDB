using System;
using System.IO;

namespace TextualDBD
{
    public class TextualDBDArgumentParser
    {
        private int position;
        private string[] args;

        public TextualDBDConfig Parse(string[] args)
        {
            if (args.Length == 0)
                displayHelp();
            this.args = args;
            TextualDBDConfig config = new TextualDBDConfig();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position])
                {
                    case "-a":
                    case "--accounts":
                        config.UsersFile = expectData("accounts file");
                        if (!File.Exists(config.UsersFile))
                            die(string.Format("Accounts file {0} does not exist!", config.UsersFile));
                        break;
                    case "-d":
                    case "--database":
                    case "--database-file":
                        config.DatabaseFile = expectData("database file");
                        if (!File.Exists(config.DatabaseFile))
                            die(string.Format("Database file {0} does not exist!"));
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-s":
                    case "--server":
                        config.TextualDBDInterfaceType = TextualDBDConfig.InterfaceType.Server;
                        config.Port = Convert.ToInt32(expectData("port"));
                        break;
                    case "-t":
                    case "--tui":
                    case "--terminal":
                        config.TextualDBDInterfaceType = TextualDBDConfig.InterfaceType.TUI;
                        break;
                    default:
                        die(string.Format("Unknown floating data or flag {0}!", args[position - 1]));
                        break;
                }
            }
            return config;
        }

        private string expectData(string type)
        {
            if (args[++position].StartsWith("-"))
                die(string.Format("Expected data type {0}, instead got flag {1}!", type, args[position]));
            return args[position];
        }

        private void displayHelp()
        {
            Console.WriteLine("-a --accounts [FILE]                   Specifies the file with the TextualDBD accounts.");
            Console.WriteLine("-d --database --database-file [FILE]   Specifies the database file to use.");
            Console.WriteLine("-h --help                              Displays this help and exits.");
            Console.WriteLine("-s --server [PORT]                     Specifies the port for the server to listen on.");
            Console.WriteLine("-t --tui --terminal                    Starts in single-user terminal mode.");
            die(string.Empty);
        }

        private void die(string msg)
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }
    }
}

