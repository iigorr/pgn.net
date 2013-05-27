namespace ilf.pgn.Data
{
    public class NAGEntry : MoveTextEntry
    {
        public int Code { get; set; }

        public NAGEntry(int code)
            : base(MoveTextEntryType.NumericAnnotationGlyph)
        {
            Code = code;
        }
    }
}