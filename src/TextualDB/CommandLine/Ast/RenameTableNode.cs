using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class RenameTableNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string OldName { get; private set; }
        public string NewName { get; private set; }

        public RenameTableNode(SourceLocation location, string oldName, string newName)
        {
            SourceLocation = location;

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
