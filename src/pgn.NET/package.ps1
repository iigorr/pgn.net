$ErrorActionPreference = "Stop"

$targets = @("net45", "net40")
$packageDir = ".\package\"

function Merge($target) {
  .\merge.ps1 -Target $target
  if ($LastExitCode -ne 0) {
    write-error "Failed to merge $target"
  }

  if (!$(Test-Path $packageDir\lib\$target\)) {
    mkdir $packageDir\lib\$target\ | out-null
  }
  cp pgn.net.dll,pgn.net.xml,pgn.net.pdb $packageDir\\lib\$target\
}

foreach ($target in $targets) {
  echo "Merging target $target"
  Merge($target)
}

Push-Location package
nuget pack -Verbosity detailed
Pop-Location