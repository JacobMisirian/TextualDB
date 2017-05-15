using System;
using System.Collections.Generic;
using System.IO;

using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.CommandLine;
using TextualDB.Components;

namespace TextualDB
{
    /// <summary>
    /// Represents a query or operation to be executed within a TextualDB Database
    /// </summary>
    public class TextualCommand
    {
        /// <summary>
        /// The string value of the query or operation
        /// </summary>
        public string CommandString { get; private set; }
        /// <summary>
        /// The database for the command to be executed inside
        /// </summary>
        public TextualDatabase Database { get; private set; }
        /// <summary>
        /// A collection of prepared statements, where keys are {0}, {1}, {2} and so forth
        /// </summary>
        public Dictionary<int, string> Placeholders { get; private set; }
        /// <summary>
        /// Constructs a new TextualCommand with the given database and command string
        /// </summary>
        /// <param name="database">The database for the command to be executed under</param>
        /// <param name="commandString">The command string</param>
        public TextualCommand(TextualDatabase database, string commandString)
        {
            CommandString = commandString;
            Database = database;
            Placeholders = new Dictionary<int, string>();
        }
        /// <summary>
        /// Constructs a new TextualCommand with the given database path and command string
        /// </summary>
        /// <param name="databasePath">The path to the database for the command to be executed under</param>
        /// <param name="commandString">The command string</param>
        public TextualCommand(string databasePath, string commandString)
        {
            CommandString = commandString;
            Database = TextualDatabase.FromFile(databasePath);
        }
        /// <summary>
        /// Constructs a new TextualCommand with the given database, command string, and collection of placeholders
        /// </summary>
        /// <param name="database">The database for the command to be executed under</param>
        /// <param name="commandString">The command string</param>
        /// <param name="placeholders">The collection of placeholders, where keys are {0}, {1}, {2} and so forth</param>
        public TextualCommand(TextualDatabase database, string commandString, Dictionary<int, string> placeholders)
        {
            CommandString = commandString;
            Database = database;
            Placeholders = placeholders;
        }
        /// <summary>
        /// Constructs a new TextualCommand with the given database path and command string
        /// </summary>
        /// <param name="databasePath">The path to the database for the command to be executed under</param>
        /// <param name="commandString">The command string</param>
        /// <param name="placeholders">The collection of placeholders, where keys are {0}, {1}, {2} and so forth</param>
        public TextualCommand(string database, string commandString, Dictionary<int, string> placeholders)
        {
            CommandString = commandString;
            Database = new TextualParser(new Scanner().Scan(database, File.ReadAllText(database))).ParseDatabase(database);
            Placeholders = placeholders;
        }
        /// <summary>
        /// Adds a new placeholder with the given key and value
        /// </summary>
        /// <param name="place">The index for the placeholder</param>
        /// <param name="value">The value of the placeholder</param>
        /// <returns>The current instance of TextualCommand</returns>
        public TextualCommand AddPlaceholder(int place, string value)
        {
            if (Placeholders.ContainsKey(place))
                Placeholders.Remove(place);
            Placeholders.Add(place, value);
            
            return this;
        }
        /// <summary>
        /// Executes the query or operation within the database and returns a TextualDB table as a result
        /// </summary>
        /// <returns>The resulting TextualDB table</returns>
        public TextualTable Execute()
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