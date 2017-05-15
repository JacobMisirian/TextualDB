using System;

using TextualDB.Deserializer;

namespace TextualDB.Exceptions
{
    /// <summary>
    /// Exception to be thrown if there is a generic error with interpreting a command
    /// </summary>
    public class CommandLineInterpreterException : Exception
    {
        /// <summary>
        /// The string error message
        /// </summary>
        public new string Message { get; private set; }
        /// <summary>
        /// The location in the command string this error originates
        /// </summary>
        public SourceLocation SourceLocation { get; private set; }
        /// <summary>
        /// Constructs a new CommandLineInterpreterException with the given source location, message format string, and format arguments
        /// </summary>
        /// <param name="location">The source location of the error</param>
        /// <param name="msgf">The C# format message string to be passed into string.Format</param>
        /// <param name="args">The format arguments to be passed into string.Format</param>
        public CommandLineInterpreterException(SourceLocation location, string msgf, params object[] args)
        {
            Message = string.Format(msgf, args);
            SourceLocation = location;
        }
    }
}
