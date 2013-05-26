namespace ilf.pgn.Exception

type PgnFormatException =
    inherit System.FormatException
    new() = { inherit System.FormatException() }
    new(message: string) = { inherit System.FormatException(message) }
    new(message: string, innerException: exn) = { inherit System.FormatException(message, innerException) }
