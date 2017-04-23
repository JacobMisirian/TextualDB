using TextualDB.Deserializer;

namespace TextualDB.CommandLine
{
    public abstract class AstNode
    {
        public abstract SourceLocation SourceLocation { get; }
        public abstract void Visit(IVisitor visitor);
        public abstract void VisitChildren(IVisitor visitor);
    }
}
