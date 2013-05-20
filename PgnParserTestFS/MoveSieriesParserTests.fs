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
        let entry = (parse pMoveSeriesEntry "1. e2e4 Nb8c6") :?> MovePairEntry
        let moveWhite = parse pMove "e2e4" 
        let moveBlack = parse pMove "Nb8c6"
         
        Assert.AreEqual(moveWhite, entry.White)
        Assert.AreEqual(moveBlack, entry.Black)

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_split_move_white() =
        let entry = (parse pMoveSeriesEntry "1. e2e4") :?> SingleMoveEntry
        let moveWhite = parse pMove "e2e4" 
         
        Assert.AreEqual(moveWhite, entry.Move)

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_single_move_by_white() =
        let entry = (parse pMoveSeriesEntry "13.Nxd4") :?> SingleMoveEntry
        let move = parse pMove "Nxd4"
         
        Assert.AreEqual(move, entry.Move)

    [<TestMethod>]
    member this.pMoveSeriesEntry_should_accept_a_continued_move_by_black() =
        let entry = (parse pMoveSeriesEntry "13... Ba6") :?> SingleMoveEntry
        let move = parse pMove "Ba6"
         
        Assert.AreEqual(move, entry.Move)

    [<TestMethod>]
    member this.pMoveSeries_should_accept_a_moveSeries() =
        let moveSeries = parse pMoveSeries "1. e4 c5 2. Nf3 d6 3. Bb5+ Bd7"
         
        Assert.AreEqual(3, moveSeries.Length)

    [<TestMethod>]
    member this.pMoveSeries_should_accept_a_moveSeries_with_split_moves() =
        let moveSeries = parse pMoveSeries "1. e4 c5 2. Nf3 \n 2... d6 3. Bb5+ Bd7"
         
        Assert.AreEqual(4, moveSeries.Length)

    [<TestMethod>]
    member this.pMoveEntry_should_accept_comments_in_braces() =
        let entry = (parse pMoveSeriesEntry "{this is a comment}") :?> CommentEntry

        Assert.AreEqual("this is a comment", entry.Comment)

    [<TestMethod>]
    member this.pMoveEntry_should_accept_comments_in_parantheses() =
        let entry = (parse pMoveSeriesEntry "(this is a comment)") :?> CommentEntry

        Assert.AreEqual("this is a comment", entry.Comment)
        
    [<TestMethod>]
    member this.pMoveEntry_should_accept_comments_in_square_brackets() =
        let entry = (parse pMoveSeriesEntry "[this is a comment]") :?> CommentEntry

        Assert.AreEqual("this is a comment", entry.Comment)


    [<TestMethod>]
    member this.pMoveEntry_should_accept_comments_semicolon_comment() =
        let moveSeries = parse pMoveSeries "1. e4 e5 2. Nf3 Nc6 3. Bb5 ;This opening is called the Ruy Lopez.
        3... a6"

        Assert.AreEqual(5, moveSeries.Length)

        let commentEntry= (moveSeries.Item(3)) :?> CommentEntry
        Assert.AreEqual("This opening is called the Ruy Lopez.", commentEntry.Comment) 

    [<TestMethod>]
    member this.pMoveEntry_should_accept_multiple_line_comments() =
        let moveSeries = parse pMoveSeries "1. e4 e5 2. Nf3 Nc6 3. Bb5 ;This opening is called the Ruy Lopez.
        ;Another comment
        3... a6"

        Assert.AreEqual(6, moveSeries.Length)
                
        let commentEntry1= (moveSeries.Item(3)) :?> CommentEntry
        let commentEntry2= (moveSeries.Item(4)) :?> CommentEntry
        Assert.AreEqual("This opening is called the Ruy Lopez.", commentEntry1.Comment) 
        Assert.AreEqual("Another comment", commentEntry2.Comment) 


    [<TestMethod>]
    member this.pEndOfGame_should_accept_Draw() =
        let entry = (parse pEndOfGame "1/2 - 1/2") :?> GameEndEntry

        Assert.AreEqual(GameResult.Draw, entry.Result)

    [<TestMethod>]
    member this.pEndOfGame_should_accept_WhiteWin() =
        let entry = (parse pEndOfGame "1-0") :?> GameEndEntry

        Assert.AreEqual(GameResult.White, entry.Result)

    [<TestMethod>]
    member this.pEndOfGame_should_accept_BlackWin() =
        let entry = (parse pEndOfGame "0-1") :?> GameEndEntry

        Assert.AreEqual(GameResult.Black, entry.Result)

    [<TestMethod>]
    member this.pEndOfGame_should_accept_EndOpen() =
        let entry = (parse pEndOfGame "*") :?> GameEndEntry

        Assert.AreEqual(GameResult.Open, entry.Result)

    [<TestMethod>]
    member this.pMoveSeries_should_accept_comment_after_end_game() =
        let entries = parse pMoveSeries "1-0 {impressive game!}"

        let commentEntry= (entries.Item(1)) :?> CommentEntry
        Assert.AreEqual("impressive game!", commentEntry.Comment) 


    [<TestMethod>]
    member this.pMoveSeries_should_accept_comment_before_moves() =
        let entries = parse pMoveSeries "{This game is gonna be awesome! Watch this} \n 1. e4 e5 2. Nf3"

        Assert.AreEqual(MoveTextEntryType.Comment, entries.Item(0).Type)

    [<TestMethod>]
    member this.pMoveSeries_should_accept_comment_between_moves() =
        let entries = parse pMoveSeries "1. e4 {[%emt 0.0]} c5 {[%emt 0.0]} 2. Nc3"

        Assert.AreEqual(5, entries.Length)
        let commentEntry= (entries.Item(1)) :?> CommentEntry
        Assert.AreEqual("[%emt 0.0]", commentEntry.Comment) 