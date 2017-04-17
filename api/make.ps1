#Requires -Version 4.0

[CmdletBinding()]
param(
    [Alias('x')] [switch] $Extract,
    [Alias('b')] [switch] $Build,
    [Alias('t')] [switch] $Test
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

. '..\tools\script-helpers.ps1'

# ------------------------------------------------------------------------------

$docfx = (Get-LocalPath "packages") | Get-DocFXExe

# Extract language metadata.
if ($Extract) { . $docfx metadata }

# Generate documentation.
if ($Build) { . $docfx build }

# Start a local HTTP server.
if ($Test) { . $docfx serve _www }

# ------------------------------------------------------------------------------
