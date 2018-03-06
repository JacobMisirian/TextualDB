using TextualDB.Deserialization.Lexer;

namespace TextualDB.Deserialization.Exceptions
{
    public class UnexpectedTokenException : DeserializerException   
    {
        public override SourceLocation SourceLocation { get; }

        public new string Message {  get { return message; } }

        private const string MESSAGE_FORMAT_PRIMARY = "Unexpected token of type \"{0}\" with value \"{1}\"!";
        private const string MESSAGE_FORMAT_SECONDARY = "Unexpected token of type \"{0}\"!";

        private string message;

        public UnexpectedTokenException(SourceLocation location, TokenType tokenType)
        {
            SourceLocation = location;

            message = string.Format(MESSAGE_FORMAT_SECONDARY, tokenType);
        }
        public UnexpectedTokenException(SourceLocation location, TokenType tokenType, string value)
        {
            SourceLocation = location;

            message = string.Format(MESSAGE_FORMAT_PRIMARY, tokenType, value);
        }
    }
}
