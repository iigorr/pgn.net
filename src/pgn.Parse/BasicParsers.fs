[<AutoOpen>]
module internal ilf.pgn.PgnParsers.BasicCommons

open FParsec

let ws = spaces
let ws1 = spaces1
let str = pstring
let strCI = pstringCI
let pNotChar c = manySatisfy (fun x -> x <> c)

let charList2String (lChars: char list)= System.String.Concat(lChars)
let pList(p, list:'a list) = list |> List.map p |> choice

let concat (a: string, b) = a + b
let concat3 ((a: string, b), c) = a + b + c

let BP (p: Parser<_,_>) stream =
    p stream // set a breakpoint here

let NBP (p: Parser<_,_>, name:string) stream =
    p stream // set a breakpoint here
    
let D (p: Parser<_,_>, name:string) stream =
    System.Console.WriteLine(name);
    p stream

#if DEBUG
let deb= Debug.Default
let (<!!>) (p: Parser<_,_>) (label, depth) : Parser<_,_> =
    fun stream ->
        let startTime = System.DateTime.Now

        if(deb.DebugMode = DebugMode.Off) 
            then p stream
        else
            deb.Log (sprintf "%A: %sEntering %s. \"%s\""  stream.Position ("->".PadLeft(2*depth)) label (stream.PeekString(5))) depth
            let reply = p stream
            let duration = System.DateTime.Now - startTime
            deb.Log (sprintf "%A: %sLeaving %s (%A) (%f)"  stream.Position ("->".PadLeft(2*depth)) label reply.Status duration.TotalMilliseconds) depth
            reply
#else
let (<!!>) (p: Parser<_,_>) (label, depth) : Parser<_,_> =
    fun stream ->
        p stream
#endif


let (<!>) (p: Parser<_,_>) label : Parser<_,_> =
        p <!!> (label, 0) 