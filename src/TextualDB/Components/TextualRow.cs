using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void AddValue(string value)
        {
            Values[Owner.Columns[columnPos++]] = new TextualData(TextualDataType.String, value);
        }
        public void AddValue(string column, string value)
        {
            Values[column] = new TextualData(TextualDataType.String, value);
        }
    }
}
