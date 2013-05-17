module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move


let pSinglePeriod = pchar '.' >>. preturn true
let pEllipsis = (str "..." .>> manyChars (pchar '.')) <|> str "…" >>. preturn false

let pMoveNumberIndicator = pint32 .>> ws .>>. (pEllipsis <|> pSinglePeriod) <!> "pMoveNumberIndicator"
let pFullMoveEntry =
    opt pMoveNumberIndicator .>> ws .>>. pMove .>> ws1 .>>. pMove 
    |>> fun ((moveInd, moveWhite), moveBlack) -> 
            match moveInd with 
            | None -> MoveEntry(White= Some moveWhite, Black= Some moveBlack)
            | Some(num, _) -> MoveEntry(MoveNumber=Some num, White= Some moveWhite, Black= Some moveBlack)
    <!> "pFullMoveEntry"

let pSplitMoveEntry = 
    pMoveNumberIndicator .>> ws .>>. pMove
    |>> fun ((num, firstMove), move) -> 
            match firstMove with 
            | true -> MoveEntry(MoveNumber= Some num, White= Some move)
            | false -> MoveEntry(MoveNumber=Some num, Black= Some move)
    <!> "pSplitMoveEntry"

let pCommentary = 
    between (str "{") (str "}") (many (noneOf "}")) 
    <|> between (str "[") (str "]") (many (noneOf "]"))
    <|> between (str "(") (str ")") (many (noneOf ")"))
    <|> between (str ";") newline (many (noneOf "\n")) //to end of line comment
    |>>charList2String

let pOneHalf = str "1/2" <|> str "½"
let pDraw = pOneHalf .>> ws .>> str "-" .>> ws .>> pOneHalf |>> fun _ -> MoveEntryType.GameEndDraw
let pWhiteWin = str "1" .>> ws .>> str "-" .>> ws .>> str "0"  |>> fun _ -> MoveEntryType.GameEndWhite
let pBlackWin = str "0" .>> ws .>> str "-" .>> ws .>> str "1"  |>> fun _ -> MoveEntryType.GameEndBlack
let pEndOpen = str "*"  |>> fun _ -> MoveEntryType.GameEndOpen

let pEndOfGame =
    pDraw <|> pWhiteWin <|> pBlackWin <|> pEndOpen |>> fun endType -> MoveEntry(Type = endType )

let pMoveSeriesEntry= 
    attempt(pEndOfGame)
    <|> attempt(pFullMoveEntry) 
    <|> pSplitMoveEntry
    .>> ws .>>. opt pCommentary
    |>> fun (entry, comment) -> 
            match comment with
            | None -> entry
            | Some(c) -> entry.Comment <- Some c; entry
    <!> "pMoveSeriesEntry"

let pMoveSeries = sepEndBy pMoveSeriesEntry ws
let appyPCommentary (p: string)= run pCommentary p
