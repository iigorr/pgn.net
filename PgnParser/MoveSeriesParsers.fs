module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move


let pPeriods =  many1Chars (pchar '.') <|> str "…" 

let pMoveNumberIndicator = 
    pint32 .>> ws .>> pPeriods <!> "pMoveNumberIndicator"
    <?> "Move number indicator (e.g. 5. or 13...)"
let pFullMoveTextEntry =
    opt pMoveNumberIndicator .>> ws >>. pMove .>> ws1 .>>. pMove 
    |>> fun (moveWhite, moveBlack) ->  MovePairEntry(moveWhite, moveBlack) :> MoveTextEntry
    <!!> ("pFullMoveTextEntry", 3)

let pSplitMoveTextEntry = 
    opt(pMoveNumberIndicator .>> ws) >>. pMove
    |>> fun move -> SingleMoveEntry(move) :> MoveTextEntry
    <!!> ("pSplitMoveTextEntry", 3)

let pCommentary = 
    between (str "{") (str "}") (many (noneOf "}")) 
    <|> between (str ";") newline (many (noneOf "\n")) //to end of line comment
    |>> charList2String
    |>> fun text -> CommentEntry(text) :> MoveTextEntry
    <!!> ("pCommentary", 3)
    <?> "Comment ( {...} or ;... )"

let pOneHalf = str "1/2" <|> str "½"
let pDraw = pOneHalf .>> ws .>> str "-" .>> ws .>> pOneHalf |>> fun _ -> GameResult.Draw
let pWhiteWin = str "1" .>> ws .>> str "-" .>> ws .>> str "0"  |>> fun _ -> GameResult.White
let pBlackWin = str "0" .>> ws .>> str "-" .>> ws .>> str "1"  |>> fun _ -> GameResult.Black
let pEndOpen = str "*"  |>> fun _ -> GameResult.Open

let pEndOfGame =
    pDraw <|> pWhiteWin <|> pBlackWin <|> pEndOpen |>> fun endType -> GameEndEntry(endType) :> MoveTextEntry
    <!!> ("pEndOfGame", 3)
    <?> "Game termination marker (1/2-1/2 or 1-0 or 0-1 or *)"

let pNAG =
    pchar '$' >>. pint32 |>> fun code -> NAGEntry(code) :> MoveTextEntry
    <?> "NAG ($<num> e.g. $6 or $32)"
    <!!> ("pNAG", 3)

let pMoveSeries, pMoveSeriesImpl = createParserForwardedToRef()

let pRAV =
    pchar '(' .>> ws >>. pMoveSeries .>> ws .>> pchar ')' 
    |>> fun moveSeries -> RAVEntry(moveSeries) :> MoveTextEntry
    <?> "RAV e.g. \"(6. Bd3)\""
    <!!> ("pRAV", 4)

let pMoveSeriesEntry= 
     pCommentary
    <|> pNAG
    <|> pRAV
    <|> attempt(pFullMoveTextEntry) 
    <|> attempt(pSplitMoveTextEntry)
    <|> attempt(pEndOfGame)
    <!!> ("pMoveSeriesEntry", 4)

do pMoveSeriesImpl := (
    sepEndBy1 pMoveSeriesEntry ws 
    <!!> ("pMoveSeries", 5)
    )
