using System;
using System.IO;

using TextualDB.CommandLine;
using TextualDB.Components;

namespace TextualDB
{
    class Program
    {
        static void Main(string[] args)
        {
            UI ui = new UI();
            if (args.Length > 0)
                ui.Run(args[0]);
            else
                ui.Run();
        }
    }
}
