using System;

using TextualDB.CommandLine.Exceptions;
using TextualDB.CommandLine.Lexer;
using TextualDB.CommandLine.Parser;
using TextualDB.Components;

namespace TextualDB.CommandLine
{
    public class UI
    {
        public static void TestCommandPrompt(TextualDatabase database)
        {
            while (true)
            {
                try
                {
                    Console.Write(">");
                    string line = Console.ReadLine();
                    var tokens = new Scanner(line).Scan();
                    var result = new Parser.OperationParser(database, tokens).Parse();
                    if (result != null)
                        Console.WriteLine(result);
                }
                catch (CommandLine.Exceptions.CommandLineException cle)
                {
                    Console.WriteLine(cle.Message);
                }
                catch (Components.Exceptions.ComponentException ce)
                {
                    Console.WriteLine(ce.Message);
                }
                catch (Components.Operations.Exceptions.OperationException oe)
                {
                    Console.WriteLine(oe.Message);
                }
            }
        }
    }
}
