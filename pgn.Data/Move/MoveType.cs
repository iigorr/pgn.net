namespace ilf.pgn.Data
{
    /// <summary>
    /// Move types.
    /// </summary>
    public enum MoveType
    {
        /// <summary>Simple Move</summary>
        Simple,
        /// <summary>Capturing Move (not en passant)</summary>
        Capture,
        /// <summary>En Passant</summary>
        CaptureEnPassant,
        /// <summary>Castle King Side</summary>
        CastleKingSide,
        /// <summary>Castle Queen Side</summary>
        CastleQueenSide,
    }
}
