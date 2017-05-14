using System;
using System.IO;

using TextualDB.Components;
using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;

namespace TextualDB
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var database = TextualDatabase.FromFile(args[0]);
                try
                {
                    Console.Write("> ");
                    string input = Console.ReadLine();
                    if (input.Trim() == string.Empty)
                        continue;

                    var command = new TextualCommand(database, input);

                    var result = command.Execute();
                    Console.WriteLine(result);
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
                catch (CommandLineInterpreterException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DeserializerException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (RowNotFoundException ex)
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
