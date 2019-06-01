using System.IO;
using ilf.pgn.Data;
using ilf.pgn.Data.Format;

namespace ilf.pgn
{
    /// <summary>
    /// Pgn format writer.
    /// </summary>
    public class PgnWriter
    {
        private readonly Stream _stream;
        /// <summary>
        /// Initializes a new instance of the <see cref="PgnWriter"/>.
        /// </summary>
        /// <param name="stream">A stream to write to.</param>
        public PgnWriter(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PgnWriter"/>.
        /// </summary>
        /// <param name="fileName">Name of the file to write to.</param>
        public PgnWriter(string fileName)
        {
            _stream=new FileStream(fileName, FileMode.OpenOrCreate);
        }

        /// <summary>
        /// Writes the specified pgn database.
        /// </summary>
        /// <param name="pgnDatabase">The pgn database.</param>
        public void Write(Database pgnDatabase)
        {
            var formatter = new Formatter();

            TextWriter writer = new StreamWriter(_stream);
            writer.NewLine = "\n";
            foreach (var game in pgnDatabase.Games)
            {
                formatter.Format(game, writer);
            }
            writer.Close();
        }
    }
}
