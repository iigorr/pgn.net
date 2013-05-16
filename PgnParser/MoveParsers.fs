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
    | _ -> raise <| System.ArgumentException "Invalid piece character"

let fileSymbol = [ 'a' .. 'h'] |> List.map (fun x -> x.ToString())

let myFunc a = 
    match a with 
    | "A" -> 1
    | _ -> 0

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
    | _ -> raise <| System.ArgumentException "Invalid file letter"

let rankSymbol = [1 .. 8] |> List.map (fun x -> x.ToString())

let pPiece = pList(strCI, pieceSymbol) |>> findPiece
let pFile =  pList(strCI, fileSymbol) |>> findFile
let pRank = pList(strCI, rankSymbol) |>> System.Convert.ToInt32

type MoveInfo(piece, file, rank) =
    member val Piece : Piece option = piece with get, set
    member val File  : File option = file with get, set
    member val Rank  : int option = rank with get, set

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

// parsers for capturing
let pCapturingSign = (pchar 'x' <|> pchar ':') |>> fun x -> x.ToString()

let pSuffixCaptureMove = // e.g. Qf4x or Qf4:
    pTarget .>> pCapturingSign  
    |>> fun moveInfo -> new Move()

let pInfixCaptureMove =   // e.g. QxBc5
    pOrigin .>> pCapturingSign .>>. pTarget
    |>> fun moveInfo -> new Move() 

let pSimplifiedPawnCapture =  // e.g. dxe or de
    pFile .>> (pCapturingSign <|>% "") .>>. pFile 
    |>> fun (file1, file2) -> 
        new Move (
//            Type = MoveType.Capture,
//            Piece = Some(Piece.Pawn),
//            TargetPiece = Some(Piece.Pawn),
//            SourceFile = Some(file1),
//            TargetFile = Some(file2)
         ) 

let pBasicCapturingMove = 
        attempt pSimplifiedPawnCapture
    <|> attempt pSuffixCaptureMove 
    <|> attempt pInfixCaptureMove


// the two most common move types: move and capture
let pCapturingMove = pBasicCapturingMove .>> opt (strCI "e.p.") <!> "pCapturingMove"
let pBasicMove = 
    attempt (pOrigin .>>. pTarget) |>> fun (origin, target) -> new Move()
    <|> (pTarget |>> fun target -> new Move())
    <!> "pBasicMove"

// special moves: pawn promotion and castle (king-side, queen-side)
let pPawnPromotion = 
    opt (strCI "P") .>> pFile .>> str "8" >>. pPiece 
    |>> fun piece -> new Move()//Type = MoveType.PawnPromotion, PromotedPiece = Some(piece))
    <!> "pPawnPromotion"

let pCasteKingSide = 
    str "O-O" <|> str "O - O" <|> str "0-0"  <|> str "0 - 0" 
    |>> fun _ -> new Move()//Type=MoveType.CastleKingSide) 
    <!> "pCastleKingSide"

let pCasteQueenSide = 
    str "O-O-O" <|> str "O - O - O" <|> str "0-0-0"  <|> str "0 - 0 - 0" 
    |>> fun _ -> new Move()//Type=MoveType.CastleQueenSide) 
    <!> "pCasteQueenSide"

let pCastle = pCasteQueenSide <|> pCasteKingSide

// indicators
let pCheckIndicator = str "+" <|> str "†" <|> str "ch" <|> str "++" <|> str "††" <|> str "dbl ch"
let pCheckMateIndicator = str "#" <|> str "‡"

let pMove = 
    attempt pBasicMove <|> 
    attempt pCapturingMove <|>
    attempt pPawnPromotion <|>
    pCastle
    .>> opt (pCheckIndicator <|> pCheckMateIndicator)
    <!> "pMove"

let appyPMove (p: string)= run pMove p
