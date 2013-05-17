namespace ilf.pgn.Test

open ilf.pgn
open ilf.pgn.PgnParsers.Move
open ilf.pgn.PgnParsers.MoveSeries
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type MoveSeriesParserTest() = 
    [<TestMethod>]
    member this.pMoveNumberIndicator_should_accept_simple_move_number() =
        tryParse pMoveNumberIndicator "1."
        tryParse pMoveNumberIndicator "104."

    [<TestMethod>]
    member this.pMoveNumberIndicator_should_accept_continued_move_number() =
        tryParse pMoveNumberIndicator "2..."
        tryParse pMoveNumberIndicator "54....."
        tryParse pMoveNumberIndicator "7…"

    [<TestMethod>]
    member this.pMoveNumberIndicator_should_accept_spaces_between_number_and_periods() =
        tryParse pMoveNumberIndicator "2  \t..."
        tryParse pMoveNumberIndicator "1    ."
        tryParse pMoveNumberIndicator "54\n....."
        tryParse pMoveNumberIndicator "7 …"

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_move_pair() =
        tryParse pMoveSeriesEntry "1. e2e4 Nb8c6"
        tryParse pMoveSeriesEntry "1 .   e2e4   Nb8c6"


    
    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_full_move_pair() =
        let entry = parse pMoveSeriesEntry "1. e2e4 Nb8c6"
        let moveWhite = parse pMove "e2e4" 
        let moveBlack = parse pMove "Nb8c6"
         
        Assert.AreEqual(Some 1, entry.MoveNumber)
        Assert.AreEqual(moveWhite, entry.White)
        Assert.AreEqual(moveBlack, entry.Black)

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_split_move_white() =
        let entry = parse pMoveSeriesEntry "1. e2e4"
        let moveWhite = parse pMove "e2e4" 
        let moveBlack = parse pMove "Nb8c6"
         
        Assert.AreEqual(Some 1, entry.MoveNumber)
        Assert.AreEqual(moveWhite, entry.White)
        Assert.AreEqual(None, entry.Black)

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_single_move_by_white() =
        let entry = parse pMoveSeriesEntry "13.Nxd4"
        let move = parse pMove "Nxd4"
         
        Assert.AreEqual(Some 13, entry.MoveNumber)
        Assert.AreEqual(move, entry.White)
        Assert.AreEqual(None, entry.Black)

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_continued_move_by_black() =
        let entry = parse pMoveSeriesEntry "13... Ba6"
        let move = parse pMove "Ba6"
         
        Assert.AreEqual(Some 13, entry.MoveNumber)
        Assert.AreEqual(None, entry.White)
        Assert.AreEqual(move, entry.Black)

    [<TestMethod>]
    member this.pMoveSeries_should_accept_a_moveSeries() =
        let moveSeries = parse pMoveSeries "1. e4 c5 2. Nf3 d6 3. Bb5+ Bd7"
         
        Assert.AreEqual(3, moveSeries.Length)

    [<TestMethod>]
    member this.pMoveSeries_should_accept_a_moveSeries_with_split_moves() =
        let moveSeries = parse pMoveSeries "1. e4 c5 2. Nf3 \n 2... d6 3. Bb5+ Bd7"
         
        Assert.AreEqual(4, moveSeries.Length)