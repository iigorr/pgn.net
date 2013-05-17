module ilf.pgn.PgnParsers.Game

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic
open ilf.pgn.PgnParsers.Tag
open ilf.pgn.PgnParsers.MoveSeries

let pTagList = 
    ws >>. sepEndBy pTag ws
    <!> "pTagList"

let pGame = 
    pTagList .>> ws .>>.  pMoveSeries
    <!> "pGame"
    