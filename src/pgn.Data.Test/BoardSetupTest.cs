using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Data.Test
{
    [TestClass]
    public class BoardSetupTest
    {
        [TestMethod]
        public void can_construct()
        {
            new BoardSetup();
        }
        [TestMethod]
        public void Index_accessor_should_set_and_get_Pieces()
        {
            var sut = new BoardSetup();
            sut[0, 4] = Piece.BlackKnight;

            Assert.AreEqual(Piece.BlackKnight, sut[0, 4]);
        }
        [TestMethod]
        public void Index_accessor_should_set_and_get_Pieces_via_file_and_rank()
        {
            var sut = new BoardSetup();
            sut[File.A, 5] = Piece.BlackKnight;

            Assert.AreEqual(Piece.BlackKnight, sut[File.A, 5]);
        }
        [TestMethod]
        public void Index_accessor_should_set_and_get_Pieces_via_Square()
        {
            var sut = new BoardSetup();
            sut[new Square(File.A, 5)] = Piece.BlackKnight;

            Assert.AreEqual(Piece.BlackKnight, sut[new Square(File.A, 5)]);
        }
        [TestMethod]
        public void Index_accessor_should_set_and_get_Pieces_via_integer()
        {
            var sut = new BoardSetup();
            sut[32] = Piece.BlackKnight;

            Assert.AreEqual(Piece.BlackKnight, sut[File.A, 5]);
        }

        [TestMethod]
        public void square_number_correspondation_test()
        {
            var sut = new BoardSetup();
            sut[0] = Piece.BlackPawn;
            sut[1] = Piece.WhiteKing;
            sut[59] = Piece.BlackRook;
            sut[36] = Piece.WhiteQueen;
            sut[63] = Piece.BlackBishop;

            Assert.AreEqual(Piece.BlackPawn, sut[File.A, 1]);
            Assert.AreEqual(Piece.WhiteKing, sut[File.B, 1]);
            Assert.AreEqual(Piece.BlackRook, sut[File.D, 8]);
            Assert.AreEqual(Piece.WhiteQueen, sut[File.E, 5]);
            Assert.AreEqual(Piece.BlackBishop, sut[File.H, 8]);
        }
    }
}
