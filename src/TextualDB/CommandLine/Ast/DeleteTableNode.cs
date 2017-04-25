using System;
using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class DeleteTableNode : AstNode
    {

        public override SourceLocation SourceLocation { get; set; }

        public string Table { get; private set; }

        public DeleteTableNode(SourceLocation location, string table)
        {
            SourceLocation = location;

            Table = table;
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
