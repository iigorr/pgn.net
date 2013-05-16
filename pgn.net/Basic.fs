namespace ilf.pgn.Basic

type File = | A | B | C | D | E | F | G | H


type Square(file: File, rank: int) = 
    member val File = file with get
    member val Rank = rank with get

    override x.Equals(yobj) =
        match yobj with
        | :? Square as y -> (x.File = y.File) && (x.Rank = y.Rank)
        | _ -> false

    override x.GetHashCode() = hash x.File * x.Rank


type Piece = | King | Queen | Rook | Bishop | Knight | Pawn