namespace ilf.pgn

open ilf.pgn

open System.Collections.Generic

type Game() = 
    member val Event:string = "" with get, set
    member val Site:string = "" with get, set

    member val Year:int option = None with get, set
    member val Month:int option = None with get, set
    member val Day:int option = None with get, set


    
