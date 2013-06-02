namespace ilf.pgn.Data
{
    public class Piece
    {
        public readonly static Piece WhitePawn = new Piece(PieceType.Pawn, Color.White);
        public readonly static Piece WhiteKnight = new Piece(PieceType.Knight, Color.White);
        public readonly static Piece WhiteBishop = new Piece(PieceType.Bishop, Color.White);
        public readonly static Piece WhiteRook = new Piece(PieceType.Rook, Color.White);
        public readonly static Piece WhiteQueen = new Piece(PieceType.Queen, Color.White);
        public readonly static Piece WhiteKing = new Piece(PieceType.King, Color.White);

        public readonly static Piece BlackPawn = new Piece(PieceType.Pawn, Color.Black);
        public readonly static Piece BlackKnight = new Piece(PieceType.Knight, Color.Black);
        public readonly static Piece BlackBishop = new Piece(PieceType.Bishop, Color.Black);
        public readonly static Piece BlackRook = new Piece(PieceType.Rook, Color.Black);
        public readonly static Piece BlackQueen = new Piece(PieceType.Queen, Color.Black);
        public readonly static Piece BlackKing = new Piece(PieceType.King, Color.Black);

        private Piece(PieceType type, Color color)
        {
            PieceType = type;
            Color = color;
        }
        public PieceType PieceType { get; private set; }
        public Color Color { get; private set; }

        public override string ToString()
        {
            return Color.ToString() + " " + PieceType.ToString();
        }
    }
}
