[<AutoOpen>]
module internal ilf.pgn.PgnParsers.Game

open FParsec
open ilf.pgn.Data

let setTag(game : Game, tag : PgnTag) =
    match tag.Name with
    | "Event" -> game.Event <- (tag :?> PgnBasicTag).Value
    | "Site" -> game.Site <- (tag :?> PgnBasicTag).Value
    | "Date" -> game.Year <- (tag :?> PgnDateTag).Year; game.Month <- (tag :?> PgnDateTag).Month; game.Day <- (tag :?> PgnDateTag).Day
    | "Round" -> game.Round <- (tag :?> PgnRoundTag).Round
    | "White" -> game.WhitePlayer <- (tag :?> PgnBasicTag).Value
    | "Black" -> game.BlackPlayer <- (tag :?> PgnBasicTag).Value
    | "Result" -> game.Result <- (tag :?> PgnResultTag).Result
    | "FEN" -> game.BoardSetup <- (tag :?> FenTag).Setup
    | _ -> 
        let basicTag = (tag :?> PgnBasicTag)
        game.AdditionalInfo.Add(GameInfo(basicTag.Name, basicTag.Value))


let makeGame (tagList : PgnTag list, moveTextList : MoveTextEntry list) =
    let game = new Game()
    tagList |> List.map(fun tag -> setTag(game, tag)) |> ignore

    moveTextList |>  List.iter (fun entry -> game.MoveText.Add(entry))
    game

let pGame = 
    pTagList .>> ws .>>.  pMoveSeries 
    |>>  makeGame
    <!!> ("pGame", 5)

let pDatabase = 
    ws >>. sepEndBy pGame ws .>> eof
    |>> fun games -> 
            let db = new Database()
            db.Games.AddRange(games)
            db
