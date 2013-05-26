module internal ilf.pgn.PgnParsers.Basic

open FParsec
open NLog

type DebugMode =
    | File
    | Console
    | Off

type Debug() = 
    do Debug.ConfigureLog()
    let fileLog : Logger = LogManager.GetLogger("file")
    let consoleLog : Logger = LogManager.GetLogger("console")

    static member private ConfigureLog() =
        let config = Config.LoggingConfiguration()

        let fileTarget = new Targets.FileTarget()
        fileTarget.FileName <- Layouts.SimpleLayout("${basedir}/debug.log")
        fileTarget.Layout <- Layouts.LayoutWithHeaderAndFooter.FromString("${message}")
        fileTarget.DeleteOldFileOnStartup <- true
        let fileTargetRule = new Config.LoggingRule("*", LogLevel.Debug, fileTarget)
        config.AddTarget("file", fileTarget)
        config.LoggingRules.Add(fileTargetRule)

        let debuggerTarget = new Targets.ConsoleTarget()
        debuggerTarget.Layout <- Layouts.LayoutWithHeaderAndFooter.FromString("${message}")
        let debuggerTargetRule = new Config.LoggingRule("*", LogLevel.Debug, debuggerTarget)
        config.AddTarget("console", debuggerTarget)
        config.LoggingRules.Add(debuggerTargetRule)

        LogManager.Configuration <- config
        LogManager.Configuration.Reload()
        |> ignore

    member val DebugMode : DebugMode = DebugMode.Off with get, set
    member val ParserLvl : int = 4 with get, set
    member this.Log (message: string) (parserLevel : int) = 
        if parserLevel >= this.ParserLvl then
            match this.DebugMode with
            | File -> fileLog.Log(LogLevel.Debug, message)
            | Console -> consoleLog.Log(LogLevel.Debug, message)
            | _ -> ()

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
    fun stream -> p stream
//        let startTime = System.DateTime.Now
//        deb.Log (sprintf "%A: %sEntering %s. \"%s\""  stream.Position ("->".PadLeft(2*depth)) label (stream.PeekString(5))) depth
//        let reply = p stream
//        let duration = System.DateTime.Now - startTime
//        deb.Log (sprintf "%A: %sLeaving %s (%A) (%f)"  stream.Position ("->".PadLeft(2*depth)) label reply.Status duration.TotalMilliseconds) depth
//        reply



let (<!>) (p: Parser<_,_>) label : Parser<_,_> =
        p <!!> (label, 0) 