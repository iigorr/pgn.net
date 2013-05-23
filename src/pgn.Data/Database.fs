namespace ilf.pgn

type Database() =
    let _games= new System.Collections.Generic.List<Game>()

    member this.Games
        with get()= _games