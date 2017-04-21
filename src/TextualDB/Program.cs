using System;
using System.IO;

using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;
using TextualDB.Serializer;

namespace TextualDB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var tokens = new Scanner().Scan(args[0], File.ReadAllText(args[0]));
                var database = new TextualParser(tokens).ParseDatabase(args[0]);

                new TextualSerializer().SerializeDatabase(args[1], database);
            }
            catch (DeserializerException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
