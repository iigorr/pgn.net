[<AutoOpen>]
module internal ilf.pgn.PgnParsers.FenTagParser

open FParsec
open ilf.pgn.Data
open System.Linq

let pFenPieces = 
        (pchar 'p' >>% [Piece.BlackPawn])
    <|> (pchar 'n' >>% [Piece.BlackKnight])
    <|> (pchar 'b' >>% [Piece.BlackBishop])
    <|> (pchar 'r' >>% [Piece.BlackRook])
    <|> (pchar 'q' >>% [Piece.BlackQueen])
    <|> (pchar 'k' >>% [Piece.BlackKing])
    <|> (pchar 'P' >>% [Piece.WhitePawn])
    <|> (pchar 'N' >>% [Piece.WhiteKnight])
    <|> (pchar 'B' >>% [Piece.WhiteBishop])
    <|> (pchar 'R' >>% [Piece.WhiteRook])
    <|> (pchar 'Q' >>% [Piece.WhiteQueen])
    <|> (pchar 'K' >>% [Piece.WhiteKing])
    <|> (pint32 |>> fun n -> Enumerable.Repeat(null, n) |> List.ofSeq)

let check8elem (msg: string) (row: 'a list) : Parser<_, _> =
    fun stream ->
        match row.Length with 
        | 8 -> Reply(row) 
        | _  -> Reply(Error, messageError(msg))

let pFenRow = 
    many pFenPieces |>> fun lists -> List.concat lists
    >>= check8elem "Invalid fen row lenght. Rows must be of length 8"


let checkBoardLenght (row: 'a list) : Parser<_, _> =
    fun stream ->
        match row.Length with 
        | 8 -> Reply(row) 
        | _  -> Reply(Error, messageError(sprintf "Invalid fen row lenght (%d). Rows must be of length 8"  row.Length ))

let pPiecePositions =
    sepEndBy1 pFenRow (pchar '/') >>= check8elem "Invalid fen row count. There must be 8 rows."
    |>> fun lists -> List.concat lists
    
let pFenCastlingInfo =
    attempt(pchar '-' >>% [ false; false; false; false] <!> "noCastling")
    <|> (
        (attempt(pchar 'K' >>% true <!> "king side white") <|> preturn false) .>>.
        (attempt(pchar 'Q' >>% true) <|> preturn false) .>>.
        (attempt(pchar 'k' >>% true) <|> preturn false) .>>.
        (attempt(pchar 'q' >>% true) <|> preturn false)
        |>> fun(((K, Q), k), q) -> [K; Q; k; q]
    )

let pFenEnPassantSquare = 
    attempt(pchar '-' >>% null)
    <|> (pFile .>>. pRank |>> fun (f, r) -> Square(f, r))

let pFenTagValue = 
    pchar '"' >>. pPiecePositions .>> ws
    .>>. ((pchar 'w' >>% true) <|> (pchar 'b' >>% false)) .>> ws
    .>>. pFenCastlingInfo .>> ws
    .>>. pFenEnPassantSquare .>> ws
    .>>. pint32 .>> ws
    .>>. pint32 .>> ws 
    .>> pchar '"'
    |>> fun (((((pieces, whiteMove), castlingInfo), enPassantSquare), halfMoves), fullMoves) -> 
            let boardSetup= new BoardSetup();
            for i in 0 .. 63 do
                boardSetup.[i] <- pieces.[i]
            
            boardSetup.IsWhiteMove <- whiteMove
            
            boardSetup.CanWhiteCastleKingSide <- castlingInfo.[0]
            boardSetup.CanWhiteCastleQueenSide <- castlingInfo.[1]
            boardSetup.CanBlackCastleKingSide <- castlingInfo.[2]
            boardSetup.CanBlackCastleQueenSide <- castlingInfo.[3]

            boardSetup.EnPassantSquare <- enPassantSquare

            boardSetup.HalfMoveClock <- halfMoves

            boardSetup.FullMoveCount <- fullMoves

            FenTag("FEN", boardSetup) :> PgnTag;

    <!> "pFenTagValue"