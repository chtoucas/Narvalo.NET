#Requires -Version 4.0

<#
.SYNOPSIS
    Run the build script.
.PARAMETER Help
    If present, display the help then exit.
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
    [Alias('h')] [switch] $Help
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

New-Variable -Name ProjectRoot `
    -Value $PSScriptRoot `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Path to the local repository for the project Narvalo.NET.'

# ------------------------------------------------------------------------------

if ($retail.IsPresent) {
    Write-Host "Make script (RETAIL).`n"
} else {
    Write-Host "Make script - Non-retail version.`n"
}

if ($help.IsPresent) {
    Get-Help $MyInvocation.MyCommand.Path -Full
    Exit 0
}

# Load the helpers.
. '.\tools\helpers.ps1'

if ($safe.IsPresent) {
    Stop-AnyMSBuildProcess
}

Restore-SolutionPackages

Write-Host -BackgroundColor Red -ForegroundColor Yellow `
    "Due to the upgrade to VS 2017, the build script is currently broken." 

#Invoke-PSake $psakefile `
#    -NoLogo `
#    -TaskList $taskList `
#    -Parameters @{
#        'verbosity' = $verbosity;
#        'retail' = $retail.IsPresent;
#    }

# ------------------------------------------------------------------------------
