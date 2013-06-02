[<AutoOpen>]
module internal ilf.pgn.PgnParsers.DateTagParser

open FParsec

// Date Tag value (e.g. ????.??.??, 2013.05.18, 2013.??.??)
let pYear = ((str "????" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pMonth = ((str "??" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pDay = pMonth

let pDateTagValue = 
    attempt(pchar '"' >>. pYear .>> pchar '.' .>>. pMonth .>> pchar '.' .>>. pDay .>> pchar '"')
    <|> ((pchar '"' >>. pYear .>> pchar '"') |>> fun year -> ((year, None), None))
    |>> fun((year, month), day) -> PgnDateTag("Date", Year =  toNullable(year), Month = toNullable(month), Day=toNullable(day)) :> PgnTag
    <!> "pDateTagValue"

let applypDateTagValue p = run pDateTagValue p