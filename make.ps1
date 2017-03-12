#Requires -Version 4.0

<#
.SYNOPSIS
    Run the build script.
.PARAMETER Release
    Instructs the script to use the Release configuration (ignored by the task 'pack')..
.PARAMETER Retail
    If present, packages are built for retail (only for the task 'pack').
.PARAMETER Safe
    If present, ensures there is no concurrent MSBuild running.
.PARAMETER Task
    Specifies the task to be executed.
    You can use one of the following values:
        build, cover, pack, test
    The default value is 'build'.
.PARAMETER Verbosity
    Specifies the amount of information displayed by MSBuild.
    You can use the following verbosity levels:
        q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
    The default value is 'minimal'.
.INPUTS
    The task to be executed.
.OUTPUTS
    None.
.EXAMPLE
    Create retail packages:
    make.ps1 -Retail -t pack
.EXAMPLE
    Run default task (build) with detailed informations:
    make.ps1 -v detailed
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [ValidateSet('build', 'cover', 'pack', 'test')]
    [Alias('t')] [string] $Task = 'build',

    [Parameter(Mandatory = $false, Position = 1)]
    [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
    [Alias('v')] [string] $Verbosity = 'minimal',

    [switch] $Release,
    [Alias('r')] [switch] $Retail,
    [switch] $Safe,

    [switch] $NoLogo,
    [Alias('q')] [switch] $Quiet
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

if ($Quiet) { $NoLogo = $true }

if (!$noLogo) {
    if ($Retail) {
        Write-Host "Make script (RETAIL).`n"
    } else {
        Write-Host "Make script - Non-retail version.`n"
    }
}

. '.\tools\script-helpers.ps1'

# ------------------------------------------------------------------------------

# MSBuild project.
$project  = Get-LocalPath 'tools\Make.proj'

# Build configuration.
if ($Release) {
    $configuration = 'Release'
} else {
    $configuration = 'Debug'
}

# MSBuild properties.
$msbuildprops = `
    '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false',
    "/p:Configuration=$configuration",
    '/p:BuildGeneratedVersion=false',
    "/p:Retail=false",
    '/p:SignAssembly=false',
    '/p:SkipDocumentation=true',
    '/p:VisibleInternals=true'

# MSBuild properties solely for the task 'pack'.
$msbuildpackprops = `
    '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false',
    '/p:Configuration=Release',
    '/p:BuildGeneratedVersion=true',
    "/p:Retail=$Retail",
    '/p:SignAssembly=true',
    '/p:SkipDocumentation=false',
    '/p:VisibleInternals=false'

# ------------------------------------------------------------------------------

if ($Safe) {
    Stop-AnyMSBuildProcess
}

if (!$Quiet) {
    Write-Host "> Executing the task '$Task'`n"
}

switch ($Task) {
    'build' { & (Get-MSBuild) $project $msbuildprops '/t:Build' }
    'test'  { & (Get-MSBuild) $project $msbuildprops '/t:Xunit' }

    'pack' {
        $git = (Get-Git)

        if ($git -eq $null) {
            Exit-Gracefully 'git.exe could not be found in your PATH. Please ensure git is installed.'
        }

        $hash = ''

        $status = Get-GitStatus $git -Short

        if ($status -eq $null) {
            Write-Warning 'Skipping git commit hash... unabled to verify the git status.'
        } elseif ($status -ne '') {
            Write-Warning 'Skipping git commit hash... uncommitted changes are pending.'
        } else {
            $hash = Get-GitCommitHash $git
        }

        if ($Retail -and $hash -eq '') {
            Exit-Gracefully 'When building retail packages, the git commit hash CAN NOT be empty.'
        }

        & (Get-MSBuild) $project $msbuildpackprops  "/p:GitCommitHash=$hash" '/t:Xunit;Package'
    }
}

# ------------------------------------------------------------------------------
