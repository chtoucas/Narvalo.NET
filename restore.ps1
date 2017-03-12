#Requires -Version 4.0

<#
.SYNOPSIS
    Restore solution packages.
.INPUTS
    None.
.OUTPUTS
    None.
#>

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

. '.\tools\script-helpers.ps1'

Write-Host '> Restoring solution packages' -NoNewline
Restore-SolutionPackages
Write-Host ' ...Done'

# ------------------------------------------------------------------------------
