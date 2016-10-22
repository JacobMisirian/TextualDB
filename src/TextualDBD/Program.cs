using System;

using TextualDB;
using TextualDB.Exceptions;

using TextualDBD.Exceptions;
using TextualDBD.Interpreter;
using TextualDBD.Interpreter.Ast;

namespace TextualDBD
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Tokenizer tokenizer = new Tokenizer();
            Parser parser = new Parser();
            CommandEvaluator evaluator = new CommandEvaluator(args[0]);

            while (true)
            {
                try
                {
                    Console.Write(">");
                    Console.WriteLine(evaluator.Execute(parser.Parse(tokenizer.Scan(Console.ReadLine()))));
                    evaluator.WriteChanges();
                }
                catch (ParserExpectedTokenException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ParserUnexpectedTokenException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (TableNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ColumnOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static int indent = 0;
        static void printAst(AstNode node)
        {
            for (int i = 0; i < indent; i++) Console.Write("   ");
            Console.WriteLine(node != null ? node.GetType().Name : "null");
            indent++;
            foreach (AstNode child in node.Children)
                printAst(child);
            indent--;
        }
    }
}
