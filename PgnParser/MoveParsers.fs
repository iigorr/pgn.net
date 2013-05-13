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

let pTarget = opt pPiece >>. pFile .>> pRank

let pOrigin = 
    pFile  // e.g. in exd5
    <|> attempt(pPiece >>. pFile >>. pRank) //e.g. Qe6xd7
    <|> attempt(pPiece >>. pFile) //e.g. Qexd7
    <|> attempt(pPiece >>. pRank) //e.g. Q4xd7
    <|> attempt(pPiece) // e.g. in Qxd7

let pCapturingSign = (pchar 'x' <|> pchar ':')
let pSuffixCaptureMove = pTarget .>> pCapturingSign // e.g. Qf4x or Qf4:
let pInfixCaptureMove =  pOrigin .>> pCapturingSign >>. pTarget // e.g. QxBc5
let pSimplifiedPawnCapture = pFile .>> opt pCapturingSign >>. pFile // e.g. dxe or de

let pBasicCapturingMove = 
        attempt pSimplifiedPawnCapture
    <|> attempt pSuffixCaptureMove 
    <|> attempt pInfixCaptureMove

let pCapturingMove = pBasicCapturingMove .>> optional (strCI "e.p.")

let pBasicMove = attempt pTarget <|> (pOrigin .>> pTarget)

let appyPCapturingMove (p: string)= run pCapturingMove p