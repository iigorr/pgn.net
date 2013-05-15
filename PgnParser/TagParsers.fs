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


let tagValue = pchar '"' >>. (pNotChar '"') .>> pchar '"'
let tagContent= pTagName .>>. (spaces >>. tagValue);

let pTag = spaces .>> pchar '[' .>> spaces >>. tagContent .>> spaces .>> pchar ']' .>> spaces |>> fun (name, value) -> PgnTag(name, value)

let applyPTag p = run pTag p
