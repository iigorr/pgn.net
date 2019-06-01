namespace ilf.pgn.Test

open ilf.pgn.Data
open ilf.pgn.PgnParsers.Move
open ilf.pgn.PgnParsers.MoveSeries
open ilf.pgn.Test.TestBase

open Xunit

type MoveSeriesParserTest() =
    [<Fact>]
    member this.pMoveNumberIndicator_should_accept_simple_move_number() =
        tryParse pMoveNumberIndicator "1."
        tryParse pMoveNumberIndicator "104."

    [<Fact>]
    member this.pMoveNumberIndicator_should_accept_continued_move_number() =
        tryParse pMoveNumberIndicator "2..."
        tryParse pMoveNumberIndicator "54....."
        tryParse pMoveNumberIndicator "7…"

    [<Fact>]
    member this.pMoveNumberIndicator_should_accept_spaces_between_number_and_periods() =
        tryParse pMoveNumberIndicator "2  \t..."
        tryParse pMoveNumberIndicator "1    ."
        tryParse pMoveNumberIndicator "54\n....."
        tryParse pMoveNumberIndicator "7 …"

    [<Fact>]
    member this.pMoveSeriesEntry_should_accept_move_pair() =
        tryParse pMoveSeriesEntry "1. e2e4 Nb8c6"
        tryParse pMoveSeriesEntry "1 .   e2e4   Nb8c6"



    [<Fact>]
    member this.pMoveSeriesEntry_should_accept_a_full_move_pair() =
        let entry = (parse pMoveSeriesEntry "1. e2e4 Nb8c6") :?> MovePairEntry
        let moveWhite = parse pMove "e2e4"
        let moveBlack = parse pMove "Nb8c6"

        Assert.Equal(moveWhite, entry.White)
        Assert.Equal(moveBlack, entry.Black)
        Assert.Equal(1, entry.MoveNumber.Value)

    [<Fact>]
    member this.pMoveSeriesEntry_should_accept_a_split_move_white() =
        let entry = (parse pMoveSeriesEntry "1. e2e4") :?> HalfMoveEntry
        let moveWhite = parse pMove "e2e4"

        Assert.Equal(moveWhite, entry.Move)
        Assert.Equal(1, entry.MoveNumber.Value)


    [<Fact>]
    member this.pMoveSeriesEntry_should_accept_a_single_move_by_white() =
        let entry = (parse pMoveSeriesEntry "13.Nxd4") :?> HalfMoveEntry
        let move = parse pMove "Nxd4"

        Assert.Equal(move, entry.Move)
        Assert.Equal(13, entry.MoveNumber.Value)
        Assert.Equal(false, entry.IsContinued)


    [<Fact>]
    member this.pMoveSeriesEntry_should_accept_a_continued_move_by_black() =
        let entry = (parse pMoveSeriesEntry "13... Ba6") :?> HalfMoveEntry
        let move = parse pMove "Ba6"

        Assert.Equal(move, entry.Move)
        Assert.Equal(13, entry.MoveNumber.Value)
        Assert.Equal(true, entry.IsContinued)

    [<Fact>]
    member this.pMoveSeries_should_accept_a_moveSeries() =
        let moveSeries = parse pMoveSeries "1. e4 c5 2. Nf3 d6 3. Bb5+ Bd7"

        Assert.Equal(3, moveSeries.Length)

    [<Fact>]
    member this.pMoveSeries_should_accept_a_moveSeries_with_split_moves() =
        let moveSeries = parse pMoveSeries "1. e4 c5 2. Nf3 \n 2... d6 3. Bb5+ Bd7"

        Assert.Equal(4, moveSeries.Length)

    [<Fact>]
    member this.pMoveEntry_should_accept_comments_in_braces() =
        let entry = (parse pMoveSeriesEntry "{this is a comment}") :?> CommentEntry

        Assert.Equal("this is a comment", entry.Comment)

    [<Fact>]
    member this.pMoveEntry_should_accept_comments_semicolon_comment() =
        let moveSeries = parse pMoveSeries "1. e4 e5 2. Nf3 Nc6 3. Bb5 ;This opening is called the Ruy Lopez.
        3... a6"

        Assert.Equal(5, moveSeries.Length)

        let commentEntry= (moveSeries.Item(3)) :?> CommentEntry
        Assert.Equal("This opening is called the Ruy Lopez.", commentEntry.Comment)

    [<Fact>]
    member this.pMoveEntry_should_accept_multiple_line_comments() =
        let moveSeries = parse pMoveSeries "1. e4 e5 2. Nf3 Nc6 3. Bb5 ;This opening is called the Ruy Lopez.
        ;Another comment
        3... a6"

        Assert.Equal(6, moveSeries.Length)

        let commentEntry1= (moveSeries.Item(3)) :?> CommentEntry
        let commentEntry2= (moveSeries.Item(4)) :?> CommentEntry
        Assert.Equal("This opening is called the Ruy Lopez.", commentEntry1.Comment)
        Assert.Equal("Another comment", commentEntry2.Comment)


    [<Fact>]
    member this.pEndOfGame_should_accept_Draw() =
        let entry = (parse pEndOfGame "1/2 - 1/2") :?> GameEndEntry
        Assert.Equal(GameResult.Draw, entry.Result)

    [<Fact>]
    member this.pEndOfGame_should_accept_utf8_draw() =
        let entry = (parse pEndOfGame "½-½") :?> GameEndEntry
        Assert.Equal(GameResult.Draw, entry.Result)

    [<Fact>]
    member this.pEndOfGame_should_accept_WhiteWin() =
        let entry = (parse pEndOfGame "1-0") :?> GameEndEntry
        Assert.Equal(GameResult.White, entry.Result)

    [<Fact>]
    member this.pEndOfGame_should_accept_BlackWin() =
        let entry = (parse pEndOfGame "0-1") :?> GameEndEntry
        Assert.Equal(GameResult.Black, entry.Result)

    [<Fact>]
    member this.pEndOfGame_should_accept_EndOpen() =
        let entry = (parse pEndOfGame "*") :?> GameEndEntry
        Assert.Equal(GameResult.Open, entry.Result)

    [<Fact>]
    member this.pMoveSeries_should_accept_comment_after_end_game() =
        let entries = parse pMoveSeries "1-0 {impressive game!}"

        let commentEntry= (entries.Item(1)) :?> CommentEntry
        Assert.Equal("impressive game!", commentEntry.Comment)


    [<Fact>]
    member this.pMoveSeries_should_accept_comment_before_moves() =
        let entries = parse pMoveSeries "{This game is gonna be awesome! Watch this} \n 1. e4 e5 2. Nf3"

        Assert.Equal(MoveTextEntryType.Comment, entries.Item(0).Type)

    [<Fact>]
    member this.pMoveSeries_should_accept_comment_between_moves() =
        let entries = parse pMoveSeries "1. e4 {[%emt 0.0]} c5 {[%emt 0.0]} 2. Nc3"

        Assert.Equal(5, entries.Length)
        let commentEntry= (entries.Item(1)) :?> CommentEntry
        Assert.Equal("[%emt 0.0]", commentEntry.Comment)


    [<Fact>]
    member this.pMoveSeries_should_accept_NAGs() =
        let entries = parse pMoveSeries "1. e4 c5 $6 "

        Assert.Equal(2, entries.Length)
        let nagEntry= (entries.Item(1)) :?> NAGEntry
        Assert.Equal(6, nagEntry.Code)


    [<Fact>]
    member this.pMoveSeries_should_accept_RAVs() =
        let entries = parse pMoveSeries "6. d5 $6 (6. Bd3 cxd4 7. exd4 d5 { - B14 }) 6... exd5"

        Assert.Equal(4, entries.Length)
        let ravEntry= (entries.Item(2)) :?> RAVEntry
        Assert.Equal(3, ravEntry.MoveText.Count)
        Assert.Equal(MoveTextEntryType.MovePair, ravEntry.MoveText.Item(0).Type)
        Assert.Equal(MoveTextEntryType.MovePair, ravEntry.MoveText.Item(1).Type)
        Assert.Equal(MoveTextEntryType.Comment, ravEntry.MoveText.Item(2).Type)


    [<Fact>]
    member this.pMoveSeries_should_accept_nested_RAVs() =
        let entries = parse pMoveSeries "6. d5 (6. Bd3 cxd4 7. exd4 d5 (7... Qa4)) 6... exd5"

        Assert.Equal(3, entries.Length)
        let ravEntry1= (entries.Item(1)) :?> RAVEntry
        Assert.Equal(3, ravEntry1.MoveText.Count)

        let ravEntry2= (ravEntry1.MoveText.Item(2)) :?> RAVEntry
        Assert.Equal(1, ravEntry2.MoveText.Count)
        Assert.Equal(MoveTextEntryType.SingleMove, ravEntry2.MoveText.Item(0).Type)