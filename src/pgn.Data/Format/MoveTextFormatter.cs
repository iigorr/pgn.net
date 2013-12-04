using System.Collections.Generic;
using System.IO;

namespace ilf.pgn.Data.Format
{
    /// <summary>
    /// A special formatter for move text in PGN notation
    /// </summary>
    class MoveTextFormatter
    {
        /// <summary>
        /// The move formatter
        /// </summary>
        private readonly MoveFormatter _moveFormatter = new MoveFormatter();

        /// <summary>
        /// Formats the specified move text entry and writes it to the text writer.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="writer">The writer.</param>
        public void Format(MoveTextEntry entry, TextWriter writer)
        {
            switch (entry.Type)
            {
                case MoveTextEntryType.MovePair:
                    FormatPair((MovePairEntry)entry, writer);
                    return;
                case MoveTextEntryType.SingleMove:
                    FormatHalfMove((HalfMoveEntry)entry, writer);
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

        /// <summary>
        /// Formats the specified entry and returns it as string.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The specified entry as string.</returns>
        public string Format(MoveTextEntry entry)
        {
            var writer = new StringWriter();
            Format(entry, writer);
            return writer.ToString();
        }

        /// <summary>
        /// Formats the specified move text (list of move text entries).
        /// </summary>
        /// <param name="moveText">The move text.</param>
        /// <param name="writer">The writer.</param>
        public void Format(List<MoveTextEntry> moveText, TextWriter writer)
        {
            if (moveText.Count == 0)
                return;

            //no foreach here as last one is special case (no trailing space)
            for (int i = 0; i < moveText.Count - 1; ++i)
            {
                Format(moveText[i], writer);
                writer.Write(" ");
            }
            Format(moveText[moveText.Count - 1], writer);
        }

        /// <summary>
        /// Formats the specified move text and returns it as string.
        /// </summary>
        /// <param name="moveText">The move text.</param>
        /// <returns>The specified move text as string.</returns>
        public string Format(List<MoveTextEntry> moveText)
        {
            var writer = new StringWriter();
            Format(moveText, writer);
            return writer.ToString();
        }


        /// <summary>
        /// Formats a full move (move pair).
        /// </summary>
        /// <param name="movePair">The move pair.</param>
        /// <param name="writer">The writer.</param>
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

        /// <summary>
        /// Formats a half move (Ply).
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="writer">The writer.</param>
        private void FormatHalfMove(HalfMoveEntry entry, TextWriter writer)
        {
            if (entry.MoveNumber != null)
            {
                writer.Write(entry.MoveNumber);
                writer.Write(entry.IsContinued ? "... " : ". ");
            }

            _moveFormatter.Format(entry.Move, writer);
        }

        /// <summary>
        /// Formats the a RAV (Recursive Annotation Variations) entry.
        /// </summary>
        /// <param name="entry">The RAV entry.</param>
        /// <param name="writer">The writer.</param>
        private void FormatRAVEntry(RAVEntry entry, TextWriter writer)
        {
            writer.Write("(");
            this.Format(entry.MoveText, writer);
            writer.Write(")");
        }

    }
}
