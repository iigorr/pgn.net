namespace ilf.pgn.Data
{
    /// <summary>
    /// The game result. Note that a game may be unfinished, which is represented by <c>GameResult.Open</c>.
    /// </summary>
    public enum GameResult
    {
        White,
        Black,
        Draw,
        Open
    }
}
