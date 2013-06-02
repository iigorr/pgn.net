using System.ComponentModel;

namespace ilf.pgn.Data
{
    public class BoardSetup
    {
        private readonly Piece[,] _board = new Piece[8, 8];

        public Piece this[int file, int rank]
        {
            get { return _board[file, rank]; }
            set { _board[file, rank] = value; }
        }

        public Piece this[File file, int rank]
        {
            get { return this[(int)file - 1, rank - 1]; }
            set { this[(int)file - 1, rank - 1] = value; }
        }

        public Piece this[Square square]
        {
            get { return this[(int)square.File, square.Rank]; }
            set { this[(int)square.File, square.Rank] = value; }
        }

        public Piece this[int pos]
        {
            get { return this[pos / 8, pos % 8]; }
            set { this[pos / 8, pos % 8] = value; }
        }

        [DefaultValue(true)]
        public bool IsWhiteMove { get; set; }

        public bool CanWhiteCastleKingSide { get; set; }
        public bool CanWhiteCastleQueenSide { get; set; }
        public bool CanBlackCastleKingSide { get; set; }
        public bool CanBlackCastleQueenSide { get; set; }

        public Square EnPassantSquare { get; set; }

        public int HalfMoveClock { get; set; }
        public int FullMoveCount { get; set; }
    }
}
