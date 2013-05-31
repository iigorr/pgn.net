
namespace ilf.pgn.Data
{
    public class MovePairEntry : MoveTextEntry
    {
        public int? MoveNumber { get; set; }
        public Move White { get; set; }
        public Move Black { get; set; }

        public MovePairEntry(Move white, Move black)
            : base(MoveTextEntryType.MovePair)
        {
            White = white;
            Black = black;
        }
    }
}