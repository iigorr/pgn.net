module ilf.pgn.Test.TestBase

open FParsec
open ilf.pgn.Data
open ilf.pgn.Exceptions
open ilf.pgn.PgnParsers

open Microsoft.VisualStudio.TestTools.UnitTesting

let parse p str =
    match run p str with
    | Success(result, _, _)   -> result
    | Failure(errorMsg, _, _) -> raise (PgnFormatException errorMsg)


let tryParse p str =
    match run p str with
    | Success(result, _, _)   -> ()
    | Failure(errorMsg, _, _) -> raise (PgnFormatException errorMsg)

let shouldFail p str = 
    match run p str with
    | Success(result, _, _)   -> raise (AssertFailedException "Expected parser did not fail")
    | Failure(errorMsg, _, _) -> ()