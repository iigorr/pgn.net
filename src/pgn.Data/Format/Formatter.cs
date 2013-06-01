using System.Globalization;
using System.IO;

namespace ilf.pgn.Data.Format
{
    public class Formatter
    {
        private MoveTextFormatter _moveTextFormatter = new MoveTextFormatter();
        public string Format(Game game)
        {
            var writer = new StringWriter();
            Format(game, writer);
            return writer.ToString();
        }

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


        private void FormatTag(string name, object value, TextWriter writer)
        {
            writer.Write("[");
            writer.Write(name + " \"");
            writer.Write(value ?? "?");
            writer.WriteLine("\"]");
        }
    }
}
