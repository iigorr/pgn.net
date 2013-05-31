using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Data.Format.Test
{
    [TestClass]
    public class MoveTextFormatterTest
    {
        private readonly Move _move1;
        private readonly Move _move2;



        public MoveTextFormatterTest()
        {
            _move1= new Move
            {
                Type = MoveType.Capture,
                TargetSquare = new Square(File.D, 5),
                OriginFile = File.E
            };

            _move2 = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.D, 4),
                Piece = Piece.Knight
            };
        }

        [TestMethod]
        public void can_create()
        {
            new MoveTextFormatter();
        }

        [TestMethod]
        public void Format_should_accept_TextWriter()
        {
            var sut = new MoveTextFormatter();
            var writer = new StringWriter();
            writer.Write("Foo ");

            var movePair = new MovePairEntry(_move1, _move2);

            sut.Format(movePair, writer);

            Assert.AreEqual("Foo exd5 Nd4", writer.ToString());
        }

        [TestMethod]
        public void Format_should_format_move_pair()
        {
            var sut = new MoveTextFormatter();
            var movePair = new MovePairEntry(_move1, _move2);

            Assert.AreEqual("exd5 Nd4", sut.Format(movePair));
        }

        [TestMethod]
        public void Format_should_format_move_pair_with_number()
        {
            var sut = new MoveTextFormatter();
            var movePair = new MovePairEntry(_move1, _move2) { MoveNumber = 6 };

            Assert.AreEqual("6. exd5 Nd4", sut.Format(movePair));
        }
    }
}
