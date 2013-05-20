namespace ilf.pgn

open ilf.pgn

type MoveTextEntryType = 
    | MovePair
    | SplitMoveWhite
    | SplitMoveBlack
    | GameEndDraw
    | GameEndWhite
    | GameEndBlack
    | GameEndOpen
    | Comment

type MoveTextEntry()=
    member val Type : MoveTextEntryType = MoveTextEntryType.MovePair with get, set
    member val MoveNumber : int option = None with get, set
    member val White :Move option = None with get, set
    member val Black :Move option = None with get, set
    member val Comment : string option = None with get, set

