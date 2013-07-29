using System.Collections.Generic;

namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents a RAV (recursive annotated variations) entry in the move text.
    /// </summary>
    public class RAVEntry : MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the inner move text of the RAV.
        /// </summary>
        /// <value>
        /// The move text.
        /// </value>
        public List<MoveTextEntry> MoveText { get; set; }

        /// <summary>
        /// Initializes a <see cref="RAVEntry"/>.
        /// </summary>
        /// <param name="moveText">The inner move text of the RAV.</param>
        public RAVEntry(List<MoveTextEntry> moveText)
            : base(MoveTextEntryType.RecursiveAnnotationVariation)
        {
            MoveText = moveText;
        }
    }
}