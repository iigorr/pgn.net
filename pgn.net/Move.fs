namespace ilf.pgn.Move

open ilf.pgn.Basic

type MoveType = 
    | Simple
    | Capture
    | CaptureEnPassant
    | CastleKingSide
    | CastleQueenSide

type MoveAnnotation =
    | MindBlowing
    | Brilliant
    | Good
    | Interesting
    | Dubious
    | Mistake
    | Blunder
    | Abysmal
    | FascinatingButUnsound
    | Unclear
    | WithCompensation
    | EvenPosition
    | SlightAdvantageWhite
    | SlightAdvantageBlack
    | AdvantageWhite
    | AdvantageBlack
    | DecisiveAdvantageWhite
    | DecisiveAdvantageBlack
    | Space
    | Initiative
    | Development
    | Counterplay
    | Countering
    | Idea
    | TheoreticalNovelty
    | UnknownAnnotation


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
    member val IsCheck : bool option = None with get, set
    member val IsDoubleCheck : bool option = None with get, set
    member val IsCheckMate : bool option = None with get, set
    member val Annotation : MoveAnnotation option = None with get, set

    override x.Equals(yobj) =
        match yobj with
        | :? Move as y -> x.EqualsMove(y)
        | :? Option<Move> as y -> 
            match y with
            | None -> false
            | Some(move) -> x.EqualsMove(move)
        | _ -> false

    member x.EqualsMove(y : Move) = 
            (x.Type = y.Type) 
            && (x.TargetPiece = y.TargetPiece)
            && (x.TargetSquare = y.TargetSquare)
            && (x.Piece = y.Piece)
            && (x.OriginSquare = y.OriginSquare)
            && (x.OriginFile = y.OriginFile)
            && (x.OriginRank = y.OriginRank)
            && (x.PromotedPiece = y.PromotedPiece)
            && (x.IsCheck = y.IsCheck)
            && (x.IsCheckMate = y.IsCheckMate)
            && (x.Annotation = y.Annotation)