#Requires -Version 3.0

# .SYNOPSIS
# Run the PSake build script.
#
# .PARAMETER TaskList
# The list of tasks to be executed.
#
# .PARAMETER Verbosity
# Specifies the amount of information displayed by MSBuild.
# You can use the following verbosity levels: 
#   q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]. 
#
# .PARAMETER Docs
# If present, display the list of available tasks then exit.
#
# .PARAMETER Pristine
# If present, force re-import of local modules into the current session.
#
# .PARAMETER NoLogo
# Don't display the startup banner.
#
# .PARAMETER Retail
# If present, packages/assemblies are built for retail.
#
# .INPUTS
# The task to be executed.
#
# .OUTPUTS
# None.
#
# .EXAMPLE
# make.ps1 -Verbosity detailed
# Default task with detailed informations.
#
# .EXAMPLE
# make.ps1 CodeAnalysis quiet
# Quiet run of the Code Analysis task.
#
# .EXAMPLE
# make.ps1 -r Package 
# Create retail packages with the default verbosity level.
#
# .LINK
# https://github.com/psake/psake

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [Alias('t')] [string[]] $taskList = @(),

    [Parameter(Mandatory = $false, Position = 1)]
    [Alias('v')] [string] $verbosity = 'minimal',

    [Alias('r')] [switch] $retail,
    [Alias('q')] [switch] $quiet,
    [switch] $docs,
    [switch] $noLogo,
    [switch] $pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

if ($pristine) {
    Write-Debug 'Unload Helpers & Project modules.'
    Get-Module Helpers | Remove-Module
    Get-Module Project | Remove-Module
}

if (!(Get-Module Helpers)) {
    Write-Debug 'Import the Helpers module.'
    Join-Path $PSScriptRoot 'tools\Helpers.psm1' | Import-Module
}
if (!(Get-Module Project)) {
    Write-Debug 'Import the Project module.'
    Join-Path $PSScriptRoot 'tools\Project.psm1' | Import-Module -NoClobber
}
if (!(Get-Module psake)) {
    Write-Debug 'Ensure PSake is installed.'
    Project\Install-NuGet | Project\Restore-SolutionPackages

    Write-Debug 'Import the psake module.'
    Project\Get-PSakeModulePath | Import-Module -NoClobber
}

# ------------------------------------------------------------------------------

# Path to the PSake script.
$PSakefile = (Project\Get-RepositoryPath 'tools\PSakefile.ps1')

if (!$noLogo) {
    Write-Host (?: $retail 'Make (RETAIL).' 'Make.'), "Copyright (c) Narvalo.Org.`n"
}

if ($docs) {
    #Get-Help $MyInvocation.MyCommand.Path
    Write-Host 'LIST OF AVAILABLE TASKS'
    Invoke-PSake $PSakefile -NoLogo -Docs
    Exit 0
}

Write-Debug 'Ensure there is no concurrent MSBuild running.'
Get-Process -Name "msbuild" -ErrorAction SilentlyContinue | %{ Stop-Process $_.ID -force }

Invoke-PSake $PSakefile `
    -NoLogo `
    -TaskList $taskList `
    -Parameters @{ 'verbosity' = $verbosity; 'retail' = $retail }

# ------------------------------------------------------------------------------
