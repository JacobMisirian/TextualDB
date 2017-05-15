using System.Collections.Generic;

using TextualDB.Exceptions;

namespace TextualDB.Components
{
    public class TextualRow
    {
        /// <summary>
        /// The TextualTable that the row belongs to
        /// </summary>
        public TextualTable Owner { get; private set; }
        /// <summary>
        /// A collection representing the columns and corresponding values
        /// </summary>
        public Dictionary<string, string> Values { get; private set; }
        /// <summary>
        /// Constructs a new TextualRow with the given owner table
        /// </summary>
        /// <param name="owner">The table for the new row to belong to</param>
        public TextualRow(TextualTable owner)
        {
            Owner = owner;

            Values = new Dictionary<string, string>();
        }
        /// <summary>
        /// Changes the owner of the row to a new table
        /// </summary>
        /// <param name="newOwner">The new table owner for this row to belong to</param>
        /// <returns>The current instance of TextualRow</returns>
        public TextualRow ChangeOwner(TextualTable newOwner)
        {
            Owner = newOwner;
            
            return this;
        }

        private int columnPos = -1;
        /// <summary>
        /// Enters automatic value adding mode, to be used before TextualRow.AddValue(string)
        /// </summary>
        public void StartAutoValueAdding()
        {
            columnPos = 0;
            foreach (var column in Owner.Columns)
                Values.Add(column, string.Empty);
        }
        /// <summary>
        /// Resets automatic value adding mode, to be used after TextualRow.AddValue(string)
        /// </summary>
        public void EndAutoValueAdding()
        {
            columnPos = -1;
        }
        /// <summary>
        /// Sets the given value of the next column, to be used after TextualRow.StartAutoValueAdding()
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>The current instance of TextualRow</returns>
        public TextualRow AddValue(string value)
        {
            Values[Owner.Columns[columnPos++]] = value;
            return this;
        }
        /// <summary>
        /// Sets the given value of the given column
        /// </summary>
        /// <param name="column">The column to set</param>
        /// <param name="value">The value to set the column to</param>
        /// <returns>The current instance of TextualRow</returns>
        public TextualRow AddValue(string column, string value)
        {
            Values[column] = value;
            return this;
        }
        /// <summary>
        /// Changes the column name within the row, to be used internally
        /// </summary>
        /// <param name="oldName">The name of the column to be changed</param>
        /// <param name="newName">The new name for the column to be changed to</param>
        /// <returns></returns>
        public TextualRow ChangeColumnName(string oldName, string newName)
        {
            var temp = Values[oldName];
            Values.Remove(oldName);
            Values.Add(newName, temp);

            return this;
        }
        /// <summary>
        /// Returns value of the given column within the row if the column exists, otherwise throws ColumnNotFoundException
        /// </summary>
        /// <param name="column">The name of the column to return</param>
        /// <returns>The value at the given column</returns>
        public string GetValue(string column)
        {
            if (!Owner.ContainsColumn(column))
                throw new ColumnNotFoundException(column, Owner);

            return Values[column];
        }
        /// <summary>
        /// Removes the column within the row, to be used internally
        /// </summary>
        /// <param name="column">The name of the column to be removed</param>
        /// <returns>The current instance of TextualRow</returns>
        public TextualRow RemoveColumn(string column)
        {
            Values.Remove(column);
            return this;
        }
    }
}
