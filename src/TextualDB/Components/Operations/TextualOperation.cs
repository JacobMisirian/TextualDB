using TextualDB.CommandLine.Lexer;
using TextualDB.CommandLine.Parser;

namespace TextualDB.Components.Operations
{
    public abstract class TextualOperation
    {
        public abstract TextualTable Result { get; }

        public static TextualTable ExecuteOperation(TextualDatabase database, string op)
        {
            var tokens = new Scanner(op).Scan();
            return new OperationParser(database, tokens).Parse();
        }
    }
}
