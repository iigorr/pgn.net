namespace ilf.pgn.Data
{
    /// <summary>
    /// The game result. Note that a game may be unfinished, which is represented by <c>GameResult.Open</c>.
    /// </summary>
    public enum GameResult
    {
        /// <summary>White wins</summary>
        White,
        /// <summary>Black wins</summary>
        Black,
        /// <summary>Draw</summary>
        Draw,
        /// <summary>Game result open (unfinished game)</summary>
        Open
    }
}
