using System.Collections.Generic;
using System.Linq;

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
        public int HalfMoveCount
        {
            get
            {
                int count = 0;
                foreach (var entry in this)
                {
                    if (entry is MovePairEntry)
                        count += 2;
                    else if (entry is HalfMoveEntry)
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
            get { return HalfMoveCount / 2; }
        }

    }
}
