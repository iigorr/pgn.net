module ilf.pgn.PgnParsers.Basic

open FParsec

let ws = spaces
let str = pstring
let pNotChar c = manySatisfy (fun x -> x <> c)
