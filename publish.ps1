#Requires -Version 3.0

Set-StrictMode -Version Latest

$nuget = "$PSScriptRoot\tools\NuGet\NuGet.exe"
$stagingDir = "$PSScriptRoot\work\staging"

if (!(Test-Path $nuget)) {
  Write-Error "This script requires a local copy of the NuGet executable: .\tools\NuGet\NuGet.exe!"
  Exit
}

Write-Host "Publishing packages..." -ForegroundColor 'Yellow'

& "$nuget" push "$stagingDir\*.nupkg"
