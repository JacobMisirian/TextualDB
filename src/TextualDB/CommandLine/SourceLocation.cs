namespace TextualDB.CommandLine
{
    public class SourceLocation
    {
        public string Source { get; private set; }

        public int Column { get; private set; }
        public int Row { get; private set; }
        
        public SourceLocation(string source, int row, int column)
        {
            Source = source;

            Column = column;
            Row = row;
        }
    }
}
