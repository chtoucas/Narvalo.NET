#Requires -Version 4.0

[CmdletBinding()]
param(
    [Alias('r')] [switch] $Retail
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

if ($Retail) {
    Write-Host "Publication script (RETAIL).`n"
} else {
    Write-Host "Publication script - Non-retail version.`n"
}

# ------------------------------------------------------------------------------

$programFiles = (${env:ProgramFiles(x86)}, ${env:ProgramFiles} -ne $null)[0]
$fsiVersion = '4.1'
$frameworkVersion = '4.0'

$fsi = "$programFiles\Microsoft SDKs\F#\$fsiVersion\Framework\v$frameworkVersion\fsi.exe"

if ($Retail) {
    $fsx = 'tools\publish-retail.fsx'
} else {
    $fsx = 'tools\publish-edge.fsx'
}

& $fsi $fsx

# ------------------------------------------------------------------------------
