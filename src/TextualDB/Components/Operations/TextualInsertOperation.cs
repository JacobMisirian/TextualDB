namespace TextualDB.Components.Operations
{
    public class TextualInsertOperation : TextualOperation
    {
        public override TextualTable Result { get { return table; }  }

        private TextualTable table;

        public TextualInsertOperation(TextualTable table)
        {
            this.table = table;
        }

        private TextualRow row;
        public TextualInsertOperation BuildRow(string column, object value)
        {
            if (row == null)
                row = new TextualRow(table);
            row.SetValue(column, value);

            return this;
        }
        
        public void Execute(int index = -1)
        {
            if (row == null)
                return;
            if (index == -1)
                Execute(row);
            else
                Execute(row, index);
        }
        public void Execute(TextualRow row)
        {
            table.AddRow(row);
        }
        public void Execute(TextualRow row, int index)
        {
            table.AddRow(row, index);
        }
    }
}
