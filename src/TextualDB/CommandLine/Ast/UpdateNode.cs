using System;
using System.Collections.Generic;

using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class UpdateNode : AstNode
    {
        public override SourceLocation SourceLocation { get; set; }

        public string Table { get; private set; }

        public Dictionary<string, string> Values { get; private set; }

        public ListNode Positions { get; private set; }
        public WhereNode Where { get; private set; }

        public UpdateNode(SourceLocation location, string table, Dictionary<string, string> values, ListNode positions)
        {
            SourceLocation = location;

            Table = table;

            Values = values;

            Positions = positions;
        }
        public UpdateNode(SourceLocation location, string table, Dictionary<string, string> values, WhereNode where)
        {
            SourceLocation = location;

            Table = table;

            Values = values;

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
