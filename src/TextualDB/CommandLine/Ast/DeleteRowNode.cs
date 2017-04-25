using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class DeleteRowNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string Table { get; private set; }

        public ListNode Positions { get; private set; }
        public WhereNode Where { get; private set; }

        public DeleteRowNode(SourceLocation location, string table,  ListNode positions)
        {
            SourceLocation = location;

            Table = table;

            Positions = positions;
        }
        public DeleteRowNode(SourceLocation location, string table, WhereNode where)
        {
            SourceLocation = location;

            Table = table;

            Where = where;
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
