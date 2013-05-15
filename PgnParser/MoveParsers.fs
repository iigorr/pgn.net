module ilf.pgn.PgnParsers.Move

open FParsec
open ilf.pgn.PgnParsers.Basic


//note we allow S (ger. "Springer") for knight was used traditionally and is around in older PGNs
let pieceSymbol = ["P"; "N"; "S"; "B"; "R"; "Q"; "K"]
let fileSymbol = [ 'a' .. 'h'] |> List.map (fun x -> x.ToString())
let rankSymbol = ['1' .. '8'] |> List.map (fun x -> x.ToString())

let pPiece = pList(strCI, pieceSymbol)
let pFile =  pList(strCI, fileSymbol)
let pRank =  pList(strCI, rankSymbol)

//MOVE MECHANICS

// target square of a move
let pTarget = 
    (pPiece <|>% "") //piece is optional in pawn moves
    .>>. pFile 
    .>>. pRank 
    |>> concat3 
    <!> "pTarget"

// origin squalre of move (usually for disambiguation)
let pOrigin = 
        attempt(pPiece .>>. pFile .>>. pRank |>> concat3) //e.g. Qe6xd7
    <|> attempt(pPiece .>>. pFile |>> concat) //e.g. Qexd7
    <|> attempt(pPiece .>>. pRank |>> concat) //e.g. Q4xd7
    <|> attempt(pFile .>>. pRank |>> concat) //e.g. d4e5
    <|> pFile  // e.g. in exd5
    <|> pPiece // e.g. in Qxd7
    <!> "pOrigin"

// parsers for capturing
let pCapturingSign = (pchar 'x' <|> pchar ':') |>> fun x -> x.ToString()
let pSuffixCaptureMove = pTarget .>>. pCapturingSign |>> concat // e.g. Qf4x or Qf4:
let pInfixCaptureMove =  pOrigin .>>. pCapturingSign .>>. pTarget |>>concat3 // e.g. QxBc5
let pSimplifiedPawnCapture = pFile .>>. (pCapturingSign <|>% "") .>>. pFile |>> concat3 // e.g. dxe or de

let pBasicCapturingMove = 
        attempt pSimplifiedPawnCapture
    <|> attempt pSuffixCaptureMove 
    <|> attempt pInfixCaptureMove


// the two most common move types: move and capture
let pCapturingMove = pBasicCapturingMove .>> opt (strCI "e.p.") <!> "pCapturingMove"
let pBasicMove = attempt (pOrigin .>> pTarget) <|> pTarget  <!> "pBasicMove"

// special moves: pawn promotion and castle (king-side, queen-side)
let pPawnPromotion = opt (strCI "P") >>. pFile .>> str "8" .>> pPiece <!> "pPawnPromotion"
let pCastle = str "O-O" <|> str "O-O-O" <|> str "0-0" <|> str "0-0-0" <!> "pCastle"

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
