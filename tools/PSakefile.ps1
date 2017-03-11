#Framework '4.5.1x64'
Framework '4.6.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    # Process mandatory parameters.
    Assert($Retail -ne $null) "`$Retail must not be null, e.g. run with -Parameters @{ 'retail' = `$true; }"

    $OpenCoverXml = Get-LocalPath 'work\log\opencover.xml'
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
# ------------------------------------------------------------------------------
# Tasks
# ------------------------------------------------------------------------------

Task OpenCover `
{
    MSBuild $script:Project $script:MSBuildCommonProps $script:MSBuildCIProps '/t:Build'

    Invoke-OpenCover 'Debug'
    Invoke-ReportGenerator -Summary
}

Task OpenCoverVerbose `
{
    MSBuild $script:Project $script:MSBuildCommonProps $script:MSBuildCIProps '/t:Build'

    Invoke-OpenCover 'Debug'
    Invoke-ReportGenerator
}

Task Package `
    -Depends _Initialize-GitCommitHash, _Package-CheckVariablesForRetail `
    -RequiredVariables Retail `
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
    MSBuild $script:Project $script:MSBuildCommonProps `
        '/t:Xunit;Package' `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:GitCommitHash=$GitCommitHash",
        "/p:Retail=$Retail",
        '/p:SignAssembly=true',
        '/p:VisibleInternals=false'
}

# ------------------------------------------------------------------------------
# Helpers
# ------------------------------------------------------------------------------

Task _Package-CheckVariablesForRetail `
    -Depends _Initialize-GitCommitHash `
    -PreCondition { $Retail } `
{
    if ($GitCommitHash -eq '') {
        Exit-Gracefully -ExitCode 1 `
            'When building retail packages, the git commit hash MUST not be empty.'
    }
}

Task _Initialize-GitCommitHash `
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
