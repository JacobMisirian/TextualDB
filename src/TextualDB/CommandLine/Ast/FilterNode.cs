using TextualDB.Deserializer;

namespace TextualDB.CommandLine.Ast
{
    public class FilterNode : AstNode
    {
        public override SourceLocation SourceLocation { get; }

        public string Column { get; private set; }
        public string Target { get; private set; }

        public TextualFilterType FilterType { get; private set; }

        public FilterNode(SourceLocation location, string column, string target, TextualFilterType filterType)
        {
            SourceLocation = location;

            Column = column;
            Target = target;

            FilterType = filterType;
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {

        }
    }

    public enum TextualFilterType
    {
        Equal,
        NotEqual,
        Greater,
        GreaterOrEqual,
        Lesser,
        LesserOrEqual
    }
}
