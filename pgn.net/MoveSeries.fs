namespace ilf.pgn.MoveSeries

open ilf.pgn.Basic
open ilf.pgn.Move

type MoveEntry()=
    member val MoveNumber : int option = None with get, set
    member val White :Move option = None with get, set
    member val Black :Move option = None with get, set
    member val Comment : string option = None with get, set

