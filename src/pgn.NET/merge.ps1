param(
  [string]$target
)


$sldir = "${Env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0"
if (Test-Path $sldir) {
  $sldir = "${Env:ProgramFiles}\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0"
}

$frameworks = @{
  "net40" = "/targetplatform:v4"; 
  "net45" = "/targetplatform:v4"; 
  "wp71" = "/targetplatform:v4,$sldir"}

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
   /lib:"C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\FSharp\\3.0\\Runtime\\v4.0\\" `
   /xmldocs `
  /out:pgn.net.dll `
  $target
