namespace ilf.pgn.Test

open ilf.pgn
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
    member this.pMovePair_should_accept_move_pair() =
        tryParse pMoveSeriesEntry "1. e2e4 Nb8c6"
        tryParse pMoveSeriesEntry "1 .   e2e4   Nb8c6"    
    
    [<TestMethod>]
    member this.pMovePair_should_accept_a_split_move_pair() =
        tryParse pMoveSeriesEntry "1. e2e4 \n 1... Nb8c6"

    [<TestMethod>]
    member this.pMovePair_should_accept_a_single_move_by_white() =
        tryParse pMoveSeriesEntry "13.Nxd4"

    [<TestMethod>]
    member this.pMovePair_should_accept_a_continued_move_by_black() =
        tryParse pMoveSeriesEntry "13... Ba6"
