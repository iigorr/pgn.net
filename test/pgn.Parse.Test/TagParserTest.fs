namespace ilf.pgn.Test

open ilf.pgn.Data
open ilf.pgn.Exceptions
open ilf.pgn.PgnParsers.PgnTags
open ilf.pgn.PgnParsers.Tag
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TagParserTests() = 
    [<TestMethod; ExpectedException(typeof<PgnFormatException>)>]
    member this.pTag_should_fail_if_expressions_starts_with_non_bracket() =
        tryParse pTag "test"

    [<TestMethod>]
    member this.pTag_should_accept_random_spaces_before_and_after_brackets() =
        tryParse pTag "          [Event \"Foo\"]"
        tryParse pTag "  \t \n  [Event \"Foo\"]"
        tryParse pTag "  \t \n  [Event \"Foo\"]"
        tryParse pTag "[  \t \tEvent \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Event \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<TestMethod>]
    member this.pTag_should_accept_random_spaces_between_tag_name_and_value() =
        tryParse pTag "[Event\t \n   \t \"Foo\"]"
        tryParse pTag "[Event \"Foo\"]"
        tryParse pTag "[Event \t \n   \t\"Foo\"]"
        tryParse pTag "[  \t \tEvent         \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Event \t \n   \t \n  \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<TestMethod>]
    ///<see href="see http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm#c8.1.1" />
    member this.pTag_should_allow_tag_names_from_SevenTagRoster() =
        tryParse pTag "[Date \"2013.05.18\"]"
        tryParse pTag "[Round \"5\"]"
        tryParse pTag "[Result \"*\"]"

        let basicTagNames = ["Event"; "Site"; "White"; "Black";]
        let parseTag x = tryParse pTag ("["+ x + " \"Foo\"]")
        List.map parseTag basicTagNames |> ignore
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
            "SetUp";
            "Termination";
            "Annotator"; "Mode"; "PlyCount"]
        let parseTag x = tryParse pTag ("["+ x + " \"Foo\"]")
        List.map parseTag allowedTagNames |> ignore
        ()

    [<TestMethod>]
    member this.pTag_should_accept_tags_which_are_prefixes_of_others() =
        tryParse pTag "[WhiteSomethingFoo \"\"]"


    [<TestMethod>]
    member this.pTag_should_create_a_PgnDateTag_object_from_a_valid_tag() =
        let tag= parse pTag "[Date \"2013.05.15\"]"
        Assert.IsInstanceOfType(tag, typeof<PgnDateTag>)
        Assert.AreEqual("Date", tag.Name)
        Assert.AreEqual(2013, (tag :?> PgnDateTag).Year)
        Assert.AreEqual(5, (tag :?> PgnDateTag).Month)
        Assert.AreEqual(15, (tag :?> PgnDateTag).Day)
    
    [<TestMethod>]
    member this.pTag_should_accept_only_the_year_as_date() =
        let tag= parse pTag "[Date \"2013\"]"
        Assert.IsInstanceOfType(tag, typeof<PgnDateTag>)
        Assert.AreEqual("Date", tag.Name)
        Assert.AreEqual(2013, (tag :?> PgnDateTag).Year)
        Assert.AreEqual(null, (tag :?> PgnDateTag).Month)
        Assert.AreEqual(null, (tag :?> PgnDateTag).Day)

    [<TestMethod>]
    member this.pTag_should_create_a_PgnRoundTag_object_from_a_valid_tag() =
        let tag= parse pTag "[Round \"13\"]"
        Assert.IsInstanceOfType(tag, typeof<PgnRoundTag>)
        Assert.AreEqual("Round", tag.Name)
        Assert.AreEqual("13", (tag :?> PgnRoundTag).Round)

    [<TestMethod>]
    member this.pTag_should_create_PgnRoundTag_object_from_two_tags_in_sequence() =
        let tag = parse pTag @"[Round ""?""][White ""Tarrasch, Siegbert""]"

        Assert.IsInstanceOfType(tag, typeof<PgnRoundTag>)
        Assert.AreEqual("Round", tag.Name)
        Assert.AreEqual(None, (tag :?> PgnRoundTag).Round)

    [<TestMethod>]
    member this.pTag_should_accept_non_numeric_rounds() =
        let tag= parse pTag "[Round \"4.1\"]"
        Assert.IsInstanceOfType(tag, typeof<PgnRoundTag>)
        Assert.AreEqual("Round", tag.Name)
        Assert.AreEqual("4.1", (tag :?> PgnRoundTag).Round)

    [<TestMethod>]
    member this.pTag_should_create_a_PgnResultTag_object_from_a_valid_tag() =
        let tag= parse pTag "[Result \"1-0\"]"
        Assert.IsInstanceOfType(tag, typeof<PgnResultTag>)
        Assert.AreEqual("Result", tag.Name)
        Assert.AreEqual(GameResult.White, (tag :?> PgnResultTag).Result)

    [<TestMethod>]
    member this.pTag_should_create_a_FenTag_object_from_a_valid_tag() =
        let tag= parse pTag "[FEN \"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\"]"
        Assert.IsInstanceOfType(tag, typeof<FenTag>)

        let setup= (tag :?> FenTag).Setup

        Assert.AreEqual(Piece.BlackRook, setup.[File.A, 1])
        Assert.AreEqual(Piece.BlackPawn, setup.[File.B, 8])
        Assert.AreEqual(null, setup.[File.C, 1])
        Assert.AreEqual(Piece.WhiteKing, setup.[File.H, 5])

        Assert.AreEqual(true, setup.IsWhiteMove)
        
        Assert.AreEqual(true, setup.CanWhiteCastleKingSide)
        Assert.AreEqual(true, setup.CanWhiteCastleQueenSide)
        Assert.AreEqual(true, setup.CanBlackCastleKingSide)
        Assert.AreEqual(true, setup.CanBlackCastleQueenSide)

        Assert.AreEqual(null, setup.EnPassantSquare)

        Assert.AreEqual(0, setup.HalfMoveClock)
        Assert.AreEqual(1, setup.FullMoveCount)


    [<TestMethod>]
    member this.pTag_should_create_a_FenTag_object_from_another_valid_tag() =
        let tag= parse pTag "[FEN \"rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR b Kq c6 1 2\"]"
        Assert.IsInstanceOfType(tag, typeof<FenTag>)

        let setup= (tag :?> FenTag).Setup

        Assert.AreEqual(Piece.BlackRook, setup.[File.A, 1])
        Assert.AreEqual(Piece.BlackPawn, setup.[File.B, 8])
        Assert.AreEqual(Piece.BlackPawn, setup.[File.D, 3])
        Assert.AreEqual(Piece.WhiteKing, setup.[File.H, 5])

        Assert.AreEqual(false, setup.IsWhiteMove)
        
        Assert.AreEqual(true, setup.CanWhiteCastleKingSide)
        Assert.AreEqual(false, setup.CanWhiteCastleQueenSide)
        Assert.AreEqual(false, setup.CanBlackCastleKingSide)
        Assert.AreEqual(true, setup.CanBlackCastleQueenSide)

        Assert.AreEqual(Square(File.C, 6), setup.EnPassantSquare)

        Assert.AreEqual(1, setup.HalfMoveClock)
        Assert.AreEqual(2, setup.FullMoveCount)