using System.Collections.Generic;
using System.IO;

namespace ilf.pgn.Data.Format
{
    class MoveTextFormatter
    {
        private readonly MoveFormatter _moveFormatter = new MoveFormatter();

        public void Format(MoveTextEntry entry, TextWriter writer)
        {
            switch (entry.Type)
            {
                case MoveTextEntryType.MovePair:
                    FormatPair((MovePairEntry)entry, writer);
                    return;
                case MoveTextEntryType.SingleMove:
                    FormatSingleMove((HalfMoveEntry)entry, writer);
                    return;
                case MoveTextEntryType.RecursiveAnnotationVariation:
                    FormatRAVEntry((RAVEntry)entry, writer);
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

        public void Format(List<MoveTextEntry> moveText, TextWriter writer)
        {
            //no foreach here as last one is special case (no trailing space)
            for (int i = 0; i < moveText.Count - 1; ++i)
            {
                Format(moveText[i], writer);
                writer.Write(" ");
            }
            Format(moveText[moveText.Count - 1], writer);
        }

        public string Format(List<MoveTextEntry> moveText)
        {
            var writer = new StringWriter();
            Format(moveText, writer);
            return writer.ToString();
        }


        private void FormatPair(MovePairEntry movePair, TextWriter writer)
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

        private void FormatSingleMove(HalfMoveEntry entry, TextWriter writer)
        {
            if (entry.MoveNumber != null)
            {
                writer.Write(entry.MoveNumber);
                writer.Write(entry.IsContinued ? "... " : ". ");
            }

            _moveFormatter.Format(entry.Move, writer);
        }

        private void FormatRAVEntry(RAVEntry entry, TextWriter writer)
        {
            writer.Write("(");
            this.Format(entry.MoveText, writer);
            writer.Write(")");
        }

    }
}
