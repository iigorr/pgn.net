using System.Collections.Generic;

namespace ilf.pgn
{
    public class Database
    {
        public List<Game> Games { get; private set; }

        public Database()
        {
            Games = new List<Game>();
        }
    }
}