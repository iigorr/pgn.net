using ilf.pgn.Data.Format;

namespace ilf.pgn.Data
{
    /// <summary>
    /// A move (actually Half-Move or Ply). Holds all information about the move and does not check for inconsistency, completeness, contradictions...
    /// </summary>
    public class Move
    {
        public MoveType Type { get; set; }
        public PieceType? TargetPiece { get; set; }
        public Square TargetSquare { get; set; }
        public File? TargetFile { get; set; }
        public PieceType? Piece { get; set; }
        public Square OriginSquare { get; set; }
        public File? OriginFile { get; set; }
        public int? OriginRank { get; set; }
        public PieceType? PromotedPiece { get; set; }
        public bool? IsCheck { get; set; }
        public bool? IsDoubleCheck { get; set; }
        public bool? IsCheckMate { get; set; }
        public MoveAnnotation? Annotation { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as Move;
            if (other == null) return false;
            if (this == obj) return true;

            return
                this.Type == other.Type
                && this.TargetPiece == other.TargetPiece
                && this.TargetSquare == other.TargetSquare
                && this.Piece == other.Piece
                && this.OriginSquare == other.OriginSquare
                && this.OriginFile == other.OriginFile
                && this.OriginRank == other.OriginRank
                && this.PromotedPiece == other.PromotedPiece
                && this.IsCheck == other.IsCheck
                && this.IsCheckMate == other.IsCheckMate
                && this.Annotation == other.Annotation;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + getNullableHashCode(TargetSquare);
                hash = hash * 23 + getNullableHashCode(Piece);
                hash = hash * 23 + getNullableHashCode(OriginSquare);
                hash = hash * 23 + getNullableHashCode(OriginFile);
                hash = hash * 23 + getNullableHashCode(OriginRank);
                hash = hash * 23 + getNullableHashCode(PromotedPiece);
                hash = hash * 23 + getNullableHashCode(IsCheck);
                hash = hash * 23 + getNullableHashCode(IsCheckMate);
                hash = hash * 23 + getNullableHashCode(Annotation);
                return hash;
            }
        }

        /// <summary>
        /// Gets the default hash code for an object or 1 if object is null.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private int getNullableHashCode(object obj)
        {
            return obj == null ? 1 : obj.GetHashCode();
        }

        public override string ToString()
        {
            return MoveFormatter.Default.Format(this);
        }
    }
}
