module ilf.pgn.PgnParsers.Game

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Tag
open ilf.pgn.PgnParsers.MoveSeries

let pTagList = 
    ws >>. sepEndBy pTag ws
    <!> "pTagList"

let setTag(game : Game, tag : PgnTag) =
    match tag.Name with
    | "Event" -> game.Event <- (tag :?> PgnBasicTag).Value
    | "Site" -> game.Site <- (tag :?> PgnBasicTag).Value
    | "Date" -> game.Year <- (tag :?> PgnDateTag).Year; game.Month <- (tag :?> PgnDateTag).Month; game.Day <- (tag :?> PgnDateTag).Day
    | _ -> ()


let makeGame (tagList : PgnTag list, moveEntryList : MoveEntry list) =
    if (tagList.Length < 7) then
        raise (PgnFormatException("The tags of the Seven Tag Roaster (STR) must be defined at least."+
                "The following tags must appear: 'Event'; 'Site'; 'Date'; 'Round'; 'White'; 'Black'; 'Result'"))
    let game = new Game()

    tagList |> List.map(fun tag -> setTag(game, tag)) |> ignore

    game

let pGame = 
    pTagList .>> ws .>>.  pMoveSeries
    |>> makeGame
    <!> "pGame"

let pGameRaw = 
    pTagList .>> ws .>>.  pMoveSeries   