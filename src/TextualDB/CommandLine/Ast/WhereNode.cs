using System;
using System.Collections.Generic;
using System.Linq;

using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class WhereNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public List<FilterNode> Filters { get; private set; }

        public WhereNode(SourceLocation location, IEnumerable<FilterNode> filters)
        {
            SourceLocation = location;
            Filters = filters.ToList();
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
