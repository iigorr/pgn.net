[<AutoOpen>]
module ilf.pgn.PgnParsers.Move

open System
open FParsec
open ilf.pgn.Data

type MoveInfo(piece, file, rank) =
    member val Piece : Nullable<PieceType> = toNullable(piece) with get, set
    member val File  : Nullable<File> = toNullable(file) with get, set
    member val Rank  : Nullable<int> = toNullable(rank) with get, set

let getSquare(moveInfo : MoveInfo) =
    match moveInfo.File, moveInfo.Rank with
    | x, y when x.HasValue && y.HasValue -> Square(x.Value, y.Value)
    | _, _ -> null

let getMove(originInfo: MoveInfo option, targetInfo: MoveInfo, moveType: MoveType) =
    match originInfo, targetInfo with
    | None, _ -> Move (
                    Type = moveType,
                    Piece = targetInfo.Piece,
                    TargetPiece = targetInfo.Piece,
                    TargetSquare =getSquare targetInfo,
                    TargetFile = targetInfo.File
                )
    | Some(orig), _ -> Move (
                        Type = moveType,
                        Piece = orig.Piece,
                        OriginSquare = getSquare orig,
                        OriginFile = orig.File,
                        OriginRank = orig.Rank,
                        TargetPiece = targetInfo.Piece,
                        TargetSquare =getSquare targetInfo,
                        TargetFile = targetInfo.File
                     )

//MOVE MECHANICS

// target square of a move
let pTarget =
    attempt(pPiece .>>. pFile .>>. pRank) // Qd5
    <|> (pFile .>>. pRank |>> fun (f, r) -> ((PieceType.Pawn, f),r)) //Pawn move, e.g. d5
    |>> fun ((piece, file), rank) ->  MoveInfo(Some(piece), Some(file), Some(rank))
    <!> "pTarget"

// origin squalre of move (usually for disambiguation)
let pOrigin =
    attempt(pPiece .>>. opt pFile .>>. opt pRank)
    <|> (pFile .>>. opt pRank |>> fun(f, r) -> ((PieceType.Pawn , Some f),r))
    |>> fun ((piece, file), rank) ->  MoveInfo(Some piece, file, rank)
    <!> "pOrigin"

let pBasicMove =
    attempt (pOrigin .>>. pTarget) |>> fun (origin, target) -> getMove(Some(origin), target, MoveType.Simple)
    <|> (pTarget |>> fun target -> getMove(None, target, MoveType.Simple))
    <!!> ("pBasicMove", 1)

// parsers for capturing
let pCapturingSign = (pchar 'x' <|> pchar ':') |>> fun x -> x.ToString()

let pInfixCaptureMove =   // e.g. QxBc5
    pOrigin .>> pCapturingSign .>>. pTarget
    |>> fun (orig, target) -> getMove(Some orig, target, MoveType.Capture)
    <!> "pInfixCaptureMove"

let pSimplifiedPawnCapture =  // e.g. dxe or de
    pFile .>> pCapturingSign .>>. pFile
    >>= fun(f1, f2) ->
        match f1 = f2 with //do not allow a6xa7
        | true -> pzero
        | false -> preturn (f1, f2)
    |>> fun (file1, file2) ->
        new Move (
            Type = MoveType.Capture,
            Piece = Nullable(PieceType.Pawn),
            OriginFile = Nullable(file1),
            TargetFile = Nullable(file2)
    )
    <!> "pSimplifiedPawnCapture"

let pSuffixCaptureMove = // e.g. Qf4d4x or Qf4:
    pBasicMove .>> pCapturingSign
    |>> fun move -> move.Type <- MoveType.Capture; move
    <!> "pSuffixCaptureMove"

let pBasicCapturingMove =
    attempt (attempt pInfixCaptureMove <|> pSuffixCaptureMove)
    <|> pSimplifiedPawnCapture
    <!> "pBasicCapturingMove"


// the two most common move types: move and capture
let pCapturingMove =
    pBasicCapturingMove .>>. opt (strCI "e.p." ) //TODO: e.p. should only be allowed for pawns
    |>> fun (move, enpassant) ->
            match enpassant with
            | None -> move
            | _ -> move.Type <- MoveType.CaptureEnPassant; move
    <!!> ("pCapturingMove", 1)


// special moves: pawn promotion and castle (king-side, queen-side)
// TODO: this parser allows to much, e.g. Qxd5(R).
//       It should be asserted, that the moved piece is a pawn.
//       If rank is set, then only rank 8 is allowed
let pPawnPromotion =
    (attempt  pBasicCapturingMove <|> pBasicMove)
    .>>. ((str "=" >>. pPiece) <|> (str "(" >>. pPiece .>> str ")"))
    |>> fun (move, piece) -> move.PromotedPiece <- Nullable(piece); move
    <!> "pPawnPromotion"

