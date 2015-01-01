#Requires -Version 3.0

Set-StrictMode -Version Latest

$nuget = "$PSScriptRoot\tools\NuGet.exe"
$stagingDir = "$PSScriptRoot\work\staging"

if (!(Test-Path $nuget)) {
  Write-Host 'This script requires a local copy of the NuGet executable: .\tools\NuGet\NuGet.exe!' -BackgroundColor Red -ForegroundColor Yellow
  Exit 1
}

Write-Host "Publishing packages..." -ForegroundColor 'Yellow'

#& "$nuget" push "$stagingDir\*.nupkg"
