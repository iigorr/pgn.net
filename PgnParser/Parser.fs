namespace ilf.pgn


type Parser() =
    member this.ReadFromFile(file:string) =  
        let streamReader = new System.IO.StreamReader(file)
        streamReader.Close()
        new Database();


    member this.ReadFromStream(stream: System.IO.Stream) =
        new Database();