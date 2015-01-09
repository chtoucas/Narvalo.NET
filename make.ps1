#Requires -Version 4.0

<#
.SYNOPSIS
    Run the PSake build script.
.PARAMETER Docs
    If present, display the list of available tasks then exit.
.PARAMETER Pristine
    If present, force re-import of local modules into the current session.
    This is a developer option and should not be used under normal circumstances.
.PARAMETER NoLogo
    If present, don't display the startup banner.
.PARAMETER Retail
    If present, packages/assemblies are built for retail.
.PARAMETER TaskList
    Specifies the list of tasks to be executed.
.PARAMETER Verbosity
    Specifies the amount of information displayed by MSBuild.
    You can use the following verbosity levels:
        q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
.INPUTS
    The list of tasks to be executed.
.OUTPUTS
    None.
.EXAMPLE
    make.ps1 -Verbosity detailed
    Default task with detailed informations.
.EXAMPLE
    make.ps1 CodeAnalysis -v quiet
    Quiet run of the Code Analysis task.
.EXAMPLE
    make.ps1 -Retail Package
    Create retail packages with the default verbosity level.
.LINK
    https://github.com/psake/psake
#>

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

Import-Module (Join-Path $PSScriptRoot 'tools\Narvalo.Local.psm1') -Force
$module = Import-RepositoryModule 'Narvalo.Project' $pristine.IsPresent -Args $PSScriptRoot

if (!$noLogo.IsPresent) {
    $version = $module.Version

    if ($retail.IsPresent) {
        Write-Host "Make (RETAIL) v$version. Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Make v$version. Copyright (c) Narvalo.Org.`n"
    }
}

if (!(Get-Module psake)) {
    Write-Debug 'Ensure PSake is installed.'
    Get-NuGet -Install | Restore-SolutionPackages

    Write-Debug 'Import the psake module.'
    Get-PSakeModulePath | Import-Module -NoClobber
}

$psakefile = Get-ProjectItem 'tools\PSakefile.ps1' -Resolve

if ($docs.IsPresent) {
    #Get-Help $MyInvocation.MyCommand.Path
    Write-Host 'LIST OF AVAILABLE TASKS'
    Invoke-PSake $psakefile -NoLogo -Docs
    Exit 0
}

Stop-AnyMSBuildProcess

Invoke-PSake $psakefile `
    -NoLogo `
    -TaskList $taskList `
    -Parameters @{ 'verbosity' = $verbosity; 'retail' = $retail.IsPresent; }

# ------------------------------------------------------------------------------
