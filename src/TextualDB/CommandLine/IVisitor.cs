using TextualDB.CommandLine.Ast;

namespace TextualDB.CommandLine
{
    public interface IVisitor
    {
        void Accept(FilterNode node);
        void Accept(IdentifierNode node);
        void Accept(ListNode node);
        void Accept(SelectNode node);
        void Accept(WhereNode node);
    }
}
