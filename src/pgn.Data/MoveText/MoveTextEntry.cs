using ilf.pgn.Data.Format;

namespace ilf.pgn.Data
{
    public class MoveTextEntry
    {
        public MoveTextEntryType Type { get; set; }

        public MoveTextEntry(MoveTextEntryType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return new MoveTextFormatter().Format(this);
        }
    }
}


