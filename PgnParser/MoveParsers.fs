module ilf.pgn.PgnParsers.Move

open FParsec
open ilf.pgn.Basic
open ilf.pgn.Move
open ilf.pgn.PgnParsers.Basic


//note we allow S (ger. "Springer") for knight was used traditionally and is around in older PGNs
let pieceSymbol = ["P"; "N"; "S"; "B"; "R"; "Q"; "K"]
let findPiece(a: string) =
    match a.ToUpper() with
    | "P" -> Piece.Pawn
    | "N" -> Piece.Knight
    | "S" -> Piece.King
    | "B" -> Piece.Bishop
    | "R" -> Piece.Rook
    | "Q" -> Piece.Queen
    | "K" -> Piece.King
    | _ -> raise <| System.ArgumentException("Invalid piece character " + a)

let fileSymbol = [ 'a' .. 'h'] |> List.map (fun x -> x.ToString())

let findFile (a: string) =
    match a.ToUpper() with
    |"A" -> File.A
    |"B" -> File.B
    |"C" -> File.C
    |"D" -> File.D
    |"E" -> File.E
    |"F" -> File.F
    |"G" -> File.G
    |"H" -> File.H
    | _ -> raise <| System.ArgumentException ("Invalid file letter " + a)

let rankSymbol = [1 .. 8] |> List.map (fun x -> x.ToString())

let pPiece = pList(strCI, pieceSymbol) |>> findPiece
let pFile =  pList(strCI, fileSymbol) |>> findFile
let pRank = pList(strCI, rankSymbol) |>> System.Convert.ToInt32

type MoveInfo(piece, file, rank) =
    member val Piece : Piece option = piece with get, set
    member val File  : File option = file with get, set
    member val Rank  : int option = rank with get, set

let getSquare(moveInfo : MoveInfo) =
    match moveInfo.File, moveInfo.Rank with
    | Some(x), Some(y) -> Some(Square(x, y))
    | _, _ -> None

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
    opt pPiece //piece is optional in pawn moves
    .>>. pFile 
    .>>. pRank
    |>> fun ((piece, file), rank) ->  MoveInfo(piece, Some(file), Some(rank))
    <!> "pTarget"

// origin squalre of move (usually for disambiguation)
let pOrigin = 
    opt pPiece .>>. opt pFile .>>. opt pRank 
    |>> fun ((piece, file), rank) ->  MoveInfo(piece, file, rank)
    <!> "pOrigin"

let pBasicMove = 
    attempt (pOrigin .>>. pTarget) |>> fun (origin, target) -> getMove(Some(origin), target, MoveType.Simple)
    <|> (pTarget |>> fun target -> getMove(None, target, MoveType.Simple))
    <!> "pBasicMove"

// parsers for capturing
let pCapturingSign = (pchar 'x' <|> pchar ':') |>> fun x -> x.ToString()

let pInfixCaptureMove =   // e.g. QxBc5
    pOrigin .>> pCapturingSign .>>. pTarget
    |>> fun (orig, target) -> getMove(Some orig, target, MoveType.Capture)
    <!> "pInfixCaptureMove"

let pSimplifiedPawnCapture =  // e.g. dxe or de
    pFile .>> (pCapturingSign <|>% "") .>>. pFile 
    |>> fun (file1, file2) -> 
        new Move (
            Type = MoveType.Capture,
            Piece = Some(Piece.Pawn),
            TargetPiece = Some(Piece.Pawn),
            OriginFile = Some(file1),
            TargetFile = Some(file2)
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
    <!> "pCapturingMove"


// special moves: pawn promotion and castle (king-side, queen-side)
// TODO: this parser allows to much, e.g. Qxd5(R). 
//       It should be asserted, that the moved piece is a pawn.
//       If rank is set, then only rank 8 is allowed
let pPawnPromotion = 
    (attempt  pBasicCapturingMove <|> pBasicMove)
    .>>. ((str "=" >>. pPiece) <|> (str "(" >>. pPiece .>> str ")"))
    |>> fun (move, piece) -> move.PromotedPiece <- Some piece; move
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
let pCheckIndicator = str "+" <|> str "†" <|> str "ch" <|> str "++" <|> str "††" <|> str "dbl ch"
let pCheckMateIndicator = str "#" <|> str "‡"

let pMove = 
    attempt pPawnPromotion <|>
    attempt pCapturingMove <|>
    attempt pBasicMove <|> 
    pCastle
    .>> opt (pCheckIndicator <|> pCheckMateIndicator)
    <!> "pMove"

let appyPMove (p: string)= run pMove p
