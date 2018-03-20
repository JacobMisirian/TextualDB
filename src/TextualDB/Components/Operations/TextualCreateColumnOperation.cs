namespace TextualDB.Components.Operations
{
    public class TextualCreateColumnOperation : TextualOperation
    {
        public override TextualTable Result => null;

        private TextualTable table;

        public TextualCreateColumnOperation(TextualTable table)
        {
            this.table = table;
        }

        public void Execute(string name, int index = -1)
        {
            table.AddColumn(name, index);
        }
    }
}
