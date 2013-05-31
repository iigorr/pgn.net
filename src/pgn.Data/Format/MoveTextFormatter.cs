using System.IO;

namespace ilf.pgn.Data.Format
{
    public class MoveTextFormatter
    {
        private readonly MoveFormatter _moveFormatter = new MoveFormatter();

        public void Format(MoveTextEntry entry, StringWriter writer)
        {
            switch (entry.Type)
            {
                case MoveTextEntryType.MovePair:
                    FormatPair((MovePairEntry)entry, writer);
                    return;
                case MoveTextEntryType.SingleMove:
                    FormatSingleMove((SingleMoveEntry)entry, writer);
                    return;
                case MoveTextEntryType.GameEnd:
                case MoveTextEntryType.Comment:
                case MoveTextEntryType.NumericAnnotationGlyph:
                    writer.Write(entry.ToString());
                    return;
            }
        }

        public string Format(MoveTextEntry entry)
        {
            var writer = new StringWriter();
            Format(entry, writer);
            return writer.ToString();
        }

        private void FormatPair(MovePairEntry movePair, StringWriter writer)
        {
            if (movePair.MoveNumber != null)
            {
                writer.Write(movePair.MoveNumber);
                writer.Write(". ");
            }
            _moveFormatter.Format(movePair.White, writer);
            writer.Write(" ");
            _moveFormatter.Format(movePair.Black, writer);
        }

        private void FormatSingleMove(SingleMoveEntry entry, StringWriter writer)
        {
            if (entry.MoveNumber != null)
            {
                writer.Write(entry.MoveNumber);
                writer.Write(entry.IsContinued ? "... " : ". ");
            }

            _moveFormatter.Format(entry.Move, writer);
        }


    }
}
