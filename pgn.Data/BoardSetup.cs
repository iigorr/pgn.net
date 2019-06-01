using System.ComponentModel;

namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents a board setup. This is used in position practices or non-standard chess variants like Chess960
    /// </summary>
    public class BoardSetup
    {
        private readonly Piece[,] _board = new Piece[8, 8];

        /// <summary>
        /// Gets or sets a <see cref="Piece"/> on the specified square (defined as integers 0..7).
        /// </summary>
        /// <value>
        /// The <see cref="Piece"/>. Use <c>null</c> to unset.
        /// </value>
        /// <param name="file">The file.</param>
        /// <param name="rank">The rank.</param>
        /// <returns>The piece at the specified square or <c>null</c> if the square is empty.</returns>
        public Piece this[int file, int rank]
        {
            get { return _board[file, rank]; }
            set { _board[file, rank] = value; }
        }

        /// <summary>
        /// Gets or sets a <see cref="Piece"/> on the specified square (via file and rank).
        /// </summary>
        /// <value>
        /// The <see cref="Piece"/>. Use <c>null</c> to unset.
        /// </value>
        /// <param name="file">The file.</param>
        /// <param name="rank">The rank.</param>
        /// <returns>The piece at the specified square or <c>null</c> if the square is empty.</returns>
        public Piece this[File file, int rank]
        {
            get { return this[(int)file - 1, rank - 1]; }
            set { this[(int)file - 1, rank - 1] = value; }
        }

        /// <summary>
        /// Gets or sets a <see cref="Piece" /> on the specified square.
        /// </summary>
        /// <value>
        /// The <see cref="Piece" />. Use <c>null</c> to unset.
        /// </value>
        /// <param name="square">The square.</param>
        /// <returns>
        /// The piece at the specified square or <c>null</c> if the square is empty.
        /// </returns>
        public Piece this[Square square]
        {
            get { return this[square.File, square.Rank]; }
            set { this[square.File, square.Rank] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Piece"/> at the specified position. Counting starts at A1 rank-wise. 0=A1, 1=B1, ..., 7=H1, 8=A2, ..., 64=H8
        /// </summary>
        /// <value>
        /// The <see cref="Piece"/>. Use <c>null</c> to unset.
        /// </value>
        /// <param name="pos">The position.</param>
        /// <returns>The piece at the specified square or <c>null</c> if the square is empty.</returns>
        public Piece this[int pos]
        {
            get { return this[pos % 8, pos / 8]; }
            set { this[pos % 8, pos / 8] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is whites move (true) or blacks (false).
        /// </summary>
        /// <value>
        /// <c>true</c> if white is to move next; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(true)]
        public bool IsWhiteMove { get; set; }

        /// <summary>
        /// Indicates whether white can castle king-side.
        /// </summary>
        /// <value>
        /// <c>true</c> if white can castle king-side; otherwise, <c>false</c>.
        /// </value>
        public bool CanWhiteCastleKingSide { get; set; }

        /// <summary>
        /// Indicates whether white can castle queen-side.
        /// </summary>
        /// <value>
        /// <c>true</c> if white can castle queen-side; otherwise, <c>false</c>.
        /// </value>
        public bool CanWhiteCastleQueenSide { get; set; }

        /// <summary>
        /// Indicates whether black can castle king-side.
        /// </summary>
        /// <value>
        /// <c>true</c> if black can castle king-side; otherwise, <c>false</c>.
        /// </value>
        public bool CanBlackCastleKingSide { get; set; }

        /// <summary>
        /// Indicates whether black can castle queen-side.
        /// </summary>
        /// <value>
        /// <c>true</c> if black can castle queen-side; otherwise, <c>false</c>.
        /// </value>
        public bool CanBlackCastleQueenSide { get; set; }

        /// <summary>
        /// The en passant target square (if present). 
        /// </summary>
        /// <value>
        /// The en passant square or <c>null</c> if not en passant move is possible.
        /// </value>
        public Square EnPassantSquare { get; set; }

        /// <summary>
        /// Gets or sets the half move clock. 
        /// It is a nonnegative integer representing the halfmove clock. This number is the count of halfmoves (or ply) 
        /// since the last pawn advance or capturing move. This value is used for the fifty move draw rule.
        /// </summary>
        /// <value>
        /// The half move clock.
        /// </value>
        public int HalfMoveClock { get; set; }

        /// <summary>
        /// A positive integer that gives the fullmove number. This will have the value "1" for the first move of a game for both White and Black. It is incremented by one immediately after each move by Black.
        /// </summary>
        /// <value>
        /// The full move count.
        /// </value>
        public int FullMoveCount { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the board setup.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents the board setup.
        /// </returns>
        public override string ToString()
        {
            var output = "";
            for (int i = 7; i >= 0; --i)
            {
                output += "---------------------------------\n";
                output += "|";
                for (int j = 0; j < 8; ++j)
                    output += PieceToString(this[j, i]) + "|";
                output += "\n";
            }
            output += "\n---------------------------------";
            return output;
        }

        private string PieceToString(Piece p)
        {
            if (p == null) return "   ";

            var str="";
            switch (p.PieceType)
            {
                case PieceType.Pawn:
                    str = " p ";
                    break;
                case PieceType.Knight:
                    str = " n ";
                    break;
                case PieceType.Bishop:
                    str = " b ";
                    break;
                case PieceType.Rook:
                    str = " r ";
                    break;
                case PieceType.Queen:
                    str = " q ";
                    break;
                case PieceType.King:
                    str = " k ";
                    break;
            }

            if (p.Color == Color.White)
                return str.ToUpper();

            return str;
        }
    }
}
