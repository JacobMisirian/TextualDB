using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class SelectNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public ListNode Columns { get; private set; }
        public string Table { get; private set; }

        public WhereNode Where { get; private set; }

        public SelectNode(SourceLocation location, ListNode columns, string table, WhereNode where)
        {
            SourceLocation = location;

            Columns = columns;
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
