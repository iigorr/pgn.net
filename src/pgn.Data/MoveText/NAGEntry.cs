namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents a NAG (numeric annotation glyph) entry in the move text.
    /// </summary>
    public class NAGEntry : MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the NAG code.
        /// </summary>
        /// <value>
        /// The NAG code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// Initializes a <see cref="NAGEntry"/>.
        /// </summary>
        /// <param name="code">The NAG code.</param>
        public NAGEntry(int code)
            : base(MoveTextEntryType.NumericAnnotationGlyph)
        {
            Code = code;
        }
        /// <summary>
        /// The NAG representation in the move text.
        /// </summary>
        /// <returns>
        /// The NAG representation in the move text.
        /// </returns>
        public override string ToString()
        {
            return "$" + Code;
        }
    }
}