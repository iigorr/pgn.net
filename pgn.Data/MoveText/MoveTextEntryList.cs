using System.Collections.Generic;

namespace ilf.pgn.Data.MoveText
{
    /// <summary>
    /// List of MoveTextEntries which also provides some helpful
    /// properties like MoveCount.
    /// </summary>
    public class MoveTextEntryList : List<MoveTextEntry>
    {
        /// <summary>
        /// Gets the number of half-moves (moves by black *or* white)
        /// </summary>
        public int MoveCount
        {
            get
            {
                int count = 0;
                foreach (var entry in this)
                {
                    if (entry.Type == MoveTextEntryType.MovePair)
                        count += 2;
                    else if (entry.Type == MoveTextEntryType.SingleMove)
                        count += 1;
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the number of full moves
        /// </summary>
        public int FullMoveCount
        {
            get { return MoveCount / 2; }
        }

        /// <summary>
        /// Gets the moves from the move text.
        /// </summary>
        /// <returns>Enumerations of the moves.</returns>
        public IEnumerable<Move> GetMoves()
        {
            foreach (var entry in this)
            {
                if (entry.Type == MoveTextEntryType.MovePair)
                {
                    yield return ((MovePairEntry)entry).White;
                    yield return ((MovePairEntry)entry).Black;
                }
                else if (entry.Type == MoveTextEntryType.SingleMove)
                {
                    yield return ((HalfMoveEntry)entry).Move;
                }
            }
        }
    }
}