let pCasteKingSide =
    str "O-O" <|> str "O - O" <|> str "0-0"  <|> str "0 - 0"
    |>> fun _ -> new Move(Type=MoveType.CastleKingSide)
    <!> "pCastleKingSide"

let pCasteQueenSide =
    str "O-O-O" <|> str "O - O - O" <|> str "0-0-0"  <|> str "0 - 0 - 0"
    |>> fun _ -> new Move(Type=MoveType.CastleQueenSide)
    <!> "pCasteQueenSide"

let pCastle = pCasteQueenSide <|> pCasteKingSide

// indicators
let pCheckIndicator = str "++" <|> str "††" <|> str "dbl ch" <|> str "+" <|> str "†" <|> str "ch" <!> "pCheckIndicator"
let pCheckMateIndicator = str "#" <|> str "‡" <!> "pCheckMateIndicator"
let pIndicator = pCheckIndicator <|> pCheckMateIndicator
let pAnnotation =
    str "????" <|> str "???"
    <|> str "!!!!" <|> str "!!!" <|> str "?!?" <|> str "!?!" <|> str "??" <|> str "?!"
    <|> str "!!" <|> str "!?" <|> str "?" <|> str "!"
    <|> str "=/∞" <|> str "=/+" <|> str "="
    <|> str "+/=" <|> str "+/-" <|> str "+-" <|> str "-/+" <|> str "-+"
    <|> str "∞" <|> str "○" <|> str "↑↑" <|> str "↑" <|> str "⇄" <|> str "∇" <|> str "Δ"
    <|> str "TN" <|> str "N"
    |>> fun annotation ->
            match annotation with
            | "!!!" | "!!!!" -> MoveAnnotation.MindBlowing
            | "!!" -> MoveAnnotation.Brilliant
            | "!" -> MoveAnnotation.Good
            | "!?" -> MoveAnnotation.Interesting
            | "?!" -> MoveAnnotation.Dubious
            | "?" -> MoveAnnotation.Mistake
            | "??" -> MoveAnnotation.Blunder
            | "???" | "????" -> MoveAnnotation.Abysmal
            | "!?!" | "?!?" -> MoveAnnotation.FascinatingButUnsound
            | "∞" -> MoveAnnotation.Unclear
            | "=/∞" -> MoveAnnotation.WithCompensation
            | "=" -> MoveAnnotation.EvenPosition
            | "+/=" -> MoveAnnotation.SlightAdvantageWhite
            | "=/+" -> MoveAnnotation.SlightAdvantageBlack
            | "+/-" -> MoveAnnotation.AdvantageWhite
            | "-/+" -> MoveAnnotation.AdvantageBlack
            | "+-" -> MoveAnnotation.DecisiveAdvantageWhite
            | "-+" -> MoveAnnotation.DecisiveAdvantageBlack
            | "○" -> MoveAnnotation.Space
            | "↑" -> MoveAnnotation.Initiative
            | "↑↑" -> MoveAnnotation.Development
            | "⇄" -> MoveAnnotation.Counterplay
            | "∇" -> MoveAnnotation.Countering
            | "Δ" -> MoveAnnotation.Idea
            | "TN" | "N" -> MoveAnnotation.TheoreticalNovelty
            | _ -> MoveAnnotation.UnknownAnnotation
    <!> "pAnnotation"
    <?> "Move annotation (e.g. ! or ??)"

let pAdditionalInfo =
    (attempt(pIndicator .>>. pAnnotation) |>> fun (i, a) -> Some(i), Some(a))
    <|> (attempt(pAnnotation) |>> fun (a) -> None, Some(a))
    <|> (pIndicator|>> fun (i) -> Some(i), None)

let pMove =
    attempt pPawnPromotion <|>
    attempt pCapturingMove <|>
    attempt pBasicMove <|>
    pCastle
    .>>. opt(pAdditionalInfo)
    |>> fun (move, addInfo) ->
            let indicator, annotation =
                match addInfo with
                | None -> None, None
                | Some(x) -> x
            move.Annotation<- toNullable(annotation)
            match indicator with
            | None -> move
            | Some(i) ->
                match i with
                | "+"  | "†"  | "ch" -> move.IsCheck <- Nullable(true); move
                | "++" | "††" | "dbl ch" -> move.IsCheck <- Nullable(true); move.IsDoubleCheck <- Nullable(true); move
                | "#"  | "‡" -> move.IsCheckMate <- Nullable(true); move
                | _ -> move


    <!!> ("pMove", 2)
    <?> "Move (e.g. Qc4 or e2e4 or 0-0-0 etc.)"

let appyPMove (p: string)= run pMove p
