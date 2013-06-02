[<AutoOpen>]
module internal ilf.pgn.PgnParsers.PgnTags

open System
open ilf.pgn.Data

type PgnTag(name: string) = 
    member val Name = name with get, set


type PgnBasicTag(name: string, value: string) =
    inherit PgnTag(name)
    member val Value: string = value with get, set


type PgnDateTag(name: string) = 
    inherit PgnTag(name)
    
    member val Year: Nullable<Int32> = Nullable() with get, set
    member val Month: Nullable<Int32> = Nullable() with get, set
    member val Day: Nullable<Int32> = Nullable() with get, set

type PgnRoundTag(name: string, round: string option) = 
    inherit PgnTag(name)
    
    member val Round: System.String = 
        match round with
        | None -> null
        | _ -> round.Value
        with get, set

type PgnResultTag(name: string, result: GameResult) = 
    inherit PgnTag(name)
    
    member val Result: GameResult = result with get, set

type FenTag(name: string, setup: BoardSetup) =
    inherit PgnTag(name)

    member val Setup: BoardSetup = setup with get, set