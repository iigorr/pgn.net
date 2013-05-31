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