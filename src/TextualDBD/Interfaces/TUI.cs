using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB;
using TextualDBD.Interpreter;

namespace TextualDBD.Interfaces
{
    public class TUI
    {
        public static void StartTUI(string file)
        {
            Tokenizer tokenizer = new Tokenizer();
            Parser parser = new Parser();
            CommandEvaluator evaluator = new CommandEvaluator(file);

            while (true)
            {
                Console.Write(">");
                Console.WriteLine(evaluator.Execute(parser.Parse(tokenizer.Scan(Console.ReadLine()))));
                evaluator.WriteChanges();
            }
        }
    }
}
