using System;
using System.IO;

using TextualDB.CommandLine;
using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;

namespace TextualDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new TextualParser(new Scanner().Scan(args[0], File.ReadAllText(args[0]))).ParseDatabase(args[0]);

            while (true)
            {
                try
                {
                    Console.Write("> ");
                    string command = Console.ReadLine();

                    var tokens = new Scanner().Scan("stdin", command);
                    var ast = new Parser(tokens).Parse();

                    new AstVisitor(ast, database);
                }
                catch (ColumnExistsException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ColumnNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (CommandLineParseException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (CommandLineVisitorException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DeserializerException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (TableNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
