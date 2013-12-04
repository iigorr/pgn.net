[<AutoOpen>]
module internal ilf.pgn.PgnParsers.Bootstrap

open System 

#if DEBUG
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

#endif

let toNullable =
    function
    | None -> Nullable()
    | Some x -> Nullable(x)