namespace TextualDB.Components.Operations
{
    public class TextualCreateTableOperation : TextualOperation
    {
        public override TextualTable Result { get; }

        public TextualCreateTableOperation(TextualDatabase database, string name)
        {
            Result = new TextualTable(database, name);
        }
    }
}
