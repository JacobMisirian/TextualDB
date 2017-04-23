using System.Collections.Generic;

using TextualDB.Exceptions;

namespace TextualDB.Components
{
    public class TextualRow
    {
        public TextualTable Owner { get; private set; }

        public Dictionary<string, TextualData> Values { get; private set; }

        public TextualRow(TextualTable owner)
        {
            Owner = owner;

            Values = new Dictionary<string, TextualData>();
        }

        private int columnPos = -1;

        public void StartAutoValueAdding()
        {
            columnPos = 0;
            foreach (var column in Owner.Columns)
                Values.Add(column, new TextualData(TextualDataType.Null, string.Empty));
        }
        public void EndAutoValueAdding()
        {
            columnPos = -1;
        }

        public TextualRow AddValue(string value)
        {
            Values[Owner.Columns[columnPos++]] = new TextualData(TextualDataType.String, value);
            return this;
        }
        public TextualRow AddValue(string column, string value)
        {
            Values[column] = new TextualData(TextualDataType.String, value);
            return this;
        }

        public TextualRow ChangeColumnName(string oldName, string newName)
        {
            var temp = Values[oldName];
            Values.Remove(oldName);
            Values.Add(newName, temp);

            return this;
        }

        public TextualData GetValue(string column)
        {
            if (!Owner.ContainsColumn(column))
                throw new ColumnNotFoundException(column, Owner);

            return Values[column];
        }

        public TextualRow RemoveValue(string column)
        {
            Values.Remove(column);
            return this;
        }
    }
}
