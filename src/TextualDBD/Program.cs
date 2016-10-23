using System;

using TextualDBD.Interfaces;

namespace TextualDBD
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args[0] == "-t")
                TUI.StartTUI(args[1]);
            if (args[0] == "-n")
                NetUI.StartNetUI(args[1], args[2], Convert.ToInt32(args[3]));
        }
    }
}
