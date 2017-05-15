using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TextualDB.Exceptions;
using TextualDB.Serializer;

namespace TextualDB.Components
{
    /// <summary>
    /// Represents a TextualDB Table, defined by columns and containing subordinate rows
    /// </summary>
    public class TextualTable
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The columns of the table
        /// </summary>
        public List<string> Columns { get; private set; }
        /// <summary>
        /// The length of the list of columns expressed visually
        /// </summary>
        public int ColumnLength { get { int total = 0; foreach (var col in Columns) total += col.Length + 2; return total + 2; } }
        /// <summary>
        /// The rows of the table
        /// </summary>
        public List<TextualRow> Rows { get; private set; }
        /// <summary>
        /// Constructs a new TextualDatabase with the given name and enumerable collection of string column names
        /// </summary>
        /// <param name="name">The name of the new table</param>
        /// <param name="columns">The enumerable collection of string column names for the new table</param>
        public TextualTable(string name, IEnumerable<string> columns)
        {
            Name = name;
            Columns = columns.ToList();
            Rows = new List<TextualRow>();
        }
        /// <summary>
        /// Constructs a new TextualDatabase with the given name, enumerable collection of string column names, and list of rows
        /// </summary>
        /// <param name="name">The name of the new table</param>
        /// <param name="columns">The enumerable collection of string column names for the new table</param>
        /// <param name="rows">The list of rows for the new table</param>
        public TextualTable(string name, IEnumerable<string> columns, List<TextualRow> rows)
        {
            Name = name;
            Columns = columns.ToList();
            Rows = rows;
        }
        /// <summary>
        /// Adds a new column to the table with the given name at the optional index, default to the end
        /// </summary>
        /// <param name="name">The name of the new column</param>
        /// <param name="pos">The optional index parameter for the position of the new column</param>
        /// <returns>The current instance of TextualTable</returns>
        public TextualTable AddColumn(string name, int pos = -1)
        {
            if (ContainsColumn(name))
                throw new ColumnExistsException(name, this);

            pos = pos == -1 ? Columns.Count : pos;

            Columns.Insert(pos, name);

            foreach (var row in Rows)
                row.AddValue(name, string.Empty);

            return this;
        }
        /// <summary>
        /// Adds an existing row to the table at the optional index, default to the end
        /// </summary>
        /// <param name="row">The row to be added</param>
        /// <param name="pos">The optional index parameter for the position of the new row</param>
        public void AddRow(TextualRow row, int pos = -1)
        {
            pos = pos == -1 ? Rows.Count : pos;
            Rows.Insert(pos, row);
        }
        /// <summary>
        /// Adds a new row to the table at the optional index, with a params array of string values
        /// </summary>
        /// <param name="pos">The optional index parameter for the position of the new row</param>
        /// <param name="values">The params array of string values for the row</param>
        public void AddRow(int pos = -1, params string[] values)
        {

            var row = new TextualRow(this);

            row.StartAutoValueAdding();

            foreach (var val in values)
                row.AddValue(val);

            AddRow(row, pos);
        }
        /// <summary>
        /// Changes the name of a column within the table
        /// </summary>
        /// <param name="oldName">The name of the column to be changed</param>
        /// <param name="newName">The new name for the column to be changed to</param>
        /// <returns>The current instance of TextualTable</returns>
        public TextualTable ChangeColumnName(string oldName, string newName)
        {
            if (!ContainsColumn(oldName))
                throw new ColumnNotFoundException(oldName, this);

            int index = Columns.IndexOf(oldName);
            Columns.Remove(oldName);
            Columns.Insert(index, newName);

            foreach (var row in Rows)
                row.ChangeColumnName(oldName, newName);

            return this;
        }
        /// <summary>
        /// Changes the name of the table
        /// </summary>
        /// <param name="newName">The new name of the table</param>
        /// <returns>The current instance of TextualTable</returns>
        public TextualTable ChangeTableName(string newName)
        {
            Name = newName;
            return this;
        }
        /// <summary>
        /// Returns true if the table contains a column with the given name
        /// </summary>
        /// <param name="name">The column name to check</param>
        /// <returns>True if the table contains the given column, otherwise; false</returns>
        public bool ContainsColumn(string name)
        {
            return Columns.Contains(name);
        }
        /// <summary>
        /// Returns the row in the table at the given index if index exists, otherwise throws RowNotFoundException
        /// </summary>
        /// <param name="pos">The position in the table of the desired row</param>
        /// <returns>The row at the given position</returns>
        public TextualRow GetRow(int pos)
        {
            if (pos < 0 || pos >= Rows.Count)
                throw new RowNotFoundException(this, pos);
            return Rows[pos];
        }
        /// <summary>
        /// Removes the column with the given name from the table if the column exists, otherwise throws ColumnNotFoundException
        /// </summary>
        /// <param name="name">The name of the table to remove</param>
        public void RemoveColumn(string name)
        {
            if (!ContainsColumn(name))
                throw new ColumnNotFoundException(name, this);

            Columns.Remove(name);

            foreach (var row in Rows)
                row.RemoveColumn(name);
        }
        /// <summary>
        /// Removes the given row from the table if the row belongs to this table, otherwise throws RowNotFoundException
        /// </summary>
        /// <param name="row">The row to remove from the table</param>
        public void RemoveRow(TextualRow row)
        {
            if (!Rows.Contains(row))
                throw new RowNotFoundException(this, row);
            Rows.Remove(row);
        }
        /// <summary>
        /// Returns a new table based on the current table that only contains columns from the given params string array of column names. If none given, returns all columns
        /// </summary>
        /// <param name="columns">The params string array of columns for the new table, none given returns all</param>
        /// <returns>The resulting table</returns>
        public TextualTable Select(params string[] columns)
        {
            if (columns.Length == 0)
                return this;

            TextualTable ret = new TextualTable(Name, columns);

            foreach (var row in Rows)
            {
                var row_ = new TextualRow(ret);
                foreach (string column in columns)
                {
                    if (!row.Values.ContainsKey(column))
                        throw new ColumnNotFoundException(column, this);
                    row_.AddValue(column, row.Values[column]);
                }
                ret.AddRow(row_);
            }

            return ret;
        }
        /// <summary>
        /// Serializes the table and returns the string value
        /// </summary>
        /// <returns>The string representation of the table</returns>
        public override string ToString()
        {
            MemoryStream mstream = new MemoryStream();

            new TextualSerializer().SerializeDatabase(mstream, new TextualDatabase(string.Empty, new Dictionary<string, TextualTable> { { Name, this } }));

            return ASCIIEncoding.ASCII.GetString(mstream.ToArray());
        }
    }
}