using System.Collections.Generic;

namespace ilf.pgn.Data
{
    public class Database
    {
        public Database()
        {
            Games = new List<Game>();
        }

        public List<Game> Games { get; private set; }
    }
}
