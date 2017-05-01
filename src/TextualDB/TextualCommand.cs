using System;
using System.Collections.Generic;
using System.IO;

using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.CommandLine;
using TextualDB.Components;

namespace TextualDB
{
    public class TextualCommand
    {
        public string CommandString { get; private set; }
        public TextualDatabase Database { get; private set; }
        public Dictionary<int, string> Placeholders { get; private set; }
        
        public TextualCommand(TextualDatabase database, string commandString)
        {
            CommandString = commandString;
            Database = database;
            Placeholders = new Dictionary<int, string>();
        }
        public TextualCommand(string database, string commandString)
        {
            CommandString = commandString;
            Database = new TextualParser(new Scanner().Scan(database, File.ReadAllText(database))).ParseDatabase(database);
        }
        public TextualCommand(TextualDatabase database, string commandString, Dictionary<int, string> placeholders)
        {
            CommandString = commandString;
            Database = database;
            Placeholders = placeholders;
        }
        public TextualCommand(string database, string commandString, Dictionary<int, string> placeholders)
        {
            CommandString = commandString;
            Database = new TextualParser(new Scanner().Scan(database, File.ReadAllText(database))).ParseDatabase(database);
            Placeholders = placeholders;
        }
        
        public TextualCommand AddPlaceholder(int place, string value)
        {
            if (Placeholders.ContainsKey(place))
                Placeholders.Remove(place);
            Placeholders.Add(place, value);
            
            return this;
        }
        
        public TextualInterpreterResult Execute()
        {
            var tokens = new Scanner().Scan("interpreter", CommandString);
            
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].TokenType == TokenType.Placeholder)
                {
                    int place = Convert.ToInt32(tokens[i].Value);
                    
                    if (Placeholders.ContainsKey(place))
                        tokens[i] = new Token(tokens[i].SourceLocation, TokenType.String, Placeholders[place]);
                }
            }
            
            var ast = new Parser(tokens).Parse();
            
            return new TextualInterpreter().Execute(ast, Database);
        }
    }
}