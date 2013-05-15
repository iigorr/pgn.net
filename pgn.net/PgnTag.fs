namespace ilf.pgn

type PgnTag(name: string, value: string) = 
    member val Name = name with get, set
    member val Value = value with get, set