#Requires -Version 4.0

<#
.SYNOPSIS
    Run the PSake build script.
.PARAMETER Developer
    If present, enable the developer mode.
.PARAMETER Docs
    If present, display the list of available tasks then exit.
.PARAMETER Help
    If present, display the help then exit.
.PARAMETER Pristine
    If present, force re-import of local modules into the current session.
    This is a developer option and should not be used under normal circumstances.
.PARAMETER NoLogo
    If present, don't display the startup banner.
.PARAMETER Retail
    If present, packages/assemblies are built for retail.
.PARAMETER Safe
    If present, ensures there is no concurrent MSBuild running.
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
    make.ps1 -Retail Package
    Create retail packages.
.EXAMPLE
    make.ps1 -Verbosity detailed
    Run default task with detailed informations.
.EXAMPLE
    make.ps1 CA, SA -v quiet
    Quiet run of the Code Analysis & SecurityAnalysis tasks.
.LINK
    https://github.com/psake/psake
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [Alias('t')] [string[]] $TaskList = @(),

    [Alias('r')] [switch] $Retail,

    [Parameter(Mandatory = $false, Position = 1)]
    [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
    [Alias('v')] [string] $Verbosity = 'minimal',

    [switch] $Safe,
    [switch] $Docs,
    [Alias('h')] [switch] $Help,
    [switch] $NoLogo,
    [switch] $Pristine,
    [Alias('d')] [switch] $Developer
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

if (!(Get-Module Narvalo.Local)) {
    Import-Module (Join-Path $PSScriptRoot 'tools\src\Narvalo.Local.psm1')
}
$module = Import-LocalModule 'Narvalo.ProjectAutomation' $pristine.IsPresent -Args $PSScriptRoot

if (!$noLogo.IsPresent) {
    $version = $module.Version

    if ($retail.IsPresent) {
        Write-Host "Make (RETAIL), version $version. Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Make, version $version. Copyright (c) Narvalo.Org.`n"
    }
}

if (!(Get-Module psake)) {
    Write-Debug 'Ensure PSake is installed by restoring the solution packages.'
    Restore-SolutionPackages -Verbosity quiet

    Write-Debug 'Import the psake module.'
    Get-PSakeModulePath | Import-Module -NoClobber
}

$psakefile = Get-LocalPath 'tools\PSakefile.ps1' -Resolve

if ($help.IsPresent) {
    Get-Help $MyInvocation.MyCommand.Path -Full
    Exit 0
}

if (!$developer) {
    foreach ($task in $taskList) {
        if ($task.StartsWith('_')) {
            Exit-Gracefully -ExitCode 1 `
                'Private tasks are only available when the developer mode is on.'
        }
    }
}

if ($docs.IsPresent) {
    $taskList = '_Documentation'
}

if ($safe.IsPresent) {
    Stop-AnyMSBuildProcess
}

Invoke-PSake $psakefile `
    -NoLogo `
    -TaskList $taskList `
    -Parameters @{
        'developer' = $developer;
        'verbosity' = $verbosity;
        'retail' = $retail.IsPresent;
    }

# ------------------------------------------------------------------------------
