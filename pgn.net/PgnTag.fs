namespace ilf.pgn


type PgnTag(name: string) = 
    member val Name = name with get, set


type PgnBasicTag(name: string, value: string) =
    inherit PgnTag(name)
    member val Value: string = value with get, set


type PgnDateTag(name: string) = 
    inherit PgnTag(name)
    
    member val Year: int option = None with get, set
    member val Month: int option = None with get, set
    member val Day: int option = None with get, set

type PgnRoundTag(name: string, round: string option) = 
    inherit PgnTag(name)
    
    member val Round: string option = round with get, set

type PgnResultTag(name: string, result: GameResult) = 
    inherit PgnTag(name)
    
    member val Result: GameResult = result with get, set