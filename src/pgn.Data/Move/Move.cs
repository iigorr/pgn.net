using ilf.pgn.Data.Format;

namespace ilf.pgn.Data
{
    /// <summary>
    /// A move (actually Half-Move or Ply). Holds all information about the move and does not check for inconsistency, completeness, contradictions...
    /// NOTE: The same move may be expressed in different ways: Qd5xe6 or Qd5xKe6 or QxKe6 etc...
    /// NOTE: The move class does not check for correctness of moves. Illegal moves may be constructed here.
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Gets or sets the move type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public MoveType Type { get; set; }
        /// <summary>
        /// Gets or sets the target piece, i.e. the captured piece.
        /// </summary>
        /// <value>
        /// The target piece.
        /// </value>
        public PieceType? TargetPiece { get; set; }
        /// <summary>
        /// Gets or sets the target square.
        /// </summary>
        /// <value>
        /// The target square.
        /// </value>
        public Square TargetSquare { get; set; }
        /// <summary>
        /// Gets or sets the target file.
        /// </summary>
        /// <value>
        /// The target file.
        /// </value>
        public File? TargetFile { get; set; }
        /// <summary>
        /// Gets or sets the moved piece.
        /// </summary>
        /// <value>
        /// The piece.
        /// </value>
        public PieceType? Piece { get; set; }
        /// <summary>
        /// Gets or sets the origin square.
        /// </summary>
        /// <value>
        /// The origin square.
        /// </value>
        public Square OriginSquare { get; set; }
        /// <summary>
        /// Gets or sets the origin file.
        /// </summary>
        /// <value>
        /// The origin file.
        /// </value>
        public File? OriginFile { get; set; }
        /// <summary>
        /// Gets or sets the origin rank.
        /// </summary>
        /// <value>
        /// The origin rank.
        /// </value>
        public int? OriginRank { get; set; }
        /// <summary>
        /// Gets or sets the promoted piece in case of a pawn reaching the other side.
        /// </summary>
        /// <value>
        /// The promoted piece.
        /// </value>
        public PieceType? PromotedPiece { get; set; }
        /// <summary>
        /// Indicates whether the move results in check.
        /// </summary>
        /// <value>
        /// <c>true</c> if the move results in check; <c>false</c> or <c>null</c> otherwise.
        /// </value>
        public bool? IsCheck { get; set; }
        /// <summary>
        /// Indicates whether the move results in double check.
        /// </summary>
        /// <value>
        /// <c>true</c> if the move results in double check; <c>false</c> or <c>null</c> otherwise.
        /// </value>
        public bool? IsDoubleCheck { get; set; }

        /// <summary>
        /// Indicates whether the move results in checkmate.
        /// </summary>
        /// <value>
        /// <c>true</c> if the move results in checkmate; <c>false</c> or <c>null</c> otherwise.
        /// </value>
        public bool? IsCheckMate { get; set; }
        /// <summary>
        /// Gets or sets the move annotation.
        /// </summary>
        /// <value>
        /// The annotation.
        /// </value>
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return MoveFormatter.Default.Format(this);
        }
    }
}
