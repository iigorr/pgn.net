using System.Collections.Generic;

namespace ilf.pgn.Data
{
    public class RAVEntry : MoveTextEntry
    {
        public List<MoveTextEntry> MoveText { get; set; }

        public RAVEntry(List<MoveTextEntry> moveText)
            : base(MoveTextEntryType.NumericAnnotationGlyph)
        {
            MoveText = moveText;
        }
    }
}