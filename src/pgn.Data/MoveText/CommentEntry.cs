namespace ilf.pgn.Data
{
    public class CommentEntry : MoveTextEntry
    {
        public string Comment { get; set; }

        public CommentEntry(string comment)
            : base(MoveTextEntryType.Comment)
        {
            Comment = comment;
        }
    }
}