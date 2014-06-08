param(
  [string]$target
)


$frameworks = @{
  "net40" = "/targetplatform:v4"; 
  "net45" = "/targetplatform:v4"}

if(!$frameworks.ContainsKey($target)) {
  echo "Target platform '$target' not found"
} else {
  $target = $frameworks.get_item($target)
  echo "Merging files for $target"
}

$mergetool="..\..\tools\ILMerge.exe"
$binFolder="bin\Release"

& $mergetool /target:library `
   $binFolder\pgn.NET.dll `
   $binFolder\pgn.Data.dll `
   $binFolder\pgn.Parse.dll `
   $binFolder\FParsec.dll `
   $binFolder\FParsecCS.dll `
   $binFolder\FSharp.Core.dll `
   /xmldocs `
  /out:pgn.net.dll `
  $target
