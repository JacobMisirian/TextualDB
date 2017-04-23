using System.Collections.Generic;
using System.Linq;

using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class ListNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public List<AstNode> Elements { get; private set; }

        public ListNode(SourceLocation location, IEnumerable<AstNode> elements)
        {
            SourceLocation = location;

            Elements = elements.ToList();
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            foreach (var element in Elements)
                element.Visit(visitor);
        }
    }
}
