namespace TextualDB.Components.Operations
{
    public class TextualInsertOperation : TextualOperation
    {
        public override TextualTable Result { get { return table; }  }

        private TextualTable table;
        private int location;

        public TextualInsertOperation(TextualTable table)
        {
            this.table = table;
            location = -1;
        }
        public TextualInsertOperation(TextualTable table, int index)
        {
            this.table = table;
            location = index;
        }

        private TextualRow row;
        public TextualInsertOperation BuildRow(string column, object value)
        {
            if (row == null)
                row = new TextualRow(table);
            row.SetValue(column, value);

            return this;
        }

        public void Execute()
        {
            Execute(row);
        }
        public void Execute(TextualRow row)
        {
            if (location == -1)
                table.AddRow(new TextualRow(row, table));
            else
                table.AddRow(new TextualRow(row, table), location);
        }
    }
}
