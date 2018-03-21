namespace TextualDB.CommandLine.Exceptions
{
    public class UnknownEscapeCodeException : CommandLineException
    {
        public override SourceLocation SourceLocation { get; }

        public override string Message { get { return message; } }

        private const string MESSAGE_FORMAT = "Unknown escape code \"{0}\"!";
        private string message;

        public UnknownEscapeCodeException(SourceLocation location, string code)
        {
            SourceLocation = location;

            message = string.Format(MESSAGE_FORMAT, code);
        }
    }
}
