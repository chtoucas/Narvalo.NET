#Requires -Version 4.0

# .SYNOPSIS
# Run the PSake build script.
#
# .PARAMETER Docs
# If present, display the list of available tasks then exit.
#
# .PARAMETER Pristine
# If present, force re-import of local modules into the current session.
#
# .PARAMETER NoLogo
# If present, don't display the startup banner.
#
# .PARAMETER Retail
# If present, packages/assemblies are built for retail.
#
# .PARAMETER TaskList
# Specifies the list of tasks to be executed.
#
# .PARAMETER Verbosity
# Specifies the amount of information displayed by MSBuild.
# You can use the following verbosity levels:
#   q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
#
# .INPUTS
# The list of tasks to be executed.
#
# .OUTPUTS
# None.
#
# .EXAMPLE
# make.ps1 -Verbosity detailed
# Default task with detailed informations.
#
# .EXAMPLE
# make.ps1 CodeAnalysis -v quiet
# Quiet run of the Code Analysis task.
#
# .EXAMPLE
# make.ps1 -Retail Package
# Create retail packages with the default verbosity level.
#
# .LINK
# https://github.com/psake/psake

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [Alias('t')] [string[]] $TaskList = @(),

    [Alias('r')] [switch] $Retail,

    [Parameter(Mandatory = $false, Position = 1)]
    [Alias('v')] [string] $Verbosity = 'minimal',
    [switch] $Docs,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

if (!$noLogo.IsPresent) {
    if ($retail.IsPresent) {
        Write-Host "Make (RETAIL). Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Make. Copyright (c) Narvalo.Org.`n"
    }
}

# ------------------------------------------------------------------------------

if ($pristine.IsPresent) {
    Write-Debug 'Unload Helpers & Project module.'
    Get-Module Helpers | Remove-Module
    Get-Module Project | Remove-Module
}

if ($pristine.IsPresent -or !(Get-Module Helpers)) {
    Write-Debug 'Import the Helpers module.'
    Join-Path $PSScriptRoot 'tools\Helpers.psm1' | Import-Module
}
if ($pristine.IsPresent -or !(Get-Module Project)) {
    Write-Debug 'Import the Project module.'
    Join-Path $PSScriptRoot 'tools\Project.psm1' | Import-Module -NoClobber
}
if (!(Get-Module psake)) {
    Write-Debug 'Ensure PSake is installed.'
    Project\Get-NuGet | Project\Install-NuGet | Project\Restore-SolutionPackages

    Write-Debug 'Import the psake module.'
    Project\Get-PSakeModulePath | Import-Module -NoClobber
}

# ------------------------------------------------------------------------------

$psakefile = (Project\Get-RepositoryPath 'tools', 'PSakefile.ps1')

if ($docs.IsPresent) {
    #Get-Help $MyInvocation.MyCommand.Path
    Write-Host 'LIST OF AVAILABLE TASKS'
    Invoke-PSake $psakefile -NoLogo -Docs
    Exit 0
}

Write-Debug 'Ensure there is no concurrent MSBuild running.'
Get-Process -Name "msbuild" -ErrorAction SilentlyContinue | %{ Stop-Process $_.ID -force }

Invoke-PSake $psakefile `
    -NoLogo `
    -TaskList $taskList `
    -Parameters @{ 'verbosity' = $verbosity; 'retail' = $retail.IsPresent; }

# ------------------------------------------------------------------------------
