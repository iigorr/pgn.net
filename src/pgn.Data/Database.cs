using System.Collections.Generic;

namespace ilf.pgn.Data
{
    /// <summary>
    /// Pgn Database. Basically a collection of games.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        public Database()
        {
            Games = new List<Game>();
        }

        /// <summary>
        /// Gets the games.
        /// </summary>
        /// <value>
        /// The games.
        /// </value>
        public List<Game> Games { get; private set; }
    }
}
