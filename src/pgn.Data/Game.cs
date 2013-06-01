using System.Collections.Generic;
using System.ComponentModel;
using ilf.pgn.Data.Format;

namespace ilf.pgn.Data
{
    public class Game
    {
        public Game()
        {
            AdditionalInfo = new List<GameInfo>();
            MoveText = new List<MoveTextEntry>();
        }
        public string Event { get; set; }
        public string Site { get; set; }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }

        public string Round { get; set; }

        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }

        [DefaultValue(GameResult.Open)]
        public GameResult Result { get; set; }

        public List<GameInfo> AdditionalInfo { get; set; }
        public List<MoveTextEntry> MoveText { get; set; }

        public IEnumerable<Move> GetMoves()
        {
            foreach (var entry in MoveText)
            {
                if (entry.Type == MoveTextEntryType.MovePair)
                {
                    yield return ((MovePairEntry)entry).White;
                    yield return ((MovePairEntry)entry).Black;
                }
                else if (entry.Type == MoveTextEntryType.SingleMove)
                {
                    yield return ((SingleMoveEntry)entry).Move;
                }
            }
        }

        public override string ToString()
        {
            return new Formatter().Format(this);
        }
    }
}
