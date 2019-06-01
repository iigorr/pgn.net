namespace ilf.pgn.Test

open ilf.pgn.Data
open ilf.pgn.PgnParsers.PgnTags
open ilf.pgn.PgnParsers.Tag
open ilf.pgn.Test.TestBase

open Xunit

type TagParserTests() =
    // TODO: Expect exception, so wait with converting this
    // [<Fact>]
    // member this.pTag_should_fail_if_expressions_starts_with_non_bracket() =
    //     tryParse pTag "test"

    [<Fact>]
    member this.pTag_should_accept_random_spaces_before_and_after_brackets() =
        tryParse pTag "          [Event \"Foo\"]"
        tryParse pTag "  \t \n  [Event \"Foo\"]"
        tryParse pTag "  \t \n  [Event \"Foo\"]"
        tryParse pTag "[  \t \tEvent \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Event \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<Fact>]
    member this.pTag_should_accept_random_spaces_between_tag_name_and_value() =
        tryParse pTag "[Event\t \n   \t \"Foo\"]"
        tryParse pTag "[Event \"Foo\"]"
        tryParse pTag "[Event \t \n   \t\"Foo\"]"
        tryParse pTag "[  \t \tEvent         \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Event \t \n   \t \n  \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<Fact>]
    ///<see href="see http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm#c8.1.1" />
    member this.pTag_should_allow_tag_names_from_SevenTagRoster() =
        tryParse pTag "[Date \"2013.05.18\"]"
        tryParse pTag "[Round \"5\"]"
        tryParse pTag "[Result \"*\"]"

        let basicTagNames = ["Event"; "Site"; "White"; "Black";]
        let parseTag x = tryParse pTag ("["+ x + " \"Foo\"]")
        List.map parseTag basicTagNames |> ignore
        ()

    [<Fact>]
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

    [<Fact>]
    member this.pTag_should_accept_tags_which_are_prefixes_of_others() =
        tryParse pTag "[WhiteSomethingFoo \"\"]"


    [<Fact>]
    member this.pTag_should_create_a_PgnDateTag_object_from_a_valid_tag() =
        let tag= parse pTag "[Date \"2013.05.15\"]"
        Assert.IsType<PgnDateTag>(tag) |> ignore
        Assert.Equal("Date", tag.Name)
        Assert.Equal(2013, (tag :?> PgnDateTag).Year.Value)
        Assert.Equal(5, (tag :?> PgnDateTag).Month.Value)
        Assert.Equal(15, (tag :?> PgnDateTag).Day.Value)

    [<Fact>]
    member this.pTag_should_accept_only_the_year_as_date() =
        let tag= parse pTag "[Date \"2013\"]"
        Assert.IsType<PgnDateTag>(tag) |> ignore
        Assert.Equal("Date", tag.Name)
        Assert.Equal(2013, (tag :?> PgnDateTag).Year.Value)
        Assert.False((tag :?> PgnDateTag).Month.HasValue)
        Assert.False((tag :?> PgnDateTag).Day.HasValue)

    [<Fact>]
    member this.pTag_should_create_a_PgnRoundTag_object_from_a_valid_tag() =
        let tag= parse pTag "[Round \"13\"]"
        Assert.IsType<PgnTag>(tag) |> ignore
        Assert.Equal("Round", tag.Name)
        Assert.Equal("13", tag.Value)

    [<Fact>]
    member this.pTag_should_create_PgnRoundTag_object_from_two_tags_in_sequence() =
        let tag = parse pTag @"[Round ""?""][White ""Tarrasch, Siegbert""]"

        Assert.IsType<PgnTag>(tag) |> ignore
        Assert.Equal("Round", tag.Name)
        Assert.Equal("?", tag.Value)

    [<Fact>]
    member this.pTag_should_accept_non_numeric_rounds() =
        let tag= parse pTag "[Round \"4.1\"]"
        Assert.IsType<PgnTag>(tag) |> ignore
        Assert.Equal("Round", tag.Name)
        Assert.Equal("4.1", tag.Value)

    [<Fact>]
    member this.pTag_should_create_a_PgnResultTag_object_from_a_valid_tag() =
        let tag= parse pTag "[Result \"1-0\"]"
        Assert.IsType<PgnResultTag>(tag) |> ignore
        Assert.Equal("Result", tag.Name)
        Assert.Equal(GameResult.White, (tag :?> PgnResultTag).Result)

    [<Fact>]
    member this.pTag_should_create_a_FenTag_object_from_a_valid_tag() =
        let tag= parse pTag "[FEN \"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1\"]"
        Assert.IsType<FenTag>(tag) |> ignore

        let setup= (tag :?> FenTag).Setup

        Assert.Equal(Piece.BlackRook, setup.[File.A, 1])
        Assert.Equal(Piece.WhiteKnight, setup.[File.B, 8])
        Assert.Equal(Piece.BlackBishop, setup.[File.C, 1])
        Assert.Equal(null, setup.[File.C, 5])
        Assert.Equal(Piece.WhiteKing, setup.[File.E, 8])

        Assert.Equal(true, setup.IsWhiteMove)

        Assert.Equal(true, setup.CanWhiteCastleKingSide)
        Assert.Equal(true, setup.CanWhiteCastleQueenSide)
        Assert.Equal(true, setup.CanBlackCastleKingSide)
        Assert.Equal(true, setup.CanBlackCastleQueenSide)

        Assert.Equal(null, setup.EnPassantSquare)

        Assert.Equal(0, setup.HalfMoveClock)
        Assert.Equal(1, setup.FullMoveCount)


    [<Fact>]
    member this.pTag_should_create_a_FenTag_object_from_another_valid_tag() =
        let tag= parse pTag "[FEN \"rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR b Kq c6 1 2\"]"
        Assert.IsType<FenTag>(tag) |> ignore

        let setup= (tag :?> FenTag).Setup

        Assert.Equal(Piece.BlackRook, setup.[File.A, 1])
        Assert.Equal(Piece.BlackPawn, setup.[File.B, 2])
        Assert.Equal(Piece.BlackPawn, setup.[File.C, 4])
        Assert.Equal(Piece.WhitePawn, setup.[File.E, 5])
        Assert.Equal(null, setup.[File.E, 7])
        Assert.Equal(Piece.WhiteKing, setup.[File.E, 8])

        Assert.Equal(false, setup.IsWhiteMove)

        Assert.Equal(true, setup.CanWhiteCastleKingSide)
        Assert.Equal(false, setup.CanWhiteCastleQueenSide)
        Assert.Equal(false, setup.CanBlackCastleKingSide)
        Assert.Equal(true, setup.CanBlackCastleQueenSide)

        Assert.Equal(Square(File.C, 6), setup.EnPassantSquare)

        Assert.Equal(1, setup.HalfMoveClock)
        Assert.Equal(2, setup.FullMoveCount)