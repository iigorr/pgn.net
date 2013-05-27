using System;
using System.Text;

namespace ilf.pgn.Data.Format
{
    public class MoveFormatter
    {
        public readonly static MoveFormatter Default = new MoveFormatter();

        public StringBuilder Format(Move move, StringBuilder sb = null)
        {
            if (sb == null) sb = new StringBuilder();

            var handled =
                HandleCastle(move, sb) ||
                HandleSimpleMove(move, sb) ||
                HandleCapturingMove(move, sb);


            if (!handled) throw new ArgumentException(String.Format("Unsupported MoveType {0}", move.Type));

            if (move.PromotedPiece != null)
                sb.Append("=").Append(GetPiece(move.PromotedPiece));

            sb.Append(GetCheckAndMateAnnotation(move));
            sb.Append(GetAnnotation(move));
            return sb;
        }

        private bool HandleCapturingMove(Move move, StringBuilder sb)
        {
            if (move.Type != MoveType.Capture && move.Type != MoveType.CaptureEnPassant) return false;

            var origin = GetMoveOrigin(move);
            var target = GetMoveTarget(move);

            if (origin == String.Empty)
                sb.Append(target).Append("x");
            else
                sb.Append(origin).Append("x").Append(target);

            if (move.Type == MoveType.CaptureEnPassant)
                sb.Append("e.p.");

            return true;
        }

        private bool HandleSimpleMove(Move move, StringBuilder sb)
        {
            if (move.Type != MoveType.Simple) return false;

            var origin = GetMoveOrigin(move);
            var target = GetMoveTarget(move);

            sb.Append(origin).Append(target);

            return true;
        }

        private string GetMoveTarget(Move move)
        {
            var piece = GetPiece(move.TargetPiece);

            var target = "";
            if (move.TargetSquare != null)
                target = piece + move.TargetSquare;
            else if (move.TargetFile != null)
                target = piece + move.TargetFile.ToString().ToLower();

            return target;
        }
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
        private string GetPiece(Piece? piece)
        {
            if (piece == null || piece == Piece.Pawn)
                return string.Empty;

            return ((char)piece).ToString();
        }

        private string GetCheckAndMateAnnotation(Move move)
        {
            if (move.IsCheckMate == true) return "#";
            if (move.IsDoubleCheck == true) return "++";
            if (move.IsCheck == true) return "+";
            
            return "";
        }

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


        private bool HandleCastle(Move move, StringBuilder sb)
        {
            switch (move.Type)
            {
                case MoveType.CastleKingSide:
                    sb.Append("0-0");
                    return true;

                case MoveType.CastleQueenSide:
                    sb.Append("0-0-0");
                    return true;
            }

            return false;
        }
    }
}
