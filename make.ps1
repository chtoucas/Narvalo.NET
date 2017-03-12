#Requires -Version 4.0

<#
.SYNOPSIS
    Run the build script.
.PARAMETER FullCoverage
    Produces a detailed coverage report (only for the task 'cover').
.PARAMETER Release
    Instructs the script to use the Release configuration (ignored by the task 'pack')..
.PARAMETER Restore
    Restore solution packages.
.PARAMETER Retail
    If present, packages are built for retail (only for the task 'pack').
.PARAMETER Safe
    If present, ensures there is no concurrent MSBuild running.
.PARAMETER Task
    Specifies the task to be executed.
    You can use one of the following values:
        build, cover, pack, test
.PARAMETER Verbosity
    Specifies the amount of information displayed by MSBuild.
    You can use the following verbosity levels:
        q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
.INPUTS
    The task to be executed.
.OUTPUTS
    None.
.EXAMPLE
    Create retail packages.
    make.ps1 -Retail -t pack
.EXAMPLE
    Run default task (build) with detailed informations.
    make.ps1 -Verbosity detailed
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [ValidateSet('build', 'cover', 'pack', 'test')]
    [Alias('t')] [string] $Task = 'build',

    [Parameter(Mandatory = $false, Position = 2)]
    [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
    [Alias('v')] [string] $Verbosity = 'minimal',

    [switch] $FullCoverage,
    [switch] $Release,
    [switch] $Restore,
    [Alias('r')] [switch] $Retail,
    [switch] $Safe
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

if ($Retail.IsPresent) {
    Write-Host "Make script (RETAIL).`n"
} else {
    Write-Host "Make script - Non-retail version.`n"
}

# Path to the local repository (used by make-helpers.ps1).
$ProjectRoot = $PSScriptRoot

# Load the helpers.
. '.\tools\make-helpers.ps1'

# ------------------------------------------------------------------------------

# MSBuild project.
$project  = Get-LocalPath 'tools\Make.proj'

# Build configuration.
if ($Release.IsPresent) {
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

if ($Safe.IsPresent) {
    Stop-AnyMSBuildProcess
}

if ($Restore.IsPresent) {
    Write-Host '> Restoring solution packages ' -NoNewline
    Restore-SolutionPackages
}

Write-Host "> Executing the task '$Task'`n"

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

    'cover' {
        & (Get-MSBuild) $project $msbuildprops '/t:Build'

        $packages     = (Get-LocalPath "packages")
        $opencoverxml = Get-LocalPath 'work\log\opencover.xml'

        # OpenCover
        # ---------

        $opencover = $packages |
            Find-PkgTool -Pkg 'OpenCover.*' -Tool 'tools\OpenCover.Console.exe'
        $xunit = $packages |
            Find-PkgTool -Pkg 'xunit.runner.console.*' -Tool 'tools\xunit.console.exe'
        $asms = Get-ChildItem -Path (Get-LocalPath "work\bin\$configuration\*") `
            -Include "*.Facts.dll"

        $targetargs   = $asms -join " "

        # Be very careful with arguments containing spaces.
        . $opencover `
          -register:user `
          "-filter:+[Narvalo*]* -[*Facts]* -[Xunit.*]*" `
          "-excludebyattribute:System.Runtime.CompilerServices.CompilerGeneratedAttribute;*.ExcludeFromCodeCoverageAttribute" `
          "-output:$opencoverxml" `
          "-target:$xunit"  `
          "-targetargs:$targetargs -nologo -noshadow"

        # ReportGenerator
        # ---------------

        $reportgenerator = $packages |
            Find-PkgTool -Pkg 'ReportGenerator.*' -Tool 'tools\ReportGenerator.exe'

        if ($FullCoverage) {
            # WARNING: We filter out most assemblies.
            $targetdir     = Get-LocalPath 'work\log\opencover'
            $reportfilters = '-Narvalo.Common;-Narvalo.Fx;-Narvalo.Money;-Narvalo.Mvp;-Narvalo.Mvp.Web;-Narvalo.Web'
            $reporttypes   = 'Html'
        }
        else {
            $targetdir     = Get-LocalPath 'work\log'
            $reportfilters = '+*'
            $reporttypes   = 'HtmlSummary'
        }

        . $reportgenerator `
            -verbosity:Info `
            -reporttypes:$reporttypes `
            "-assemblyfilters:$reportfilters" `
            -reports:$opencoverxml `
            -targetdir:$targetdir
    }
}

# ------------------------------------------------------------------------------
