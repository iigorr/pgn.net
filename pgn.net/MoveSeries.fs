namespace ilf.pgn

open ilf.pgn

type MoveTextEntryType = 
    | MovePair
    | SingleMove
    | GameEnd
    | Comment

type MoveTextEntry()=
    member val Type : MoveTextEntryType = MoveTextEntryType.MovePair with get, set

type MovePairEntry(white : Move, black : Move)=
    inherit MoveTextEntry(Type = MoveTextEntryType.MovePair)

    member val White :Move = white with get, set
    member val Black :Move = black with get, set

type SingleMoveEntry(move : Move)=
    inherit MoveTextEntry(Type = MoveTextEntryType.SingleMove)

    member val Move :Move = move with get, set

type CommentEntry(comment : string)=
    inherit MoveTextEntry(Type = MoveTextEntryType.Comment)

    member val Comment : string = comment with get, set


type GameEndEntry(result : GameResult)=
    inherit MoveTextEntry(Type = MoveTextEntryType.GameEnd)

    member val Result : GameResult = result with get, set