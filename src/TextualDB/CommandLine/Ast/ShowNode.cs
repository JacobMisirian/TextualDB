using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class ShowNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public ShowNode(SourceLocation location)
        {
            SourceLocation = location;
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

