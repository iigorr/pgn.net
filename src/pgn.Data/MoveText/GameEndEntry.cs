namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents the game end entry in the move text. 
    /// </summary>
    public class GameEndEntry : MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the game result.
        /// </summary>
        /// <value>
        /// The game result.
        /// </value>
        public GameResult Result { get; set; }

        /// <summary>
        /// Initializes a <see cref="GameEndEntry"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        public GameEndEntry(GameResult result)
            : base(MoveTextEntryType.GameEnd)
        {
            Result = result;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the game result entry.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents the game result entry.
        /// </returns>
        public override string ToString()
        {
            switch (Result)
            {
                case GameResult.White:
                    return "1-0";
                case GameResult.Black:
                    return "0-1";
                case GameResult.Draw:
                    return "½-½";
            }

            return "*";
        }
    }
}