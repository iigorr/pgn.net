using System.IO;
using ilf.pgn.Data;
using ilf.pgn.Data.Format;

namespace pgn.NET
{
    public class PgnWriter
    {
        private readonly Stream _stream;
        public PgnWriter(Stream stream)
        {
            _stream = stream;
        }

        public PgnWriter(string fileName)
        {
            _stream=new FileStream(fileName, FileMode.OpenOrCreate);
        }

        public void Write(Database db)
        {
            var formatter = new Formatter();

            TextWriter writer = new StreamWriter(_stream);
            foreach (var game in db.Games)
            {
                formatter.Format(game, writer);
            }
            writer.Close();
        }
    }
}
