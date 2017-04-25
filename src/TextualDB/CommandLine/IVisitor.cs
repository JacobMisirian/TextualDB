using TextualDB.CommandLine.Ast;

namespace TextualDB.CommandLine
{
    public interface IVisitor
    {
        void Accept(CreateColumnNode node);
        void Accept(CreateTableNode node);
        void Accept(DeleteColumnNode node);
        void Accept(DeleteRowNode node);
        void Accept(DeleteTableNode node);
        void Accept(FilterNode node);
        void Accept(IdentifierNode node);
        void Accept(InsertNode node);
        void Accept(ListNode node);
        void Accept(NumberNode node);
        void Accept(RenameColumnNode node);
        void Accept(RenameTableNode node);
        void Accept(SelectNode node);
        void Accept(ShowNode node);
        void Accept(WhereNode node);
    }
}
