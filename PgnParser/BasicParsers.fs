module ilf.pgn.PgnParsers.Basic

open FParsec

let ws = spaces
let str = pstring
let strCI = pstringCI
let pNotChar c = manySatisfy (fun x -> x <> c)

let pList(p, list:'a list) = list |> List.map p |> choice

let BP (p: Parser<_,_>) stream =
    p stream // set a breakpoint here

let NBP (p: Parser<_,_>, name:string) stream =
    p stream // set a breakpoint here

let D (p: Parser<_,_>, name:string) stream =
    System.Console.WriteLine(name);
    p stream