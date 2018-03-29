using System;
using System.Linq;

using TextualDB.CommandLine.Exceptions;
using TextualDB.Components.Exceptions;
using TextualDB.Components.Operations;
using TextualDB.Components.Operations.Exceptions;

namespace TextualDB.CommandLine
{
    public class UI
    {
        private UIState state;

        public UI()
        {
            state = new UIState();
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    Console.Write(state.Prompt);
                    string line = Console.ReadLine();
                    string[] parts = line.Split(' ');
                    if (parts[0].ToUpper() == "UI")
                        handleUICommand(parts.Skip(1).ToArray());
                    else
                        Console.WriteLine(TextualOperation.ExecuteOperation(state.Database, line));
                }
                catch (CommandLineException cle)
                {
                    Console.WriteLine(cle.Message);
                }
                catch (ComponentException ce)
                {
                    Console.WriteLine(ce.Message);
                }
                catch (OperationException oe)
                {
                    Console.WriteLine(oe.Message);
                }
            }
        }
        public void Run(string path)
        {
            state.OpenDatabase(path);
            Run();
        }

        private void handleUICommand(string[] parts)
        {
            switch (parts[0].ToUpper())
            {
                case "OPEN":
                    bool result = state.OpenDatabase(parts[1]);
                    if (result)
                        Console.WriteLine("Opened database {0}", parts[1]);
                    else
                        Console.WriteLine("Failed to open database {0}", parts[1]);
                    break;
                    
            }
        }
    }
}
