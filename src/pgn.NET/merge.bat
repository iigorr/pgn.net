set mergetool="..\..\tools\ILMerge.exe"
set binFolder="bin\Debug"
%mergetool% /target:library ^
	 %binFolder%\pgn.Data.dll ^
	 %binFolder%\pgn.NET.dll ^
	 %binFolder%\pgn.Parse.dll ^
	 %binFolder%\FParsec.dll ^
	 %binFolder%\FParsecCS.dll ^
	 %binFolder%\NLog.dll ^
	 /lib:"C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\FSharp\\3.0\\Runtime\\v4.0\\" ^
  /out:pgn.net.dll /v4