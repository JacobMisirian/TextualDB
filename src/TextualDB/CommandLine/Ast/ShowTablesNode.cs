using System;
using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class ShowTablesNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public ShowTablesNode(SourceLocation location)
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
