namespace ilf.pgn

open System

type ParseException = 
    inherit Exception
    
    new() = { inherit Exception() }
    new(message: string) = { inherit Exception(message) }
    new(message: string, innerException: Exception) = { inherit Exception(message, innerException) }
