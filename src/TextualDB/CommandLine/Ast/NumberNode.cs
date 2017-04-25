using System;
using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class NumberNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public int Number { get; private set; }

        public NumberNode(SourceLocation location, int number)
        {
            SourceLocation = location;

            Number = number;
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
