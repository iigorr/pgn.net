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


// Date Tag value (e.g. ????.??.??, 2013.05.18, 2013.??.??)
let pYear = ((str "????" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pMonth = ((str "??" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pDay = pMonth

let pDateTagValue = 
    pchar '"' >>. pYear .>> pchar '.' .>>. pMonth .>> pchar '.' .>>. pDay .>> pchar '"'
    |>> fun((year, month), day) -> PgnDateTag("Date", Year = year, Month = month, Day=day) :> PgnTag

let pRound = 
    pchar '"' >>. ((str "?" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x))) .>> pchar '"'
    |>> fun round -> PgnRoundTag("Round", round) :> PgnTag

let pResult = 
    pchar '"' >>. (
        ((str "1-0" <|> str "1 - 0") |>> fun _ -> GameResult.White)
    <|> ((str "0-1" <|> str "0 - 1") |>> fun _ -> GameResult.Black)
    <|> ((str "1/2-1/2" <|> str "1/2 - 1/2" <|> str "½-½" <|> str "½ - ½")  |>> fun _ -> GameResult.Draw)
    <|> ((str "*" <|> str "?") |>> fun _ -> GameResult.Open)
    ) .>> pchar '"'
    |>> fun result -> PgnResultTag("Result", result) :> PgnTag

 // Basic tag (e.g. [Site "Boston"]
let tagValue = pchar '"' >>. (pNotChar '"') .>> pchar '"'
let pBasicTag = 
    pTagName .>> spaces .>>. tagValue
    |>> fun (tagName, tagValue) -> PgnBasicTag(tagName, tagValue) :> PgnTag

let tagContent = 
    (str "Date" .>> spaces >>. pDateTagValue)
    <|> (str "Round" .>> spaces >>. pRound)
    <|> (str "Result" .>> spaces >>. pResult)
    <|> pBasicTag 

let pTag = 
    ws .>> pchar '[' .>> ws >>. tagContent .>> ws .>> pchar ']' .>> ws 
    <!> "pTag"

let applyPTag p = run pTag p
