namespace ilf.pgn.Data
{
    public class SingleMoveEntry : MoveTextEntry
    {
        public Move Move { get; set; }

        public SingleMoveEntry(Move move)
            : base(MoveTextEntryType.SingleMove)
        {
            Move = move;
        }
    }
}