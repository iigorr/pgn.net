namespace ilf.pgn.Data

type File = 
    | A = 1 
    | B = 2 
    | C = 3 
    | D = 4 
    | E = 5 
    | F = 6 
    | G = 7 
    | H = 8

[<AllowNullLiteral>]
type Square(file: File, rank: int) = 
    member val File = file with get
    member val Rank = rank with get

    override x.Equals(yobj) =
        match yobj with
        | :? Square as y -> (x.File = y.File) && (x.Rank = y.Rank)
        | _ -> false

    override x.GetHashCode() = hash x.File * x.Rank

type Piece = | King = 1 | Queen = 2 | Rook = 3 | Bishop = 4 | Knight = 5 | Pawn = 6

type GameResult =
    | White =  1
    | Black = 2
    | Draw = 3
    | Open = 4

