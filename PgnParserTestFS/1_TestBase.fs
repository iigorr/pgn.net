module ilf.pgn.Test.TestBase

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers

let parse p str =
    match run p str with
    | Success(result, _, _)   -> result
    | Failure(errorMsg, _, _) -> raise (ParseException errorMsg)

let tryParse p str =
    match run p str with
    | Success(result, _, _)   -> ()
    | Failure(errorMsg, _, _) -> raise (ParseException errorMsg)