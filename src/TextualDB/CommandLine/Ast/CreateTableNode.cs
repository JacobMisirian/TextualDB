using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class CreateTableNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string Table { get; private set; }
        public ListNode Columns { get; private set; }

        public CreateTableNode(SourceLocation location, string table, ListNode columns)
        {
            SourceLocation = location;

            Table = table;
            Columns = columns;
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
