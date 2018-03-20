using System;

namespace TextualDB.Components.Operations
{
    public class TextualDeleteColumnOperation : TextualOperation
    {
        public override TextualTable Result => null;

        private TextualTable table;
        
        public TextualDeleteColumnOperation(TextualTable table)
        {
            this.table = table;
        }

        public void Execute(int index)
        {
            table.RemoveColumn(index);
        }
        public void Execute(string name)
        {
            table.RemoveColumn(name);
        }
    }
}
