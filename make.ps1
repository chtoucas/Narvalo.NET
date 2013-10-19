param($task = "default")

$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath

get-module psake | remove-module

.\tools\NuGet.exe install .nuget\packages.config -OutputDirectory packages
import-module (Get-ChildItem "$scriptDir\packages\psake.*\tools\psake.psm1" | Select-Object -First 1)

exec { invoke-psake "$scriptDir\scripts\make.ps1" $task }