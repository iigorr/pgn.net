
namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents a full move (white and black). 
    /// </summary>
    public class MovePairEntry : MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the move number.
        /// </summary>
        /// <value>
        /// The move number.
        /// </value>
        public int? MoveNumber { get; set; }

        /// <summary>
        /// Gets or sets white's move.
        /// </summary>
        /// <value>
        /// The move.
        /// </value>
        public Move White { get; set; }

        /// <summary>
        /// Gets or sets blacks's move.
        /// </summary>
        /// <value>
        /// The move.
        /// </value>
        public Move Black { get; set; }

        /// <summary>
        /// Initializes the <see cref="MovePairEntry"/>.
        /// </summary>
        /// <param name="white">The white.</param>
        /// <param name="black">The black.</param>
        public MovePairEntry(Move white, Move black)
            : base(MoveTextEntryType.MovePair)
        {
            White = white;
            Black = black;
        }
    }
}