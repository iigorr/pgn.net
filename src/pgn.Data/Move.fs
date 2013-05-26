namespace ilf.pgn.Data

open System
open ilf.pgn

type MoveType = 
    | Simple = 1
    | Capture = 2
    | CaptureEnPassant = 3
    | CastleKingSide = 4
    | CastleQueenSide = 5

type MoveAnnotation =
    | MindBlowing = 1
    | Brilliant = 2
    | Good = 3
    | Interesting = 4
    | Dubious = 5
    | Mistake = 6
    | Blunder = 7 
    | Abysmal = 8
    | FascinatingButUnsound = 9
    | Unclear = 10
    | WithCompensation = 11
    | EvenPosition = 12
    | SlightAdvantageWhite = 13
    | SlightAdvantageBlack = 14
    | AdvantageWhite = 15
    | AdvantageBlack = 16
    | DecisiveAdvantageWhite = 17 
    | DecisiveAdvantageBlack = 18
    | Space = 19
    | Initiative = 20
    | Development = 21
    | Counterplay = 22
    | Countering = 23
    | Idea = 24
    | TheoreticalNovelty = 25
    | UnknownAnnotation = 26


type Move() =
    member val Type = MoveType.Simple with get, set
    member val TargetPiece : Nullable<Piece> = Nullable() with get, set
    member val TargetSquare : Square = null with get, set
    member val TargetFile : Nullable<File> = Nullable() with get, set
    member val Piece : Nullable<Piece> = Nullable() with get, set
    member val OriginSquare : Square = null with get, set
    member val OriginFile : Nullable<File> = Nullable() with get, set
    member val OriginRank : Nullable<int> = Nullable() with get, set
    member val PromotedPiece : Nullable<Piece> = Nullable() with get, set
    member val IsCheck : Nullable<bool> = Nullable() with get, set
    member val IsDoubleCheck : Nullable<bool> = Nullable() with get, set
    member val IsCheckMate : Nullable<bool> = Nullable() with get, set
    member val Annotation : Nullable<MoveAnnotation> = Nullable() with get, set

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