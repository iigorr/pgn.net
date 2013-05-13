namespace ilf.pgn.Test

open ilf.pgn
open ilf.pgn.PgnParsers.Tag
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type ParserTests() = 
    [<TestMethod; ExpectedException(typeof<ParseException>)>]
    member this.pTag_should_fail_if_expressions_starts_with_non_bracket() =
        tryParse pTag "test"

    [<TestMethod>]
    member this.pTag_should_accept_random_spaces_before_and_after_brackets() =
        tryParse pTag "          [Date \"Foo\"]"
        tryParse pTag "  \t \n  [Date \"Foo\"]"
        tryParse pTag "  \t \n  [Date \"Foo\"]"
        tryParse pTag "[  \t \nDate \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Date \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<TestMethod>]
    member this.pTag_should_accept_random_spaces_between_tag_name_and_value() =
        tryParse pTag "[Date\t \n   \t \"Foo\"]"
        tryParse pTag "[Date \"Foo\"]"
        tryParse pTag "[Date \t \n   \t\"Foo\"]"
        tryParse pTag "[  \t \nDate         \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Date \t \n   \t \n  \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<TestMethod>]
    ///<see href="see http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm#c8.1.1" />
    member this.pTag_should_allow_tag_names_from_SevenTagRoaster() =
        let allowedTagNames = ["Event"; "Site"; "Date"; "Round"; "White"; "Black"; "Result"]
        let parseTag x = tryParse pTag ("["+ x + " \"Foo\"]")
        List.map parseTag allowedTagNames |> ignore
        ()

    [<TestMethod>]
    ///<see href="http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm#c9" />
    member this.pTag_should_allow_suplemental_tag_names() =
        let allowedTagNames = 
            ["WhiteTitle"; "BlackTitle"; "WhiteElo"; "BlackElo"; "WhiteUSCF"; "BlackUSCF"; "WhiteNA"; "BlackNA"; "WhiteType"; "BlackType"; 
            "EventDate"; "EventSponsor"; "Section"; "Stage"; "Board";
            "Opening"; "Variation"; "SubVariation";
            "ECO"; "NIC"; "Time"; "UTCTime"; "UTCDate";
            "TimeControl";
            "SetUp"; "FEN";
            "Termination";
            "Annotator"; "Mode"; "PlyCount"]
        let parseTag x = tryParse pTag ("["+ x + " \"Foo\"]")
        List.map parseTag allowedTagNames |> ignore
        ()
