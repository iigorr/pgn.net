namespace ilf.pgn.Move

open ilf.pgn.Basic

type MoveType = 
    | Simple
    | Capture
    | CaptureEnPassant
    | CastleKingSide
    | CastleQueenSide
    | PawnPromotion

type Move() =
    member val Type = MoveType.Simple with get, set
    member val TargetPiece : Piece option = None with get, set
    member val TargetSquare : Square option = None with get, set
    member val TargetFile : File option = None with get, set
    member val Piece : Piece option = None with get, set
    member val OriginSquare : Square option = None with get, set
    member val OriginFile : File option = None with get, set
    member val OriginRank : int option = None with get, set
    member val PromotedPiece : Piece option = None with get, set
