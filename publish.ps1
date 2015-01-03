#Requires -Version 3.0

# .SYNOPSIS
# Publish NuGet packages.
#
# .PARAMETER Retail
# If present, publish to NuGet server. Otherwise publish to MyGet server.
#
# .PARAMETER Reload
# If present, force re-import of custom modules into the current session.
#
# .INPUTS
# None.
#
# .OUTPUTS
# None.

[CmdletBinding()]
param(
    [switch] $retail,
    [switch] $reload
)

Set-StrictMode -Version Latest

# Force reload of the Project module.
if ($Reload) {
    Get-Module Project | Remove-Module
}

# Import Narvalo module.
if (!(Get-Module Project)) {
    Join-Path $PSScriptRoot 'tools\Project.psm1' | Import-Module 
}

Write-Host "Publishing packages..." -ForegroundColor 'Green'

$nuget = Install-NuGet
$packagesDir = Get-RepositoryPath 'work', 'packages'

Write-Host 'Not yet implemented!' -BackgroundColor Red -ForegroundColor Yellow

#& $nuget push "$packagesDir\*.nupkg"
