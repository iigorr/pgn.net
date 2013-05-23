namespace ilf.pgn

open ilf.pgn

open System.Collections.Generic

type GameInfo(name, value) =
    member val Name:string = name with get, set
    member val Value:string = value with get, set

type Game() = 
    member val Event:string = "?" with get, set
    member val Site:string = "?" with get, set

    member val Year:int option = None with get, set
    member val Month:int option = None with get, set
    member val Day:int option = None with get, set

    member val Round:string option = None with get, set

    member val WhitePlayer:string = "?" with get, set
    member val BlackPlayer:string = "?" with get, set

    member val Result: GameResult = GameResult.Open with get, set

    member val AdditionalInfo: List<GameInfo> = new List<GameInfo>() with get

    member val MoveText: List<MoveTextEntry> = new List<MoveTextEntry>() with get
