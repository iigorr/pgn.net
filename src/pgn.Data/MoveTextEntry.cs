using System.Collections.Generic;

namespace ilf.pgn.Data
{
    public class MoveTextEntry
    {
        public MoveTextEntryType Type { get; set; }

        public MoveTextEntry(MoveTextEntryType type)
        {
            Type = type;
        }
    }

    public class MovePairEntry : MoveTextEntry
    {
        public Move White { get; set; }
        public Move Black { get; set; }

        public MovePairEntry(Move white, Move black)
            : base(MoveTextEntryType.MovePair)
        {
            White = white;
            Black = black;
        }
    }

    public class SingleMoveEntry : MoveTextEntry
    {
        public Move Move { get; set; }

        public SingleMoveEntry(Move move)
            : base(MoveTextEntryType.SingleMove)
        {
            Move = move;
        }
    }

    public class CommentEntry : MoveTextEntry
    {
        public string Comment { get; set; }

        public CommentEntry(string comment)
            : base(MoveTextEntryType.Comment)
        {
            Comment = comment;
        }
    }

    public class GameEndEntry : MoveTextEntry
    {
        public GameResult Result { get; set; }

        public GameEndEntry(GameResult result)
            : base(MoveTextEntryType.GameEnd)
        {
            Result = result;
        }
    }

    public class NAGEntry : MoveTextEntry
    {
        public int Code { get; set; }

        public NAGEntry(int code)
            : base(MoveTextEntryType.NumericAnnotationGlyph)
        {
            Code = code;
        }
    }

    public class RAVEntry : MoveTextEntry
    {
        public List<MoveTextEntry> MoveText { get; set; }

        public RAVEntry(List<MoveTextEntry> moveText)
            : base(MoveTextEntryType.NumericAnnotationGlyph)
        {
            MoveText = moveText;
        }
    }
}


