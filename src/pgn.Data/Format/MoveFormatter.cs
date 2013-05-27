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

        private string GetPiece(Piece? piece)
        {
            if (piece == null || piece == Piece.Pawn)
                return string.Empty;

            return ((char)piece).ToString();
        }


        private string GetMoveOrigin(Move move)
        {
            var piece = GetPiece(move.Piece);

            if (move.OriginSquare != null)
                return piece + move.OriginSquare;

            var originSquare = "";
            if (move.OriginFile != null)
                originSquare = move.OriginFile.ToString().ToLower();

            if (move.OriginRank != null)
                originSquare += move.OriginRank;

            return piece + originSquare;
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
