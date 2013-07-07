namespace ilf.pgn.Data
{
    /// <summary>
    /// Represents additional game information (usually a tag).
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public GameInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0} \"{1}\"]", Name, Value);
        }
    }

}
