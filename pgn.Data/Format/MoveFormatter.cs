using System;
using System.Globalization;
using System.IO;

namespace ilf.pgn.Data.Format
{
    /// <summary>
    /// A special formatter moves in PGN notation.
    /// </summary>
    public class MoveFormatter
    {
        /// <summary>
        /// The default writer.
        /// </summary>
        public readonly static MoveFormatter Default = new MoveFormatter();

        /// <summary>
        /// Formats the specified move and writes it to the writer.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="writer">The writer.</param>
        /// <exception cref="System.ArgumentException">Thrown on unsupported move types.</exception>
        public void Format(Move move, TextWriter writer)
        {
            var handled =
                HandleCastle(move, writer) ||
                HandleSimpleMove(move, writer) ||
                HandleCapturingMove(move, writer);

            if (!handled) throw new ArgumentException(String.Format("Unsupported MoveType {0}", move.Type));

            if (move.PromotedPiece != null)
            {
                writer.Write("=");
                writer.Write(GetPiece(move.PromotedPiece));
            }

            writer.Write(GetCheckAndMateAnnotation(move));
            writer.Write(GetAnnotation(move));
        }

        /// <summary>
        /// Formats the specified move.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <returns>The PGN representation of the move as string.</returns>
        public string Format(Move move)
        {
            var writer = new StringWriter();
            Format(move, writer);
            return writer.ToString();
        }

        /// <summary>
        /// Checks whether the specified move is a capturing move and formats it if so.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="writer">The writer.</param>
        /// <returns><c>true</c> if the move is a capturing move; otherwise <c>false</c></returns>
        private bool HandleCapturingMove(Move move, TextWriter writer)
        {
            if (move.Type != MoveType.Capture && move.Type != MoveType.CaptureEnPassant) return false;

            var origin = GetMoveOrigin(move);
            var target = GetMoveTarget(move);

            if (origin == String.Empty)
            {
                writer.Write(target);
                writer.Write("x");
            }
            else
            {
                writer.Write(origin);
                writer.Write("x");
                writer.Write(target);
            }
            if (move.Type == MoveType.CaptureEnPassant)
                writer.Write("e.p.");

            return true;
        }

        /// <summary>
        /// Checks whether the specified move is a simple move and formats it if so.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="writer">The writer.</param>
        /// <returns><c>true</c> if the move is a simple move; otherwise <c>false.</c></returns>
        private bool HandleSimpleMove(Move move, TextWriter writer)
        {
            if (move.Type != MoveType.Simple) return false;

            var origin = GetMoveOrigin(move);
            var target = GetMoveTarget(move);

            writer.Write(origin);
            writer.Write(target);

            return true;
        }

        /// <summary>
        /// Checks whether the move is a castling move and formats it if so.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="writer">The writer.</param>
        /// <returns></returns>
        private bool HandleCastle(Move move, TextWriter writer)
        {
            switch (move.Type)
            {
                case MoveType.CastleKingSide:
                    writer.Write("O-O");
                    return true;

                case MoveType.CastleQueenSide:
                    writer.Write("O-O-O");
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the string representation of the target. e.g. "Ne5" in "QxNe5",  "e4" in "e4", or "N5" in "QxN5"
        /// </summary>
        /// <param name="move">The move.</param>
        /// <returns>The string representation of the target.</returns>
        private string GetMoveTarget(Move move)
        {
            var piece = "";

            if (move.Type != MoveType.Simple) // do not render target piece on a simple move
                piece = GetPiece(move.TargetPiece);

            var target = "";
            if (move.TargetSquare != null)
                target = piece + move.TargetSquare;
            else if (move.TargetFile != null)
                target = piece + move.TargetFile.ToString().ToLower();

            return target;
        }

        /// <summary>
        /// Gets the move origin (piece + starting square info) if specified. E.g. "Q" in "QxNe5" or "Qg2" in "Qg2xNe5", or even "" in "e4"
        /// </summary>
        /// <param name="move">The move.</param>
        /// <returns>The origin (piece + starting square info) if specified; otherwise an empty string</returns>
        private string GetMoveOrigin(Move move)
        {
            var piece = GetPiece(move.Piece);

            if (move.OriginSquare != null)
                return piece + move.OriginSquare;

            var origin = "";
            if (move.OriginFile != null)
                origin = move.OriginFile.ToString().ToLower();

            if (move.OriginRank != null)
                origin += move.OriginRank;

            return piece + origin;
        }

        /// <summary>
        /// Gets the string representation of the specified piece if specified or an empty string.
        /// </summary>
        /// <param name="pieceType">Type of the piece or <c>null</c>.</param>
        /// <returns>The string representation of the specified piece if specified or an empty string.</returns>
        private string GetPiece(PieceType? pieceType)
        {
            if (pieceType == null || pieceType == PieceType.Pawn)
                return string.Empty;

            return ((char)pieceType).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the check and mate annotation.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <returns>The check/checkmate anntoation symbols or an empty string if no chec/checkmate move.</returns>
        private string GetCheckAndMateAnnotation(Move move)
        {
            if (move.IsCheckMate == true) return "#";
            if (move.IsDoubleCheck == true) return "++";
            if (move.IsCheck == true) return "+";

            return "";
        }

        /// <summary>
        /// Gets the move annotation symbol.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <returns>The move annotation symbol.</returns>
        private string GetAnnotation(Move move)
        {
            if (move.Annotation == null) return "";

            switch (move.Annotation.Value)
            {
                case MoveAnnotation.MindBlowing: return "!!!";
                case MoveAnnotation.Brilliant: return "!!";
                case MoveAnnotation.Good: return "!";
                case MoveAnnotation.Interesting: return "!?";
                case MoveAnnotation.Dubious: return "?!";
                case MoveAnnotation.Mistake: return "?";
                case MoveAnnotation.Blunder: return "??";
                case MoveAnnotation.Abysmal: return "???";
                case MoveAnnotation.FascinatingButUnsound: return "!!?";
                case MoveAnnotation.Unclear: return "∞";
                case MoveAnnotation.WithCompensation: return "=/∞";
                case MoveAnnotation.EvenPosition: return "=";
                case MoveAnnotation.SlightAdvantageWhite: return "+/=";
                case MoveAnnotation.SlightAdvantageBlack: return "=/+";
                case MoveAnnotation.AdvantageWhite: return "+/−";
                case MoveAnnotation.AdvantageBlack: return "−/+";
                case MoveAnnotation.DecisiveAdvantageWhite: return "+−";
                case MoveAnnotation.DecisiveAdvantageBlack: return "-+";
                case MoveAnnotation.Space: return "○";
                case MoveAnnotation.Initiative: return "↑";
                case MoveAnnotation.Development: return "↑↑";
                case MoveAnnotation.Counterplay: return "⇄";
                case MoveAnnotation.Countering: return "∇";
                case MoveAnnotation.Idea: return "Δ";
                case MoveAnnotation.TheoreticalNovelty: return "N";
            }
            return "";
        }
    }
}
