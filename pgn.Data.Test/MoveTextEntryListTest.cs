using System.Collections.Generic;
using System.Linq;
using Xunit;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Test
{
    public class MoveTextEntryListTest
    {
        public const string TestGameString =
    @"[Event ""Breslau""]
[Site ""Breslau""]
[Date ""1879.??.??""]
[Round ""?""]
[White ""Tarrasch, Siegbert""]
[Black ""Mendelsohn, J.""]
[Result ""1-0""]

1.e4 {...} e5 2.Nf3 Nc6 3.Nc3 Nf6 4.Bb5 Bb4 5.Nd5 Nxd5 6.exd5 Nd4 7.Ba4 b5 8.Nxd4 bxa4
9.Nf3 O-O 10.O-O d6 11.c3 Bc5 12.d4 exd4 13.Nxd4 Ba6 14.Re1 Bc4 15.Nc6 Qf6
16.Be3 Rfe8 17.Bxc5 Rxe1+ 18.Qxe1 dxc5 19.Qe4 Bb5 20.d6 Kf8 21.Ne7 Re8 22.Qxh7 Qxd6
23.Re1 Be2 24.Nf5  1-0";

        [Fact]
        public void FullMoveCount_should_return_move_count_without_comments()
        {
            var sut = new MoveTextEntryList
                {
                    new CommentEntry("foo"),
                    new NAGEntry(1),
                    new RAVEntry(new MoveTextEntryList {new MovePairEntry(new Move(), new Move())}),
                    new MovePairEntry(new Move(), new Move()),
                    new MovePairEntry(new Move(), new Move()),
                    new GameEndEntry(GameResult.Draw)
                };

            Assert.Equal(2, sut.FullMoveCount);
            Assert.Equal(4, sut.MoveCount);
        }

        [Fact]
        public void FullMoveCount_should_not_count_single_halfmoves()
        {
            var sut = new MoveTextEntryList { new HalfMoveEntry(new Move()) };
            Assert.Equal(0, sut.FullMoveCount);
            Assert.Equal(1, sut.MoveCount);

        }

        [Fact]
        public void FullMoveCount_should_count_pairwise_halfmoves()
        {
            var sut = new MoveTextEntryList { new HalfMoveEntry(new Move()), new HalfMoveEntry(new Move()) };
            Assert.Equal(1, sut.FullMoveCount);
            Assert.Equal(2, sut.MoveCount);
        }

        // [Fact]
        // public void Moves_should_return_an_enumeration_of_moves()
        // {
        //     var parser = new PgnReader();
        //     var db = parser.ReadFromString(TestGameString);
        //     var moveText = db.Games[0].MoveText;

        //     var moves = (from c in moveText.GetMoves() select c).ToList();
        //     Assert.Equal(new Square(File.E, 4), moves[0].TargetSquare);
        //     Assert.Equal(new Square(File.E, 5), moves[1].TargetSquare);
        // }
    }
}
