module ilf.pgn.PgnParsers.Tag

open FParsec
open ilf.pgn
open ilf.pgn.PgnParsers.Basic

let sevenTagRosterTagNames= ["Event"; "Site"; "Date"; "Round"; "White"; "Black"; "Result"];
let suplementTagNames = 
            ["WhiteTitle"; "BlackTitle"; "WhiteElo"; "BlackElo"; "WhiteUSCF"; "BlackUSCF"; "WhiteNA"; "BlackNA"; "WhiteType"; "BlackType"; 
            "EventDate"; "EventSponsor"; "Section"; "Stage"; "Board";
            "Opening"; "Variation"; "SubVariation";
            "TimeControl";
            "ECO"; "NIC"; "Time"; "UTCTime"; "UTCDate";
            "SetUp"; "FEN";
            "Termination";
            "Annotator"; "Mode"; "PlyCount";
            "WhiteClock"; "BlackClock"]

let pTagName =
    attempt(suplementTagNames @ sevenTagRosterTagNames |> Seq.map pstring |> choice .>> ws1)
    <|> (identifier (IdentifierOptions()) .>> ws1)
    <!> "pTagName"


// Date Tag value (e.g. ????.??.??, 2013.05.18, 2013.??.??)
let pYear = ((str "????" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pMonth = ((str "??" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pDay = pMonth

let pDateTagValue = 
    attempt(pchar '"' >>. pYear .>> pchar '.' .>>. pMonth .>> pchar '.' .>>. pDay .>> pchar '"')
    <|> ((pchar '"' >>. pYear .>> pchar '"') |>> fun year -> ((year, None), None))
    |>> fun((year, month), day) -> PgnDateTag("Date", Year = year, Month = month, Day=day) :> PgnTag

let pRound = 
    attempt(pchar '"' >>. ((str "?" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x))) .>> pchar '"')
    <|> (pchar '"' .>> pchar '"' >>. preturn None)
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
    <!!> ("pTag", 1)
    <?> "Tag (e.g [Date \"2013.10.02\"])"

let checkSTRTags (tagList: PgnTag list) : Parser<_, _> =
    fun stream ->
        match tagList with 
        | l when l.Length = 0 -> Reply(Error, NoErrorMessages) //no tags
        | _  -> Reply(tagList)

let pTagList = 
    ws >>. sepEndBy pTag ws
    >>=  checkSTRTags
    <!> "pTagList"

let applyPTag p = run pTag p
