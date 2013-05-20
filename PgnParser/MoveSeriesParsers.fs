module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move


let pPeriods =  many1Chars (pchar '.') <|> str "…" 

let pMoveNumberIndicator = pint32 .>> ws .>> pPeriods <!> "pMoveNumberIndicator"
let pFullMoveTextEntry =
    opt pMoveNumberIndicator .>> ws >>. pMove .>> ws1 .>>. pMove 
    |>> fun (moveWhite, moveBlack) ->  MovePairEntry(moveWhite, moveBlack) :> MoveTextEntry
    <!> "pFullMoveTextEntry"

let pSplitMoveTextEntry = 
    opt(pMoveNumberIndicator .>> ws) >>. pMove
    |>> fun move -> SingleMoveEntry(move) :> MoveTextEntry
    <!> "pSplitMoveTextEntry"

let pCommentary = 
    between (str "{") (str "}") (many (noneOf "}")) 
    <|> between (str ";") newline (many (noneOf "\n")) //to end of line comment
    |>> charList2String
    |>> fun text -> CommentEntry(text) :> MoveTextEntry

let pOneHalf = str "1/2" <|> str "½"
let pDraw = pOneHalf .>> ws .>> str "-" .>> ws .>> pOneHalf |>> fun _ -> GameResult.Draw
let pWhiteWin = str "1" .>> ws .>> str "-" .>> ws .>> str "0"  |>> fun _ -> GameResult.White
let pBlackWin = str "0" .>> ws .>> str "-" .>> ws .>> str "1"  |>> fun _ -> GameResult.Black
let pEndOpen = str "*"  |>> fun _ -> GameResult.Open

let pEndOfGame =
    pDraw <|> pWhiteWin <|> pBlackWin <|> pEndOpen |>> fun endType -> GameEndEntry(endType) :> MoveTextEntry
    <!> "pEndOfGame"

let pMoveSeriesEntry= 
    attempt(pEndOfGame)
    <|> attempt(pFullMoveTextEntry) 
    <|> attempt(pSplitMoveTextEntry)
    <|> pCommentary
    <!> "pMoveSeriesEntry"

let pMoveSeries = 
    sepEndBy1 pMoveSeriesEntry ws
    <!> "pMoveSeries"
