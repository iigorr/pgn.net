namespace ilf.pgn.Data
{
    /// <summary>
    /// A representation of a chess board square.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Square"/>.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="rank">The rank.</param>
        public Square(File file, int rank)
        {
            File = file;
            Rank = rank;
        }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public File File { get; set; }
        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        public int Rank { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as Square;
            if (other == null) return false;

            return
                this.File == other.File &&
                this.Rank == other.Rank;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for the square (product of rank and file value). 
        /// </returns>
        public override int GetHashCode()
        {
            return ((int) this.File)*this.Rank;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the Square.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents the Square.
        /// </returns>
        public override string ToString()
        {
            return this.File.ToString().ToLower() + this.Rank;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left hand side</param>
        /// <param name="b">Right hand side</param>
        /// <returns><c>true</c> if a equals b; otherwise <c>false</c>.</returns>
        public static bool operator ==(Square a, Square b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Ineqaulity operator.
        /// </summary>
        /// <param name="a">Left hand side</param>
        /// <param name="b">Right hand side</param>
        /// <returns><c>false</c> if a equals b; otherwise <c>true</c>.</returns>
        public static bool operator !=(Square a, Square b)
        {
            return !(a == b);
        }
    }
}

