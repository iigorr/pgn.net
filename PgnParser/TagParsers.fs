module ilf.pgn.PgnParsers.Tag

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic

let sevenTagRoasterTagNames= ["Event"; "Site"; "Date"; "Round"; "White"; "Black"; "Result"];
let suplementTagNames = 
            ["WhiteTitle"; "BlackTitle"; "WhiteElo"; "BlackElo"; "WhiteUSCF"; "BlackUSCF"; "WhiteNA"; "BlackNA"; "WhiteType"; "BlackType"; 
            "EventDate"; "EventSponsor"; "Section"; "Stage"; "Board";
            "Opening"; "Variation"; "SubVariation";
            "TimeControl";
            "ECO"; "NIC"; "Time"; "UTCTime"; "UTCDate";
            "SetUp"; "FEN";
            "Termination";
            "Annotator"; "Mode"; "PlyCount"]

let pTagName =
    suplementTagNames @ sevenTagRoasterTagNames
    |> Seq.map pstring 
    |> choice
    <!> "pTagName"


let tagValue = pchar '"' >>. (pNotChar '"') .>> pchar '"'
let tagContent= pTagName .>>. (spaces >>. tagValue);

let pTag = 
    ws .>> pchar '[' .>> ws >>. tagContent .>> ws .>> pchar ']' .>> ws |>> fun (name, value) -> PgnTag(name, value)
    <!> "pTag"

let applyPTag p = run pTag p
