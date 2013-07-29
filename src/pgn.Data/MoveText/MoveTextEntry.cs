using ilf.pgn.Data.Format;

namespace ilf.pgn.Data
{
    /// <summary>
    /// An abstract move text entry.
    /// </summary>
    public abstract class MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the move entry type.
        /// </summary>
        /// <value>
        /// The move entry type.
        /// </value>
        public MoveTextEntryType Type { get; set; }

        /// <summary>
        /// Initializes a new  <see cref="MoveTextEntry"/>.
        /// </summary>
        /// <param name="type">The move entry type.</param>
        protected MoveTextEntry(MoveTextEntryType type)
        {
            Type = type;
        }

        /// <summary>
        /// Returns a move text representation of this entry.
        /// </summary>
        /// <returns>
        /// A move text representation of this entry.
        /// </returns>
        public override string ToString()
        {
            return new MoveTextFormatter().Format(this);
        }
    }
}


