#Requires -Version 3.0

Set-StrictMode -Version Latest

$stagingDir = "$PSScriptRoot\work\staging"

# Download nuget if needed.
$nuget = "$PSScriptRoot\tools\NuGet.exe";
if (!(Test-Path $nuget)) {
    Write-Host -NoNewline 'Downloading NuGet.exe...'
    Invoke-WebRequest "https://nuget.org/nuget.exe" -OutFile $nuget
}

Write-Host "Publishing packages..." -ForegroundColor 'Yellow'

#& "$nuget" push "$stagingDir\*.nupkg"
