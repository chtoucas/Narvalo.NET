#Requires -Version 4.0
#Requires -RunAsAdministrator

<#
.SYNOPSIS
    Deploy the MyGet server.
.NOTES
    Explore DSC or PSWorkflow.
.LINKS
    http://technet.microsoft.com/fr-fr/library/ee790599.aspx
    http://technet.microsoft.com/fr-fr/library/ee909471%28v=ws.10%29.aspx
    http://www.iis.net/downloads/microsoft/powershell
#>

[CmdletBinding()]
param(
    [Alias('f')] [switch] $Force,
    [Alias('h')] [switch] $Help,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

if (!$noLogo.IsPresent) {
    $version = '0.1.0'

    if ($force.IsPresent) {
        Write-Host "Deploy-MyGet, version $version. Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Deploy-MyGet (DRY RUN), version $version. Copyright (c) Narvalo.Org.`n"
    }
}

if ($help.IsPresent) {
    Get-Help $MyInvocation.MyCommand.Path -Full
    Exit 0
}

Write-Error 'Not yet implemented.'

# ------------------------------------------------------------------------------
