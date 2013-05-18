namespace ilf.pgn.Test

open ilf.pgn
open ilf.pgn.PgnParsers.Game
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting


[<TestClass>]
type GameParserTest() = 
    let defaultGame = "
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

    [<TestMethod>]
    member this.pGame_should_accept_a_standard_pgn_game() =
        let (tags, moves)= parse pGameRaw defaultGame
        Assert.AreEqual(10, tags.Length);
        Assert.AreEqual(25, moves.Length); //24 move pairs and finish tag
        Assert.AreEqual(MoveEntryType.GameEndWhite, moves.Item(24).Type);

    [<TestMethod; ExpectedException(typeof<PgnFormatException>)>]
    member this.pGame_should_raise_an_exception_if_less_than_7_tags_are_defined() =
        tryParse pGame "
        [Event \"Breslau\"]
        [Site \"Breslau\"]
        [Date \"1879.??.??\"]
        [Round \"?\"]
        [White \"Tarrasch, Siegbert\"]
        [Black \"Mendelsohn, J.\"]

        1.e4"

    [<TestMethod>]
    member this.pGame_should_set_event_correctly() =
        let game= parse pGame defaultGame
        Assert.AreEqual("Breslau", game.Event)

    [<TestMethod>]
    member this.pGame_should_set_site_correctly() =
        let game= parse pGame defaultGame
        Assert.AreEqual("Breslau", game.Site)

