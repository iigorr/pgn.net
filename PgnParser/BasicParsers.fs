module ilf.pgn.PgnParsers.Basic

open FParsec

let ws = spaces
let ws1 = spaces1
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

let (<!>) (p: Parser<_,_>) label : Parser<_,_> =
    fun stream ->
        printfn "%A: Entering %s" stream.Position label
        let reply = p stream
        printfn "%A: Leaving %s (%A)" stream.Position label reply.Status
        reply