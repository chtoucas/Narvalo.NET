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
.PARAMETER Task
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
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [ValidateSet('build', 'test', 'cover', 'pack')]
    [Alias('t')] [string] $Task = 'build',

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

# ------------------------------------------------------------------------------

# Path to the local repository for the project Narvalo.NET.
$ProjectRoot = $PSScriptRoot

# Main MSBuild projects.
$Project  = Get-LocalPath 'tools\Make.proj'

# Common MSBuild options.
$MSBuildCommonProps = '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false';

# Default CI properties.
# - Leak internals to enable all white-box tests.
$MSBuildCIProps = `
    '/p:Configuration=Debug',
    '/p:BuildGeneratedVersion=false',
    "/p:Retail=$Retail",
    '/p:SignAssembly=false',
    '/p:SkipDocumentation=true',
    '/p:VisibleInternals=true'

# ------------------------------------------------------------------------------

if ($safe.IsPresent) {
    Stop-AnyMSBuildProcess
}

Restore-SolutionPackages

# See https://github.com/Microsoft/vswhere/wiki/Find-MSBuild
# Apparently, the documentation (or vswhere) is wrong, it gives us the path to VS not MSBuild.
$VisualStudio = .\packages\vswhere.1.0.50\tools\vswhere -latest -products * `
    -requires Microsoft.Component.MSBuild -property installationPath
$MSBuild = Join-Path $VisualStudio 'MSBuild\15.0\Bin\MSBuild.exe'
if (!(Test-Path $MSBuild)) {
    Write-Host -BackgroundColor Red -ForegroundColor Yellow 'Unable to locate MSBuild' `
    Exit 1
}

switch ($task) {
    'build' { Invoke-BuildProjects }
    'test' { Invoke-TestProjects }
    'cover' { Write-Host "COVER has not yet been implemented." }
    'pack' { Invoke-Package }
}

# ------------------------------------------------------------------------------
