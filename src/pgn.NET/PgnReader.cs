using System.IO;
using ilf.pgn.Data;
using ilf.pgn.PgnParsers;

namespace ilf.pgn
{
    /// <summary>
    /// Pgn format reader.
    /// </summary>
    public class PgnReader
    {
        /// <summary>
        /// Creates a PgnReader. Not much more really. 
        /// </summary>
        public PgnReader() { }

        /// <summary>
        /// Reads a pgn database from a fileName.
        /// </summary>
        /// <param name="fileName">The fileName name.</param>
        /// <returns>A pgn database.</returns>
        public Database ReadFromFile(string fileName)
        {
            var p = new Parser();
            return p.ReadFromFile(fileName);
        }

        /// <summary>
        /// Reads a pgn database from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A pgn database.</returns>
        public Database ReadFromStream(Stream stream)
        {
            var p = new Parser();
            return p.ReadFromStream(stream);
        }

        /// <summary>
        /// Reads a pgn database from a string.
        /// </summary>
        /// <param name="input">The string input.</param>
        /// <returns>A pgn database.</returns>
        public Database ReadFromString(string input)
        {
            var p = new Parser();
            return p.ReadFromString(input);
        }
    }
}
