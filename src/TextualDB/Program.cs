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
            var interpreter = new TextualInterpreter();
            while (true)
            {
                var database = new TextualParser(new Scanner().Scan(args[0], File.ReadAllText(args[0]))).ParseDatabase(args[0]);
                try
                {
                    Console.Write("> ");
                    string command = Console.ReadLine();
                    if (command.Trim() == string.Empty)
                        continue;

                    var tokens = new Scanner().Scan("stdin", command);
                    var ast = new Parser(tokens).Parse();

                    var result = interpreter.Execute(ast, database);
                    Console.WriteLine(result.TableResult);
                    Console.WriteLine(result.TextResult.ToString());
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
