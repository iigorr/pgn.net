module ilf.pgn.PgnParsers.MoveSeries

open FParsec
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Move

let pPeriods = (many1Chars (pchar '.')) <|> str "…"

let pMoveNumberIndicator = pint32 .>> ws .>> pPeriods <!> "pMoveNumberIndicator"
let pFinishedMovePair =
    opt pMoveNumberIndicator .>> ws1 .>> pMove .>> ws1 .>> pMove <!> "pFinishedMovePair"
let pUnfinishedMovePair = 
    opt pMoveNumberIndicator .>> ws1 .>> pMove <!> "pUnfinishedMovePair"

let pMovePair= attempt(pFinishedMovePair) <|> (pUnfinishedMovePair .>> ws .>> pUnfinishedMovePair) <!> "pMovePair"
let appyPMoveSeries (p: string)= run pMovePair p
