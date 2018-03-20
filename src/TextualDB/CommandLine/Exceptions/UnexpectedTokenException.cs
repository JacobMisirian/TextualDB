using TextualDB.CommandLine.Lexer;

namespace TextualDB.CommandLine.Exceptions
{
    public class UnexpectedTokenException : CommandLineException   
    {
        public override SourceLocation SourceLocation { get; }

        public new string Message {  get { return message; } }

        private const string MESSAGE_FORMAT= "Unexpected token of type \"{0}\" with value \"{1}\"!";

        private string message;

        public UnexpectedTokenException(Token token)
        {
            SourceLocation = token.SourceLocation;

            message = string.Format(MESSAGE_FORMAT, token.TokenType, token.Value);
        }
    }
}
