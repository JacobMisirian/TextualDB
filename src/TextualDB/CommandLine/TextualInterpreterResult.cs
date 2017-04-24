using System.Text;

using TextualDB.Components;

namespace TextualDB.CommandLine
{
    public class TextualInterpreterResult
    {
        public TextualTable TableResult { get; set; }
        public StringBuilder TextResult { get; set; }

        public TextualInterpreterResult()
        {
            TextResult = new StringBuilder();
        }
    }
}

