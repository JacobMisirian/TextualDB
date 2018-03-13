namespace TextualDB.CommandLine.Parser.AST
{
    public abstract class AstNode
    {
        public abstract SourceLocation SourceLocation { get; }

        public abstract void Visit(IVisitor visitor);
    }
}
