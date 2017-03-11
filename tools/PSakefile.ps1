#Framework '4.5.1x64'
Framework '4.6.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    # Process mandatory parameters.
    Assert($Retail -ne $null) "`$Retail must not be null, e.g. run with -Parameters @{ 'retail' = `$true; }"

    # MSBuild options.
    $Opts = '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false'

    # Main MSBuild projects.
    $Everything  = Get-LocalPath 'tools\Make.proj'

    $OpenCoverXml = Get-LocalPath 'work\log\opencover.xml'
}

FormatTaskName {
    param([Parameter(Mandatory = $true)] [string] $TaskName)

    Write-Host "Executing Task '$taskName'." -ForegroundColor DarkCyan
}

TaskTearDown {
    # Catch errors from both PowerShell and Win32 exe.
    if (!$?) {
        Exit-Gracefully -ExitCode 1 'Build failed.'
    }

    #if ($LastExitCode -ne 0) {
    #    Exit-Gracefully -ExitCode $LastExitCode 'Build failed.'
    #}
}

Task Default -Depends Build

# ------------------------------------------------------------------------------
# Continuous Integration and development tasks
# ------------------------------------------------------------------------------

Task Build `
    -Description 'Build.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Everything $Opts $CI_Props '/t:Build'
}

Task Test `
    -Description 'Build then run tests.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Everything $Opts $CI_Props `
        '/t:Xunit',
        '/p:Configuration=Debug',
        '/p:SkipDocumentation=true'
}

Task Package `
    -Description 'Create the packages.' `
    -Depends _Initialize-GitCommitHash, _Package-CheckVariablesForRetail `
    -RequiredVariables Retail `
    -Alias Pack `
{
    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Sign assemblies
    # - Unconditionally hide internals (implies no white-box testing)
    # Packaging targets:
    # - Rebuild all
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests
    # - Package
    MSBuild $Everything $Opts '/t:Xunit;Package' `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:GitCommitHash=$GitCommitHash",
        "/p:Retail=$Retail",
        '/p:SignAssembly=true',
        '/p:VisibleInternals=false'
}

Task OpenCover `
    -Description 'Run OpenCover (summary only).' `
    -Depends _CI-InitializeVariables `
    -Alias Cover `
{
    # Use debug build to also cover debug-only tests.
    MSBuild $Everything $Opts $CI_Props `
        '/t:Build',
        '/p:Configuration=Debug',
        '/p:SkipDocumentation=true'

    Invoke-OpenCover 'Debug'
    Invoke-ReportGenerator -Summary
}

Task OpenCoverVerbose `
    -Description 'Run OpenCover (full details).' `
    -Depends _CI-InitializeVariables `
    -Alias CoverVerbose `
{
    # Use debug build to also cover debug-only tests.
    MSBuild $Everything $Opts $CI_Props `
        '/t:Build',
        '/p:Configuration=Debug',
        '/p:SkipDocumentation=true'

    Invoke-OpenCover 'Debug'
    Invoke-ReportGenerator
}

# ------------------------------------------------------------------------------
# Helpers
# ------------------------------------------------------------------------------

Task _CI-InitializeVariables `
    -Description 'Initialize variables only used by the CI tasks.' `
    -RequiredVariables Retail `
{
    # Default CI properties:
    # - Release configuration
    # - Do not generate assembly versions
    # - Do not sign assemblies
    # - Leak internals to enable all white-box tests.
    $script:CI_Props = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        "/p:Retail=$Retail",
        '/p:SignAssembly=false',
        '/p:VisibleInternals=true'

    # FIXME: Don't understand why doing what follows does not work.
    # Either MSBuild or PowerShell mixes up the MSBuild parameters.
    # The result is that Configuration property takes all following properties
    # as its value. For instance, Configuration is read as "Release /p:BuildGeneratedVersion=false...".
    # For static analysis, we hide internals, otherwise we might not truly
    # analyze the public API.
    #$script:CI_AnalysisProps = $CI_Props, '/p:VisibleInternals=false'
}

Task _Package-CheckVariablesForRetail `
    -Description 'Check conditions are met for creating retail packages.' `
    -Depends _Initialize-GitCommitHash `
    -PreCondition { $Retail } `
{
    if ($GitCommitHash -eq '') {
        Exit-Gracefully -ExitCode 1 `
            'When building retail packages, the git commit hash MUST not be empty.'
    }
}

Task _Initialize-GitCommitHash `
    -Description 'Initialize GitCommitHash.' `
{
    $git = (Get-Git)

    $hash = ''

    if ($git -ne $null) {
        $status = Get-GitStatus $git -Short

        if ($status -eq $null) {
            Write-Warning 'Skipping... unabled to verify the git status.'
        } elseif ($status -ne '') {
            Write-Warning 'Skipping... uncommitted changes are pending.'
        } else {
            $hash = Get-GitCommitHash $git
        }
    }

    $script:GitCommitHash = $hash
}

# ------------------------------------------------------------------------------
# Functions
# ------------------------------------------------------------------------------

function Invoke-OpenCover {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [string] $Configuration
    )

    # TODO: Make it survive NuGet updates.
    $OpenCoverVersion = '4.6.519'
    $XunitVersion = '2.2.0'

    $opencover = Get-LocalPath "packages\OpenCover.$OpenCoverVersion\tools\OpenCover.Console.exe" -Resolve
    $xunit     = Get-LocalPath "packages\xunit.runner.console.$XunitVersion\tools\xunit.console.exe" -Resolve

    $filter = '+[Narvalo*]* -[*Facts]* -[Xunit.*]*'
    $excludeByAttribute = 'System.Runtime.CompilerServices.CompilerGeneratedAttribute;Narvalo.ExcludeFromCodeCoverageAttribute;System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute'

    $asm1 = Get-LocalPath "work\bin\$Configuration\Narvalo.Facts.dll" -Resolve
    $asm2  = Get-LocalPath "work\bin\$Configuration\Narvalo.Finance.Facts.dll" -Resolve
    $asm3  = Get-LocalPath "work\bin\$Configuration\Narvalo.Mvp.Facts.dll" -Resolve
    $asms = "$asm1 $asm2 $asm3"

    # Be very careful with arguments containing spaces.
    . $opencover `
      -register:user `
      "-filter:$filter" `
      "-excludebyattribute:$excludeByAttribute" `
      "-output:$OpenCoverXml" `
      "-target:$xunit"  `
      "-targetargs:$asms -nologo -noshadow"
}

function Invoke-ReportGenerator {
    [CmdletBinding()]
    param(
        [switch] $Summary
    )

    # TODO: Make it survive NuGet updates.
    $ReportGeneratorVersion = '2.5.6'

    $reportgenerator = Get-LocalPath "packages\ReportGenerator.$ReportGeneratorVersion\tools\ReportGenerator.exe" -Resolve

    if ($summary.IsPresent) {
        $targetdir   = Get-LocalPath 'work\log'
        $filters     = '+*'
        $reporttypes = 'HtmlSummary'
    }
    else {
        $targetdir   = Get-LocalPath 'work\log\opencover'
        $filters     = '-Narvalo.Common;-Narvalo.Core;-Narvalo.Fx;-Narvalo.Mvp;-Narvalo.Mvp.Web;-Narvalo.Web'
        $reporttypes = 'Html'
    }

    . $reportgenerator `
        -verbosity:Info `
        -reporttypes:$reporttypes `
        "-filters:$filters" `
        -reports:$OpenCoverXml `
        -targetdir:$targetdir
}

# ------------------------------------------------------------------------------
