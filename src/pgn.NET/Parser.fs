namespace ilf.pgn

open FParsec
open System.IO
open ilf.pgn.Exception
open ilf.pgn.PgnParsers.Game

type Parser() =
    member this.ReadFromFile(file:string) =  
        let stream = new FileStream(file, FileMode.Open)
        let result = this.ReadFromStream(stream)
        stream.Close()

        result

    member this.ReadFromStream(stream: System.IO.Stream) =
        let parserResult = runParserOnStream pDatabase () "pgn" stream System.Text.Encoding.UTF8
        
        let db =
            match parserResult with
            | Success(result, _, _)   -> result
            | Failure(errorMsg, _, _) -> raise (PgnFormatException errorMsg)

        db