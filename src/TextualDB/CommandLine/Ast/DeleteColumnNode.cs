using System;
using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class DeleteColumnNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string Column { get; private set; }
        public string Table { get; private set; }

        public DeleteColumnNode(SourceLocation location, string column, string table)
        {
            SourceLocation = location;

            Column = column;
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
