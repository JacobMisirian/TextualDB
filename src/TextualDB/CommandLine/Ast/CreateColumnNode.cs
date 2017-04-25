using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class CreateColumnNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string Column { get; private set; }
        public string Table { get; private set; }
        public int Position { get; private set; }

        public CreateColumnNode(SourceLocation location, string column, string table, int position = -1)
        {
            SourceLocation = location;

            Column = column;
            Table = table;
            Position = position;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }

        public override void VisitChildren(IVisitor visitor)
        {

        }
    }
}
