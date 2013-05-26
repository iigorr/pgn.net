namespace ilf.pgn.Test

open ilf.pgn.Data
open ilf.pgn.Exception
open ilf.pgn.PgnParsers.Game
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting


[<TestClass>]
type GameParserTest() = 
    let testGame1 = "
    [Event \"Breslau\"]
    [Site \"Breslau\"]
    [Date \"1879.??.??\"]
    [Round \"?\"]
    [White \"Tarrasch, Siegbert\"]
    [Black \"Mendelsohn, J.\"]
    [Result \"1-0\"]
    [WhiteElo \"\"]
    [BlackElo \"\"]
    [ECO \"C49\"]

    1.e4 e5 2.Nf3 Nc6 3.Nc3 Nf6 4.Bb5 Bb4 5.Nd5 Nxd5 6.exd5 Nd4 7.Ba4 b5 8.Nxd4 bxa4
    9.Nf3 O-O 10.O-O d6 11.c3 Bc5 12.d4 exd4 13.Nxd4 Ba6 14.Re1 Bc4 15.Nc6 Qf6
    16.Be3 Rfe8 17.Bxc5 Rxe1+ 18.Qxe1 dxc5 19.Qe4 Bb5 20.d6 Kf8 21.Ne7 Re8 22.Qxh7 Qxd6
    23.Re1 Be2 24.Nf5  1-0"

    let testGame2= "
    [Event \"Braingames WCC\"]
[Site \"London ENG\"]
[Date \"2000.11.02\"]
[Round \"15\"]
[White \"Kasparov,G\"]
[Black \"Kramnik,V\"]
[Result \"1/2-1/2\"]
[WhiteElo \"2849\"]
[BlackElo \"2770\"]
[ECO \"E05\"]

1.d4 Nf6 2.c4 e6 3.g3 d5 4.Bg2 Be7 5.Nf3 O-O 6.O-O dxc4 7.Qc2 a6 8.Qxc4 b5
9.Qc2 Bb7 10.Bd2 Be4 11.Qc1 Bb7 12.Bf4 Bd6 13.Nbd2 Nbd7 14.Nb3 Bd5 15.Rd1 Qe7
16.Ne5 Bxg2 17.Kxg2 Nd5 18.Nc6 Nxf4+ 19.Qxf4 Qe8 20.Qf3 e5 21.dxe5 Nxe5 22.Nxe5 Qxe5
23.Rd2 Rae8 24.e3 Re6 25.Rad1 Rf6 26.Qd5 Qe8 27.Rc1 g6 28.Rdc2 h5 29.Nd2 Rf5
30.Qe4 c5 31.Qxe8 Rxe8 32.e4 Rfe5 33.f4 R5e6 34.e5 Be7 35.b3 f6 36.Nf3 fxe5
37.Nxe5 Rd8 38.h4 Rd5  1/2-1/2"

    [<TestMethod>]
    member this.pGame_should_accept_a_standard_pgn_game() =
        let game= parse pGame testGame1
        Assert.AreEqual("Tarrasch, Siegbert", game.WhitePlayer);
        Assert.AreEqual(25, game.MoveText.Count); //24 move pairs and finish tag
        Assert.AreEqual(MoveTextEntryType.GameEnd, game.MoveText.Item(24).Type);

    [<TestMethod>]
    member this.pGame_should_set_event_correctly() =
        let game= parse pGame testGame1
        Assert.AreEqual("Breslau", game.Event)

    [<TestMethod>]
    member this.pGame_should_set_site_correctly() =
        let game= parse pGame testGame1
        Assert.AreEqual("Breslau", game.Site)

    [<TestMethod>]
    member this.pGame_should_set_Date_correctly() =
        let game= parse pGame testGame2
        Assert.AreEqual(2000, game.Year)
        Assert.AreEqual(11, game.Month)
        Assert.AreEqual(2, game.Day)

    [<TestMethod>]
    member this.pGame_should_set_incomplete_date_correctly() =
        let game= parse pGame testGame1
        Assert.AreEqual(1879, game.Year)
        Assert.AreEqual(null, game.Month)
        Assert.AreEqual(null, game.Day)

    [<TestMethod>]
    member this.pGame_should_set_round_correctly() =
        let game= parse pGame testGame2
        Assert.AreEqual("15", game.Round)

    [<TestMethod>]
    member this.pGame_should_set_players_correctly() =
        let game= parse pGame testGame1
        Assert.AreEqual("Tarrasch, Siegbert", game.WhitePlayer)
        Assert.AreEqual("Mendelsohn, J.", game.BlackPlayer)

    [<TestMethod>]
    member this.pGame_should_set_game_result_correctly() =
        let game= parse pGame testGame2
        Assert.AreEqual(GameResult.Draw, game.Result)


    [<TestMethod>]
    member this.pGame_should_set_all_other_tags_correctly() =
        let game= parse pGame testGame2

        Assert.AreEqual(3, game.AdditionalInfo.Count)

    [<TestMethod>]
    member this.pGame_should_set_the_move_text() =
        let game= parse pGame testGame2

        Assert.AreEqual(39, game.MoveText.Count)


    [<TestMethod; ExpectedException(typeof<PgnFormatException>)>]
    member this.pGame_should_disallow_non_game_data_before_end_of_file() =
        tryParse pDatabase (testGame2 + "   x")