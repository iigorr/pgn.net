using System;

namespace ilf.pgn.Exceptions
{
    /// <summary>
    /// Indicates a problem with the pgn format.
    /// </summary>
    public class PgnFormatException : FormatException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgnFormatException"/> .
        /// </summary>
        public PgnFormatException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PgnFormatException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PgnFormatException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PgnFormatException"/>.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public PgnFormatException(string message, Exception innerException) : base(message, innerException) { }
    }
}
