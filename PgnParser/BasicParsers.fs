module ilf.pgn.PgnParsers.Basic

open FParsec
open NLog

type Debug() = 
    do Debug.ConfigureLog()
    let log : Logger = LogManager.GetCurrentClassLogger()

    static member private ConfigureLog() =
        let logTarget = new Targets.FileTarget()
        logTarget.FileName <- Layouts.SimpleLayout("${basedir}/debug.log")
        logTarget.DeleteOldFileOnStartup <- true
        let loggingRule = new Config.LoggingRule("*", LogLevel.Debug, logTarget)

        let config = Config.LoggingConfiguration()
        config.AddTarget("default", logTarget)
        config.LoggingRules.Add(loggingRule)


        LogManager.Configuration <- config
        LogManager.Configuration.Reload()
        |> ignore

    member val DebugMode : bool = false with get, set
    member val ParserLvl : int = 4 with get, set
    member this.Log (message: string) = fun (logLvl : LogLevel) -> log.Log(logLvl, message)

    static member val Default = Debug()

let deb = Debug.Default

let ws = spaces
let ws1 = spaces1
let str = pstring
let strCI = pstringCI
let pNotChar c = manySatisfy (fun x -> x <> c)

let charList2String (lChars: char list)= System.String.Concat(lChars)
let pList(p, list:'a list) = list |> List.map p |> choice

let concat (a: string, b) = a + b
let concat3 ((a: string, b), c) = a + b + c

let BP (p: Parser<_,_>) stream =
    p stream // set a breakpoint here

let NBP (p: Parser<_,_>, name:string) stream =
    p stream // set a breakpoint here
    
let D (p: Parser<_,_>, name:string) stream =
    System.Console.WriteLine(name);
    p stream


let (<!!>) (p: Parser<_,_>) (label, depth) : Parser<_,_> =
    fun stream ->
        if deb.DebugMode && depth >= deb.ParserLvl then deb.Log (sprintf "%A: %sEntering %s. \"%s\""  stream.Position ("->".PadLeft(depth)) label (stream.PeekString(20))) LogLevel.Debug
        let reply = p stream
        if deb.DebugMode && depth >= deb.ParserLvl then deb.Log (sprintf "%A: %sLeaving %s (%A)"  stream.Position ("->".PadLeft(depth)) label reply.Status) LogLevel.Debug
        reply


let (<!>) (p: Parser<_,_>) label : Parser<_,_> =
        p <!!> (label, 0) 