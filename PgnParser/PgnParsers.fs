module ilf.pgn.PgnParsers

open FParsec

let ws = spaces
let str = pstring
let pNotChar c = manySatisfy (fun x -> x <> c)

let pTagName =
    ["Event"; "Site"; "Date"; "Round"; "White"; "Black"; "Result"]
    |> Seq.map pstring 
    |> choice


let tagValue = pchar '"' >>. (pNotChar '"') .>> pchar '"'
let tagContent= pTagName .>>. (spaces >>. tagValue);

let pTag = spaces >>. pchar '[' >>. spaces >>. tagContent .>> spaces .>> pchar ']' .>> spaces

let appyPTag (p: string)= run pTag 