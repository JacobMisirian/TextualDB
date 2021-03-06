﻿using System;

namespace TextualDB.Components.Operations
{
    public class TextualDeleteTableOperation : TextualOperation
    {
        public override TextualTable Result { get; }

        private TextualDatabase database;

        public TextualDeleteTableOperation(TextualDatabase database)
        {
            this.database = database;
        }

        public void Execute(string name)
        {
            database.RemoveTable(name);
        }
    }
}
