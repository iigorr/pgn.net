module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move

let pPeriods = (many1Chars (pchar '.')) <|> str "…"

let pMoveNumberIndicator = (pint32 |>> fun c -> c.ToString()) .>> ws .>> pPeriods <!> "pMoveNumberIndicator"
let pFullMoveEntry =
    opt pMoveNumberIndicator .>> ws .>> pMove .>> ws1 .>> pMove <!> "pFullMoveEntry"
let pSplitMoveEntry = 
    opt pMoveNumberIndicator .>> ws .>> pMove <!> "pSplitMoveEntry"

let pCommentary = pchar '{' >>. sepEndBy (noneOf "{") (pchar '}') |>> charList2String
let pMoveSeriesEntry= attempt(pFullMoveEntry) <|> pSplitMoveEntry  <!> "pMoveSeriesEntry"

let appyPMoveSeries (p: string)= run pMoveSeriesEntry p
let appyPCommentary (p: string)= run pCommentary p
