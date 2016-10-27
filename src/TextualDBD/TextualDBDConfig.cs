using System;

using TextualDBD.Interfaces;

namespace TextualDBD
{
    public class TextualDBDConfig
    {
        public void Main()
        {
            if (TextualDBDInterfaceType == InterfaceType.Server)
            {
                if (DatabaseFile == string.Empty || DatabaseFile == null)
                    die("No database file specified!");
                if (UsersFile == string.Empty || UsersFile == null)
                    die("No accounts file specified!");
                NetUI.StartNetUI(DatabaseFile, UsersFile, Port);
            }
            else
                TUI.StartTUI(DatabaseFile);
        }

        private void die(string msg)
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }

        public enum InterfaceType
        { 
            Server,
            TUI
        }

        public InterfaceType TextualDBDInterfaceType { get; set; }
        public int Port { get; set; }
        public string UsersFile { get; set; }
        public string DatabaseFile { get; set; }

        public TextualDBDConfig()
        {
            Port = 1337;
            TextualDBDInterfaceType = InterfaceType.TUI;
        }
    }
}

