using System.Collections.Generic;

using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class InsertNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public string Table { get; private set; }
        public int Position { get; private set; }
        
        public Dictionary<string, string> Values { get; private set; }

        public InsertNode(SourceLocation location, string table, Dictionary<string, string> values, int position = -1)
        {
            SourceLocation = location;

            Table = table;
            Position = position;

            Values = values;
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
