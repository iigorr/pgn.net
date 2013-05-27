namespace ilf.pgn.Data
{
    public class GameEndEntry : MoveTextEntry
    {
        public GameResult Result { get; set; }

        public GameEndEntry(GameResult result)
            : base(MoveTextEntryType.GameEnd)
        {
            Result = result;
        }
    }
}