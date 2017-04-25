using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class RenameColumnNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string Table { get; private set; }

        public string OldName { get; private set; }
        public string NewName { get; private set; }

        public RenameColumnNode(SourceLocation location, string table, string oldName, string newName)
        {
            SourceLocation = location;

            Table = table;

            OldName = oldName;
            NewName = newName;
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
