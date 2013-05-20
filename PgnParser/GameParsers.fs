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
    | "Round" -> game.Round <- (tag :?> PgnRoundTag).Round
    | "White" -> game.WhitePlayer <- (tag :?> PgnBasicTag).Value
    | "Black" -> game.BlackPlayer <- (tag :?> PgnBasicTag).Value
    | "Result" -> game.Result <- (tag :?> PgnResultTag).Result
    | _ -> 
        let basicTag = (tag :?> PgnBasicTag)
        game.AdditionalInfo.Add(GameInfo(basicTag.Name, basicTag.Value))

let checkErrors(tagList: PgnTag list, moveTextList : MoveTextEntry list) : Parser<_, _> =
    let existingTags = tagList |> List.collect (fun tag -> [tag.Name]) //select tag names
    let missingTags = (set sevenTagRoasterTagNames) - set existingTags |> Set.toList

    fun stream ->
        match missingTags with 
        | [] -> Reply((tagList, moveTextList))
        | _  -> Reply(Error, messageError( sprintf "The following tags of the Seven Tag Roaster are missing: '%s'" (missingTags |> String.concat "'")))


let makeGame (tagList : PgnTag list, moveTextList : MoveTextEntry list) =
    let game = new Game()
    tagList |> List.map(fun tag -> setTag(game, tag)) |> ignore

    moveTextList |>  List.iter (fun entry -> game.MoveText.Add(entry))
    game

let pGame = 
    pTagList .>> ws .>>.  pMoveSeries >>= checkErrors |>>  makeGame
    <!> "pGame"

let pEmptyDatabase = 
    ws >>. eof |>> fun _ -> []
    <!> "pEmptyDatabase"

let pDatabase = 
    attempt(pEmptyDatabase)
    <|> (ws >>. sepEndBy pGame (ws <|> eof))