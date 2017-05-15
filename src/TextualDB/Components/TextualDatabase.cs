using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;

namespace TextualDB.Components
{
    /// <summary>
    /// Represents a TextualDB Database, containing subordinate tables
    /// </summary>
    public class TextualDatabase
    {
        /// <summary>
        /// Deserializes a TextualDatabase from the given path
        /// </summary>
        /// <param name="filePath">The path on disc of the database</param>
        /// <returns>The resulting TextualDatabase</returns>
        public static TextualDatabase FromFile(string filePath)
        {
            return new TextualParser(new Scanner().Scan(filePath, File.ReadAllText(filePath))).ParseDatabase(filePath);
        }
        /// <summary>
        /// The path on disc of the database
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// The tables contained within the database
        /// </summary>
        public Dictionary<string, TextualTable> Tables { get; private set; }
        /// <summary>
        /// Constructs a new TextualDatabase from the given path
        /// </summary>
        /// <param name="filePath">The path on disc of the database</param>
        public TextualDatabase(string filePath)
        {
            FilePath = filePath;
            Tables = new Dictionary<string, TextualTable>();
        }
        /// <summary>
        /// Constructs a new TextualDatabase from the given path with the given collection of tables
        /// </summary>
        /// <param name="filePath">The path on disc of the database</param>
        /// <param name="tables">The collection of tables for the database to contain</param>
        public TextualDatabase(string filePath, Dictionary<string, TextualTable> tables)
        {
            FilePath = filePath;
            Tables = tables;
        }
        /// <summary>
        /// Adds an existing table to the database
        /// </summary>
        /// <param name="table">The table to be added</param>
        /// <returns>The current instance of TextualDatabase</returns>
        public TextualDatabase AddTable(TextualTable table)
        {
            if (ContainsTable(table.Name))
                throw new TableExistsException(table.Name, this);
            Tables.Add(table.Name, table);
            return this;
        }
        /// <summary>
        /// Adds a new table to the database with the given name
        /// </summary>
        /// <param name="name">The name of the new table</param>
        /// <returns>The current instance of TextualDatabase</returns>
        public TextualDatabase AddTable(string name)
        {
            return AddTable(new TextualTable(name, new List<string>()));
        }
        /// <summary>
        /// Adds a new table to the database with the given name and enumerable collection of column strings
        /// </summary>
        /// <param name="name">The name of the new table</param>
        /// <param name="columns">The string collection of column names for the new table</param>
        /// <returns>The current instance of TextualDatabase</returns>
        public TextualDatabase AddTable(string name, IEnumerable<string> columns)
        {
            return AddTable(new TextualTable(name, columns));
        }
        /// <summary>
        /// Adds a new table to the database with the given name, enumerable collection of column string, and list of rows
        /// </summary>
        /// <param name="name">The name of the new table</param>
        /// <param name="columns">The string collection of column names for the new table</param>
        /// <param name="rows">The list of rows for the new table</param>
        /// <returns>The current instance of TextualDatabase</returns>
        public TextualDatabase AddTable(string name, IEnumerable<string> columns, List<TextualRow> rows)
        {
            return AddTable(new TextualTable(name, columns, rows));
        }
        /// <summary>
        /// Returns true if the database contains a table with the given name
        /// </summary>
        /// <param name="name">The table name to check</param>
        /// <returns>True if the database contains the given table, otherwise; false</returns>
        public bool ContainsTable(string name)
        {
            return Tables.ContainsKey(name);
        }
        /// <summary>
        /// Returns the TextualTable with the given name from the database if said table exists, otherwise throws TableNotFoundException
        /// </summary>
        /// <param name="name">The name of the table to return</param>
        /// <returns>The table of the given name</returns>
        public TextualTable GetTable(string name)
        {
            if (!ContainsTable(name))
                throw new TableNotFoundException(this, name);
            return Tables[name];
        }
        /// <summary>
        /// Removes the table with the given name from the database if said table exists, otherwise throws TableNotFoundException
        /// </summary>
        /// <param name="name">The name of the table to remove</param>
        public void RemoveTable(string name)
        {
            if (!ContainsTable(name))
                throw new TableNotFoundException(this, name);
            Tables.Remove(name);
        }
    }
}
