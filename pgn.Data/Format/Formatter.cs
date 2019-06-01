using System.Globalization;
using System.IO;

namespace ilf.pgn.Data.Format
{
    /// <summary>
    /// Formatter for Chess games in PGN format.
    /// </summary>
    public class Formatter
    {
        private readonly MoveTextFormatter _moveTextFormatter = new MoveTextFormatter();
        /// <summary>
        /// Formats the specified game as PGN string.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns>The PGN string.</returns>
        public string Format(Game game)
        {
            var writer = new StringWriter();
            writer.NewLine = "\n";
            Format(game, writer);
            return writer.ToString();
        }

        /// <summary>
        /// Formats the specified game and writes it to the writer.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="writer">The writer.</param>
        public void Format(Game game, TextWriter writer)
        {
            FormatTag("Event", game.Event, writer);
            FormatTag("Site", game.Site, writer);
            FormatDate(game, writer);
            FormatTag("Round", game.Round, writer);
            FormatTag("White", game.WhitePlayer, writer);
            FormatTag("Black", game.BlackPlayer, writer);
            FormatTag("Result", GetResultString(game.Result), writer);

            foreach(var info in game.AdditionalInfo)
                FormatTag(info.Name, info.Value, writer);

            writer.WriteLine();
            _moveTextFormatter.Format(game.MoveText, writer);
        }

        /// <summary>
        /// Formats the date tag of the game and writes it to the string. e.g [Date "2013.??.??].
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="writer">The writer.</param>
        private void FormatDate(Game game, TextWriter writer)
        {
            writer.Write("[Date \"");
            writer.Write(game.Year == null ? "????" : game.Year.Value.ToString(CultureInfo.InvariantCulture));
            writer.Write(".");
            writer.Write(game.Month == null ? "??" : game.Month.Value.ToString(CultureInfo.InvariantCulture));
            writer.Write(".");
            writer.Write(game.Day == null ? "??" : game.Day.Value.ToString(CultureInfo.InvariantCulture));
            writer.WriteLine("\"]");
        }

        /// <summary>
        /// Gets the game result representation and writes it using the writer.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The game result representation.</returns>
        private string GetResultString(GameResult result)
        {
            switch (result)
            {
                case GameResult.White: return "1-0";
                case GameResult.Black: return "0-1";
                case GameResult.Draw: return "½-½";
            }
            return "*";
        }


        /// <summary>
        /// Formats a pgn tag tag.
        /// </summary>
        /// <param name="name">The tag name.</param>
        /// <param name="value">The tag value.</param>
        /// <param name="writer">The writer.</param>
        private void FormatTag(string name, object value, TextWriter writer)
        {
            writer.Write("[");
            writer.Write(name + " \"");
            writer.Write(value ?? "?");
            writer.WriteLine("\"]");
        }
    }
}
