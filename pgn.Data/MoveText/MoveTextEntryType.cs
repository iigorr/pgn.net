namespace ilf.pgn.Data
{
    /// <summary>
    /// Move Text entry types.
    /// </summary>
    public enum MoveTextEntryType
    {
        /// <summary>Move Pair (white + black)</summary>
        MovePair,
        /// <summary>Single Move (white or black)</summary>
        SingleMove,
        /// <summary>Move text entry indicating the game end result</summary>
        GameEnd,
        /// <summary>Comment</summary>
        Comment,
        /// <summary>A NAG (Numeric Annotation Glyph)</summary>
        NumericAnnotationGlyph,
        /// <summary>A RAV (Recursive Annotation Variation)</summary>
        RecursiveAnnotationVariation
    }
}
