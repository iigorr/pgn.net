[<AutoOpen>]
module internal ilf.pgn.PgnParsers.Game

open FParsec
open ilf.pgn.Data

let setTag(game : Game, tag : PgnTag) =
    game.Tags.Add(tag.Name, tag.Value)
    match tag.Name with
    | "Event" -> game.Event <- tag.Value
    | "Site" -> game.Site <- tag.Value
    | "Date" -> game.Year <- (tag :?> PgnDateTag).Year; game.Month <- (tag :?> PgnDateTag).Month; game.Day <- (tag :?> PgnDateTag).Day
    | "Round" -> game.Round <- tag.Value
    | "White" -> game.WhitePlayer <- tag.Value
    | "Black" -> game.BlackPlayer <- tag.Value
    | "Result" -> game.Result <- (tag :?> PgnResultTag).Result
    | "FEN" -> game.BoardSetup <- (tag :?> FenTag).Setup
    | _ -> 
        game.AdditionalInfo.Add(GameInfo(tag.Name, tag.Value))
        
        


let makeGame (tagList : PgnTag list, moveTextList : MoveTextEntry list) =
    let game = new Game()
    tagList |> List.map(fun tag -> setTag(game, tag)) |> ignore

    moveTextList |>  List.iter (fun entry -> game.MoveText.Add(entry))
    game

let pGame = 
    ws >>. pTagList .>> ws .>>.  pMoveSeries .>> (ws <|> eof)
    |>>  makeGame
    <!!> ("pGame", 5)


let pDatabase = 
    sepEndBy pGame ws .>> eof
    |>> fun games -> 
            let db = new Database()
            db.Games.AddRange(games)
            db

