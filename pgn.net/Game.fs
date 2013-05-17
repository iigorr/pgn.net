namespace ilf.pgn

open ilf.pgn

open System.Collections.Generic

type Game() = 
    member val Tags : List<PgnTag> = new List<PgnTag>() with get
    member val Moves : List<MoveEntry> = new List<MoveEntry>() with get
    
