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
                Console.Write(">");
                string line = Console.ReadLine();
                var tokens = new Scanner(line).Scan();
                var result = new Parser.Parser(database, tokens).Parse();
                if (result != null)
                    Console.WriteLine(result);
            }
        }
    }
}
