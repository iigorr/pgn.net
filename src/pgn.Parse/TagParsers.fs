module internal ilf.pgn.PgnParsers.Tag

open System
open FParsec
open ilf.pgn.Data
open ilf.pgn.PgnParsers.Bootstrap
open ilf.pgn.PgnParsers.Basic


type PgnTag(name: string) = 
    member val Name = name with get, set


type PgnBasicTag(name: string, value: string) =
    inherit PgnTag(name)
    member val Value: string = value with get, set


type PgnDateTag(name: string) = 
    inherit PgnTag(name)
    
    member val Year: Nullable<Int32> = Nullable() with get, set
    member val Month: Nullable<Int32> = Nullable() with get, set
    member val Day: Nullable<Int32> = Nullable() with get, set

type PgnRoundTag(name: string, round: string option) = 
    inherit PgnTag(name)
    
    member val Round: System.String = 
        match round with
        | None -> null
        | _ -> round.Value
        with get, set

type PgnResultTag(name: string, result: GameResult) = 
    inherit PgnTag(name)
    
    member val Result: GameResult = result with get, set

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
    identifier (IdentifierOptions()) .>> ws1
    <!> "pTagName"


// Date Tag value (e.g. ????.??.??, 2013.05.18, 2013.??.??)
let pYear = ((str "????" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pMonth = ((str "??" |>> fun x -> None) <|> (pint32 |>> fun x -> Some(x)))
let pDay = pMonth

let pDateTagValue = 
    attempt(pchar '"' >>. pYear .>> pchar '.' .>>. pMonth .>> pchar '.' .>>. pDay .>> pchar '"')
    <|> ((pchar '"' >>. pYear .>> pchar '"') |>> fun year -> ((year, None), None))
    |>> fun((year, month), day) -> PgnDateTag("Date", Year =  toNullable(year), Month = toNullable(month), Day=toNullable(day)) :> PgnTag
    <!> "pDateTagValue"

let pRoundTagValue = 
    attempt(pchar '"' .>> pchar '?' .>> pchar '"' >>. preturn None)
    <|> attempt(pchar '"' .>> pchar '"' >>. preturn None)
    <|> (pchar '"' >>. many (noneOf "\"")  .>> pchar '"' |>> fun x -> Some(charList2String(x))) 
    |>> fun round -> PgnRoundTag("Round", round) :> PgnTag
    <!> "pRoundTagValue"

let pResultTagVaue = 
    pchar '"' >>. (
        ((str "1-0" <|> str "1 - 0") |>> fun _ -> GameResult.White)
    <|> ((str "0-1" <|> str "0 - 1") |>> fun _ -> GameResult.Black)
    <|> ((str "1/2-1/2" <|> str "1/2 - 1/2" <|> str "½-½" <|> str "½ - ½")  |>> fun _ -> GameResult.Draw)
    <|> ((str "*" <|> str "?") |>> fun _ -> GameResult.Open)
    ) .>> pchar '"'
    |>> fun result -> PgnResultTag("Result", result) :> PgnTag
    <!> "pResultTagVaue"

 // Basic tag (e.g. [Site "Boston"]
let pBasicTagValue = 
    between (pchar '"') (pchar '"') (pNotChar '"')
    <!> "pBasicTagValue"

let pBasicTag = 
    pTagName .>> spaces .>>. pBasicTagValue
    |>> fun (tagName, tagValue) -> PgnBasicTag(tagName, tagValue) :> PgnTag
    <!> "pBasicTag"

let tagContent = 
    (str "Date" .>> ws >>. pDateTagValue)
    <|> (str "Round" .>> ws >>. pRoundTagValue)
    <|> (str "Result" .>> ws >>. pResultTagVaue)
    <|> pBasicTag 
    <!> "pTagContent"

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
    <!!> ("pTagList", 2)

let applyPTag p = run pTag p
