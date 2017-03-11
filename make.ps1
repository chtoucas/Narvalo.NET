#Requires -Version 4.0

<#
.SYNOPSIS
    Run the build script.
.PARAMETER Configuration
    Specifies the configuration (ignored by the task Package).
.PARAMETER FullCoverage
    Produces a detailed coverage report (only for the task Cover).
.PARAMETER Retail
    If present, packages/assemblies are built for retail (only for the task Package).
.PARAMETER Safe
    If present, ensures there is no concurrent MSBuild running.
.PARAMETER Task
    Specifies the task to be executed.
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
    [ValidateSet('Debug', 'Release')]
    [Alias('c')] [string] $Configuration = 'Debug',

    [Parameter(Mandatory = $false, Position = 2)]
    [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
    [Alias('v')] [string] $Verbosity = 'minimal',

    [switch] $Safe,
    [switch] $FullCoverage
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

# Path to the local repository (used by helpers.ps1).
$ProjectRoot = $PSScriptRoot

# Load the helpers.
. '.\tools\make-helpers.ps1'

# ------------------------------------------------------------------------------

# Main MSBuild projects.
$Project  = Get-LocalPath 'tools\Make.proj'

# Common MSBuild options.
$MSBuildCommonProps = '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false';

# CI properties.
$MSBuildCIProps = `
    "/p:Configuration=$Configuration",
    '/p:BuildGeneratedVersion=false',
    "/p:Retail=false",
    '/p:SignAssembly=false',
    '/p:SkipDocumentation=true',
    '/p:VisibleInternals=true'

# Packaging properties.
$MSBuildPackageProps = `
    '/p:Configuration=Release',
    '/p:BuildGeneratedVersion=true',
    "/p:Retail=$Retail",
    '/p:SignAssembly=true',
    '/p:SkipDocumentation=false',
    '/p:VisibleInternals=false'

# ------------------------------------------------------------------------------

if ($safe.IsPresent) {
    Stop-AnyMSBuildProcess
}

Restore-SolutionPackages

switch ($task) {
    'build' {
        & (Get-MSBuild) $Project $MSBuildCommonProps $MSBuildCIProps '/t:Build'
    }

    'test' {
        & (Get-MSBuild) $Project $MSBuildCommonProps $MSBuildCIProps '/t:Xunit'
    }

    'pack' {
        $git = (Get-Git)

        $hash = ''

        if ($git -ne $null) {
            $status = Get-GitStatus $git -Short

            if ($status -eq $null) {
                Write-Warning 'Skipping git commit hash... unabled to verify the git status.'
            } elseif ($status -ne '') {
                Write-Warning 'Skipping git commit hash... uncommitted changes are pending.'
            } else {
                $hash = Get-GitCommitHash $git
            }
        }

        if ($Retail -and $hash -eq '') {
            Exit-Gracefully -ExitCode 1 `
                'When building retail packages, the git commit hash CAN NOT be empty.'
        }

        & (Get-MSBuild) $Project $MSBuildCommonProps $MSBuildPackageProps "/p:GitCommitHash=$hash" '/t:Xunit;Package'
    }

    'cover' {
        & (Get-MSBuild) $Project $MSBuildCommonProps $MSBuildCIProps '/t:Build'

        $packages     = (Get-LocalPath "packages")
        $opencoverxml = Get-LocalPath 'work\log\opencover.xml'

        # OpenCover
        # ---------

        $opencover = $packages | Find-PkgTool -Pkg 'OpenCover.*' -Tool 'tools\OpenCover.Console.exe'
        $xunit     = $packages | Find-PkgTool -Pkg 'xunit.runner.console.*' -Tool 'tools\xunit.console.exe'
        $asms      = Get-ChildItem -Path (Get-LocalPath "work\bin\$Configuration\*") -Include "*.Facts.dll"

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

        $reportgenerator = $packages | Find-PkgTool -Pkg 'ReportGenerator.*' -Tool 'tools\ReportGenerator.exe'

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
