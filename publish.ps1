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

# Force reload of the Narvalo module.
if ($Reload) {
    Get-Module Narvalo | Remove-Module
}

# Import Narvalo module.
if (!(Get-Module Narvalo)) {
    Join-Path $PSScriptRoot 'tools\Narvalo.psm1' | Import-Module 
}

Write-Host "Publishing packages..." -ForegroundColor 'Yellow'

$NuGet = Install-NuGet

$packagesDir = Get-RepositoryPath 'work', 'packages'

#& "$NuGet" push "$packagesDir\*.nupkg"
