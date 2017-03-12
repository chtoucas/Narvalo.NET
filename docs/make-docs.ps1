#Requires -Version 4.0

[CmdletBinding()]
param(
    [Alias('x')] [switch] $ExtractApi,
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

$docfx = (Get-LocalPath "packages") |
    Find-PkgTool -Pkg 'docfx.console.*' -Tool 'tools\docfx.exe'

# Extract language metadata.
if ($ExtractApi) { . $docfx metadata }

# Generate documentation.
if ($Build) { . $docfx build }

# Start a local HTTP server.
if ($Test) { . $docfx serve _www }

# ------------------------------------------------------------------------------
