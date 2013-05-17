namespace ilf.pgn

open ilf.pgn

type MoveEntryType = 
    | MovePair
    | SplitMoveWhite
    | SplitMoveBlack
    | GameEndDraw
    | GameEndWhite
    | GameEndBlack
    | GameEndOpen

type MoveEntry()=
    member val Type : MoveEntryType = MoveEntryType.MovePair with get, set
    member val MoveNumber : int option = None with get, set
    member val White :Move option = None with get, set
    member val Black :Move option = None with get, set
    member val Comment : string option = None with get, set

