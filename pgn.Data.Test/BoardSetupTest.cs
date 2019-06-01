using Xunit;

namespace ilf.pgn.Data.Test
{
    public class BoardSetupTest
    {
        [Fact]
        public void can_construct()
        {
            new BoardSetup();
        }
        [Fact]
        public void Index_accessor_should_set_and_get_Pieces()
        {
            var sut = new BoardSetup();
            sut[0, 4] = Piece.BlackKnight;

            Assert.Equal(Piece.BlackKnight, sut[0, 4]);
        }
        [Fact]
        public void Index_accessor_should_set_and_get_Pieces_via_file_and_rank()
        {
            var sut = new BoardSetup();
            sut[File.A, 5] = Piece.BlackKnight;

            Assert.Equal(Piece.BlackKnight, sut[File.A, 5]);
        }
        [Fact]
        public void Index_accessor_should_set_and_get_Pieces_via_Square()
        {
            var sut = new BoardSetup();
            sut[new Square(File.A, 5)] = Piece.BlackKnight;

            Assert.Equal(Piece.BlackKnight, sut[new Square(File.A, 5)]);
        }
        [Fact]
        public void Index_accessor_should_set_and_get_Pieces_via_integer()
        {
            var sut = new BoardSetup();
            sut[32] = Piece.BlackKnight;

            Assert.Equal(Piece.BlackKnight, sut[File.A, 5]);
        }

        [Fact]
        public void square_number_correspondation_test()
        {
            var sut = new BoardSetup();
            sut[0] = Piece.BlackPawn;
            sut[1] = Piece.WhiteKing;
            sut[59] = Piece.BlackRook;
            sut[36] = Piece.WhiteQueen;
            sut[63] = Piece.BlackBishop;

            Assert.Equal(Piece.BlackPawn, sut[File.A, 1]);
            Assert.Equal(Piece.WhiteKing, sut[File.B, 1]);
            Assert.Equal(Piece.BlackRook, sut[File.D, 8]);
            Assert.Equal(Piece.WhiteQueen, sut[File.E, 5]);
            Assert.Equal(Piece.BlackBishop, sut[File.H, 8]);
        }
    }
}
