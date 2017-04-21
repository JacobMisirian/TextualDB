using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Deserializer
{
    public class SourceLocation
    {
        public string File { get; private set; }

        public int Line { get; private set; }
        public int Position { get; private set; }

        public SourceLocation(string file, int line, int position)
        {
            File = file;

            Line = line;
            Position = position;
        }
    }
}
