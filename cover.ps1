#Requires -Version 4.0

<#
.EXAMPLE
    Rebuild then run test coverage for Narvalo.Core with full details:
    cover.ps1 Narvalo.Core -r -f
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [string] $AssemblyName = '*',

    [Alias('r')] [switch] $Rebuild,

    [Alias('f')] [switch] $Full
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

Write-Host "Test coverage script.`n"

. '.\tools\script-helpers.ps1'

# ------------------------------------------------------------------------------

$packages     = (Get-LocalPath "packages")
$opencoverxml = (Get-LocalPath 'work\log\opencover.xml')

if ($Rebuild) {
    # Build the projects then create the opencover report.

    if ($AssemblyName -eq '*') {
        .\make.ps1 -q
    } else {
        .\make.ps1 build $AssemblyName -q
    }

    $opencover = $packages | Get-OpenCoverExe
    $xunit = $packages | Get-XunitExe

    $asms = Get-ChildItem -Path (Get-LocalPath "work\bin\Debug\*") `
        -Include "$AssemblyName.Facts.dll"
    $targetargs = $asms -join " "

    # Be very careful with arguments containing spaces.
    . $opencover `
        -register:user `
        "-filter:+[Narvalo*]* -[*Facts]* -[Xunit.*]*" `
        "-excludebyattribute:System.Runtime.CompilerServices.CompilerGeneratedAttribute;*.ExcludeFromCodeCoverageAttribute" `
        "-output:$opencoverxml" `
        "-target:$xunit"  `
        "-targetargs:$targetargs -nologo -noshadow"
}

# Generate the report.
# WARNING: Be sure that you already run opencover; option $Rebuild.

$reportgenerator = $packages | Get-ReportGeneratorExe

if ($Full) {
    $targetdir     = Get-LocalPath 'work\log\opencover'
    $reporttypes   = 'Html'
    $result        = 'work\log\opencover\index.htm'
}
else {
    $targetdir     = Get-LocalPath 'work\log'
    $reporttypes   = 'HtmlSummary'
    $result        = 'work\log\summary.htm'
}

. $reportgenerator `
    -verbosity:Info `
    -reporttypes:$reporttypes `
    "-assemblyfilters:+$AssemblyName" `
    -reports:$opencoverxml `
    -targetdir:$targetdir

Write-Host "The report is here: $result." -BackgroundColor 'DarkGreen' -ForegroundColor Yellow

# ------------------------------------------------------------------------------
