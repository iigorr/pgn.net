using System.IO;
using ilf.pgn.Data;
using ilf.pgn.PgnParsers;

namespace ilf.pgn
{
    public class PgnReader
    {
        public Database ReadFromFile(string file)
        {
            var p = new Parser();
            return p.ReadFromFile(file);
        }

        public Database ReadFromStream(Stream stream)
        {
            var p = new Parser();
            return p.ReadFromStream(stream);
        }

        public Database ReadFromString(string input)
        {
            var p = new Parser();
            return p.ReadFromString(input);
        }
    }
}
