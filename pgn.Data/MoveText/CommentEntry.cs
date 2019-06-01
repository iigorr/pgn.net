namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents a comment entry in the move text.
    /// </summary>
    public class CommentEntry : MoveTextEntry
    {
        /// <summary>
        /// Gets or sets the comment (without the curly braces).
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Initializes a<see cref="CommentEntry"/>.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public CommentEntry(string comment)
            : base(MoveTextEntryType.Comment)
        {
            Comment = comment;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> that represents the comment entry.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents the comment entry.
        /// </returns>
        public override string ToString()
        {
            return "{" + Comment + "}";
        }
    }
}