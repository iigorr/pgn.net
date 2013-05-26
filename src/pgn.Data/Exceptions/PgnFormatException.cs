using System;

namespace ilf.pgn.Exceptions
{
    public class PgnFormatException : FormatException
    {
        public PgnFormatException() { }
        public PgnFormatException(string message) : base(message) { }
        public PgnFormatException(string message, Exception innerException) : base(message, innerException) { }
    }
}
