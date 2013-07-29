namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents a half-move (or ply). E.g.  "17. Ne5" (white moves knight e5) or "17... Qxe5" (black queen takes e5)
    /// </summary>
    public class HalfMoveEntry : MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the move number.
        /// </summary>
        /// <value>
        /// The move number.
        /// </value>
        public int? MoveNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the move is continued (black) or not (white).
        /// </summary>
        /// <value>
        /// <c>true</c> if the move is continued (black); otherwise, <c>false</c>.
        /// </value>
        public bool IsContinued { get; set; }

        /// <summary>
        /// Gets or sets the move.
        /// </summary>
        /// <value>
        /// The move.
        /// </value>
        public Move Move { get; set; }

        /// <summary>
        /// Constructs the half move. By default the move is for white (not continued).
        /// </summary>
        /// <param name="move">The move.</param>
        public HalfMoveEntry(Move move)
            : base(MoveTextEntryType.SingleMove)
        {
            Move = move;
        }
    }
}