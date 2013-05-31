using System.IO;

namespace ilf.pgn.Data.Format
{
    public class MoveTextFormatter
    {
        private readonly MoveFormatter _moveFormatter=new MoveFormatter();

        public void Format(MovePairEntry movePair, StringWriter writer)
        {
            if (movePair.MoveNumber != null)
            {
                writer.Write(movePair.MoveNumber);
                writer.Write(". ");
            }
            _moveFormatter.Format(movePair.White, writer);
            writer.Write(" ");
            _moveFormatter.Format(movePair.Black, writer);
        }
        public string Format(MovePairEntry movePair)
        {
            var writer = new StringWriter();
            Format(movePair, writer);
            return writer.ToString();
        }
    }
}
