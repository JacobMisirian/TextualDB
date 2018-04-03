using System;
using System.Linq;

using TextualDB.CommandLine.Exceptions;
using TextualDB.Components;
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
                        handleResult(TextualOperation.ExecuteOperation(state.Database, line));

                    if (state.PersistChanges)
                        state.SaveDatabase();
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

        private void handleResult(TextualTable table)
        {
            if (table == null) return;

            if (table.Rows.Count > 15)
            {
                Console.WriteLine("Result table has more than 15 values. Display (y/N)? ");
                string res = Console.ReadLine();
                if (res.ToUpper() == "Y" || res.ToUpper() == "YES")
                    Console.WriteLine(table);
            }
            else
                Console.WriteLine(table);
        }

        private void handleUICommand(string[] parts)
        {
            switch (parts[0].ToUpper())
            {
                case "OPEN":
                    bool result = state.OpenDatabase(parts[1]);
                    if (result)
                        Console.WriteLine("Opened database {0}.", parts[1]);
                    else
                        Console.WriteLine("Failed to open database {0}.", parts[1]);
                    break;
                case "PERSIST":
                    if (parts[1].ToUpper() == "TRUE")
                    {
                        Console.WriteLine("Turned persist on.");
                        state.PersistChanges = true;
                    }
                    else if (parts[1].ToUpper() == "FALSE")
                    {
                        Console.WriteLine("Turned persist off.");
                        state.PersistChanges = false;
                    }
                    else
                        Console.WriteLine("Expected 'TRUE' or 'FALSE'!");
                    break;
                case "SAVE":
                    state.SaveDatabase();
                    break;
            }
        }
    }
}
