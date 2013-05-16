namespace ilf.pgn.Basic

type File = | A | B | C | D | E | F | G | H

type Square(file: File, rank: int) = 
    member val File = file with get
    member val Rank = rank with get

type Piece = | King | Queen | Rook | Bishop | Knight | Pawn