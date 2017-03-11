# Old stuff.

$OpenCoverXml = Get-LocalPath 'work\log\opencover.xml'

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
