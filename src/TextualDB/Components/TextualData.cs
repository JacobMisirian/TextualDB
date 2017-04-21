using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components
{
    public class TextualData
    {
        public TextualDataType TextualDataType { get; private set; }
        public string Value { get; private set; }

        public TextualData(TextualDataType dataType, string value)
        {
            TextualDataType = dataType;
            Value = value;
        }
    }
}
