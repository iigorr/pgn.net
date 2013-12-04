using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Test
{
    [TestClass]
    public class MoveTextEntryListTest
    {
        [TestMethod]
        public void Instance_should_be_a_list_of_MoveTextEntries()
        {
            Assert.IsInstanceOfType(new MoveTextEntryList(), typeof(List<MoveTextEntry>));
        }

        [TestMethod]
        public void FullMoveCount_should_return_move_count_without_comments()
        {
            var sut = new MoveTextEntryList
                {
                    new CommentEntry("foo"),
                    new NAGEntry(1),
                    new RAVEntry(new List<MoveTextEntry>{new MovePairEntry(new Move(), new Move())}),
                    new MovePairEntry(new Move(), new Move()),
                    new MovePairEntry(new Move(), new Move()),
                    new GameEndEntry(GameResult.Draw)
                };

            Assert.AreEqual(2, sut.FullMoveCount);
            Assert.AreEqual(4, sut.HalfMoveCount);
        }

        [TestMethod]
        public void FullMoveCount_should_not_count_single_halfmoves()
        {
            var sut = new MoveTextEntryList { new HalfMoveEntry(new Move()) };
            Assert.AreEqual(0, sut.FullMoveCount);
            Assert.AreEqual(1, sut.HalfMoveCount);

        }

        [TestMethod]
        public void FullMoveCount_should_count_pairwise_halfmoves()
        {
            var sut = new MoveTextEntryList { new HalfMoveEntry(new Move()), new HalfMoveEntry(new Move()) };
            Assert.AreEqual(1, sut.FullMoveCount);
            Assert.AreEqual(2, sut.HalfMoveCount);
        }

    }
}
