namespace ilf.pgn.Data
{
    /// <summary>
    /// Pieces on the board (combination of type and color). There is a static 
    /// instance for each of them, e.g. Piece.WhiteRook or Piece.BlackPawn. 
    /// </summary>
    public class Piece
    {
        /// <summary>White Pawn</summary>
        public readonly static Piece WhitePawn = new Piece(PieceType.Pawn, Color.White);
        /// <summary>White Knight</summary>
        public readonly static Piece WhiteKnight = new Piece(PieceType.Knight, Color.White);
        /// <summary>White Bishop</summary>
        public readonly static Piece WhiteBishop = new Piece(PieceType.Bishop, Color.White);
        /// <summary>White Rook</summary>
        public readonly static Piece WhiteRook = new Piece(PieceType.Rook, Color.White);
        /// <summary>White Queen</summary>
        public readonly static Piece WhiteQueen = new Piece(PieceType.Queen, Color.White);
        /// <summary>White King</summary>
        public readonly static Piece WhiteKing = new Piece(PieceType.King, Color.White);

        /// <summary>Black Pawn</summary>
        public readonly static Piece BlackPawn = new Piece(PieceType.Pawn, Color.Black);
        /// <summary>Black Knight</summary>
        public readonly static Piece BlackKnight = new Piece(PieceType.Knight, Color.Black);
        /// <summary>Black Bishop</summary>
        public readonly static Piece BlackBishop = new Piece(PieceType.Bishop, Color.Black);
        /// <summary>Black Rook</summary>
        public readonly static Piece BlackRook = new Piece(PieceType.Rook, Color.Black);
        /// <summary>Black Queen</summary>
        public readonly static Piece BlackQueen = new Piece(PieceType.Queen, Color.Black);
        /// <summary>Black King</summary>
        public readonly static Piece BlackKing = new Piece(PieceType.King, Color.Black);

        /// <summary>
        /// Prevents the construction of further pieces. All pieces have static instances.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="color">The color.</param>
        private Piece(PieceType type, Color color)
        {
            PieceType = type;
            Color = color;
        }
        /// <summary>
        /// Gets the type of the piece.
        /// </summary>
        /// <value>
        /// The type of the piece.
        /// </value>
        public PieceType PieceType { get; private set; }
        /// <summary>
        /// Gets the color of the piece.
        /// </summary>
        /// <value>
        /// The color of the piece.
        /// </value>
        public Color Color { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the piece.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents the piece.
        /// </returns>
        public override string ToString()
        {
            return Color.ToString() + " " + PieceType.ToString();
        }
    }
}
