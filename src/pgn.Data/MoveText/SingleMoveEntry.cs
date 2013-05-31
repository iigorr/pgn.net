namespace ilf.pgn.Data
{
    public class SingleMoveEntry : MoveTextEntry
    {
        public int? MoveNumber { get; set; }

        public Move Move { get; set; }

        public SingleMoveEntry(Move move)
            : base(MoveTextEntryType.SingleMove)
        {
            Move = move;
        }
    }
}