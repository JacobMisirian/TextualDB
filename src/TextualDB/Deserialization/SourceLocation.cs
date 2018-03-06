namespace TextualDB.Deserialization
{
    public class SourceLocation
    {
        public int Column { get; private set; }
        public int Row { get; private set; }

        public SourceLocation(int row, int column)
        {
            Column = column;
            Row = row;
        }
    }
}
