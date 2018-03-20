using System;

namespace TextualDB.Components.Operations
{
    public class TextualRenameColumnOperation : TextualOperation
    {
        public override TextualTable Result { get; }

        private TextualTable table;

        public TextualRenameColumnOperation(TextualTable table)
        {
            this.table = table;

            Result = table;
        }

        public void Execute(string oldName, string newName)
        {
            table.RenameColumn(oldName, newName);
        }
    }
}
