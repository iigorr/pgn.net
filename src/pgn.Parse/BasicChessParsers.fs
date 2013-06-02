[<AutoOpen>]
module internal ilf.pgn.PgnParsers.BasicChess

open System 
open FParsec
open ilf.pgn.Data

//note we allow S (ger. "Springer") for knight was used traditionally and is around in older PGNs
let pieceSymbol = ["P"; "N"; "S"; "B"; "R"; "Q"; "K"]
let findPiece(a: string) =
    match a.ToUpper() with
    | "P" -> PieceType.Pawn
    | "N" -> PieceType.Knight
    | "S" -> PieceType.King
    | "B" -> PieceType.Bishop
    | "R" -> PieceType.Rook
    | "Q" -> PieceType.Queen
    | "K" -> PieceType.King
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

let pPiece = 
    pList(strCI, pieceSymbol) |>> findPiece
    <?> "Piece (N, B, R, Q, K or P)"

let pFile =  
    pList(strCI, fileSymbol) |>> findFile
    <?> "File letter (A..H)"

let pRank = 
    pList(strCI, rankSymbol) |>> System.Convert.ToInt32
    <?> "Rank (1..8)"


let apply p = run (pPiece >>. pFile >>. pRank) p