namespace TextualDB.Components.Operations
{
    public class TextualRenameTableOperation : TextualOperation
    {
        public override TextualTable Result { get; }

        private TextualTable table;

        public TextualRenameTableOperation(TextualTable table)
        {
            this.table = table;

            Result = table;
        }

        public void Execute(string newName)
        {
            table.Rename(newName);
        }
    }
}
