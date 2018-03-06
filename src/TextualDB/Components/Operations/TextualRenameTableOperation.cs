using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components.Operations
{
    public class TextualRenameTableOperation : TextualOperation
    {
        public override TextualTable Result => throw new NotImplementedException();

        private TextualTable table;

        public TextualRenameTableOperation(TextualTable table)
        {
            this.table = table;
        }

        public void Execute(string newName)
        {
            table.Rename(newName);
        }
    }
}
