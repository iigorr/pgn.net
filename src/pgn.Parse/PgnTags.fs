[<AutoOpen>]
module internal ilf.pgn.PgnParsers.PgnTags

open System
open ilf.pgn.Data

type PgnTag(name: string, value: string) = 
    member val Name = name with get, set
    member val Value = value with get, set

let formatDate (year : int option, month : int option, day : int option) = 
    let yearStr = match year with 
        | None -> "????"
        | _ -> year.Value.ToString("D4")

    let monthStr = match month with
        | None -> "??"
        | _ -> month.Value.ToString("D2");

    let dayStr = match day with
        | None -> "??"
        | _ -> day.Value.ToString("D2");

    String.Format("{0}-{1}-{2}", yearStr, monthStr, dayStr)

type PgnDateTag(name: string, year: int option, month: int option, day: int option) = 
    inherit PgnTag(name, formatDate(year, month, day))
    
    member val Year: Nullable<Int32> = toNullable(year) with get, set
    member val Month: Nullable<Int32> = toNullable(month) with get, set
    member val Day: Nullable<Int32> = toNullable(day) with get, set


let formatResult(result: GameResult) = 
    match result with
    | GameResult.White -> "1 - 0"
    | GameResult.Black -> "0 - 1"
    | GameResult.Draw  -> "1/2 - 1/2"
    | GameResult.Open  -> "*"

type PgnResultTag(name: string, result: GameResult) = 
    inherit PgnTag(name, formatResult(result))
    
    member val Result: GameResult = result with get, set

type FenTag(name: string, setup: BoardSetup) =
    inherit PgnTag(name, setup.ToString())

    member val Setup: BoardSetup = setup with get, set