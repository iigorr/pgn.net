module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move


let pSinglePeriod = pchar '.' >>. preturn true
let pEllipsis = (str "..." .>> manyChars (pchar '.')) <|> str "…" >>. preturn false

let pMoveNumberIndicator = pint32 .>> ws .>>. (pEllipsis <|> pSinglePeriod) <!> "pMoveNumberIndicator"
let pFullMoveTextEntry =
    opt pMoveNumberIndicator .>> ws .>>. pMove .>> ws1 .>>. pMove 
    |>> fun ((moveInd, moveWhite), moveBlack) -> 
            match moveInd with 
            | None -> MoveTextEntry(White= Some moveWhite, Black= Some moveBlack)
            | Some(num, _) -> MoveTextEntry(MoveNumber=Some num, White= Some moveWhite, Black= Some moveBlack)
    <!> "pFullMoveTextEntry"

let pSplitMoveTextEntry = 
    pMoveNumberIndicator .>> ws .>>. pMove
    |>> fun ((num, firstMove), move) -> 
            match firstMove with 
            | true -> MoveTextEntry(MoveNumber= Some num, White= Some move)
            | false -> MoveTextEntry(MoveNumber=Some num, Black= Some move)
    <!> "pSplitMoveTextEntry"

let pCommentary = 
    between (str "{") (str "}") (many (noneOf "}")) 
    <|> between (str "[") (str "]") (many (noneOf "]"))
    <|> between (str "(") (str ")") (many (noneOf ")"))
    <|> between (str ";") newline (many (noneOf "\n")) //to end of line comment
    |>>charList2String

let pOneHalf = str "1/2" <|> str "½"
let pDraw = pOneHalf .>> ws .>> str "-" .>> ws .>> pOneHalf |>> fun _ -> MoveTextEntryType.GameEndDraw
let pWhiteWin = str "1" .>> ws .>> str "-" .>> ws .>> str "0"  |>> fun _ -> MoveTextEntryType.GameEndWhite
let pBlackWin = str "0" .>> ws .>> str "-" .>> ws .>> str "1"  |>> fun _ -> MoveTextEntryType.GameEndBlack
let pEndOpen = str "*"  |>> fun _ -> MoveTextEntryType.GameEndOpen

let pEndOfGame =
    pDraw <|> pWhiteWin <|> pBlackWin <|> pEndOpen |>> fun endType -> MoveTextEntry(Type = endType )

let pMoveSeriesEntry= 
    attempt(pEndOfGame)
    <|> attempt(pFullMoveTextEntry) 
    <|> pSplitMoveTextEntry
    .>> ws .>>. opt pCommentary
    |>> fun (entry, comment) -> 
            match comment with
            | None -> entry
            | Some(c) -> entry.Comment <- Some c; entry
    <!> "pMoveSeriesEntry"

let pMoveSeries = sepEndBy pMoveSeriesEntry ws
let appyPCommentary (p: string)= run pCommentary p
