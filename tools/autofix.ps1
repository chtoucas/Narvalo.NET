#Requires -Version 3.0

# TODO:
# Check hidden files
# Check DependentUpon, SubType files
# Check status & ignored & untracked: git status -u --ignored
# Check files ignored by StyleCop
# Reset repository
# - Strong: Git reset (WARNING: remove ALL ignored files)
# - Soft: Remove nuget.exe, 7zip.exe

# .SYNOPSIS
# Perform maintenance tasks.
#
# .PARAMETER Force
# If present, do the actual work, otherwise only display what would have been done.
#
# .PARAMETER NoLogo
# If present, don't display the startup banner.
#
# .PARAMETER Pristine
# If present, force re-import of local modules into the current session.
#
# .PARAMETER Purge
# If present, process cleanup tasks.
#
# .PARAMETER Repair
# If present, process repair tasks.
#
# .PARAMETER Yes
# If present, do not ask for any confirmation.
#
# .OUTPUTS
# None.
[CmdletBinding()]
param(
    [Alias('fix')] [switch] $repair,
    [switch] $purge,

    [Alias('f')] [switch] $force,
    [Alias('y')] [switch] $yes,
    [switch] $noLogo,
    [switch] $pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

$verbose = $PSBoundParameters.ContainsKey('Verbose')
#$whatIf = $WhatIfPreference
$dryRun = !$force.IsPresent

if (!$noLogo.IsPresent) {
    if ($dryRun) {
        Write-Host "Autofix (DRY RUN). Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Autofix. Copyright (c) Narvalo.Org.`n"
    }
}

# ------------------------------------------------------------------------------

if ($pristine.IsPresent) {
    Write-Debug 'Unload Project & Autofix modules.'
    Get-Module Project | Remove-Module
    Get-Module Autofix | Remove-Module
}

if ($pristine.IsPresent -or !(Get-Module Project)) {
    Write-Debug 'Import the Project module.'
    Join-Path $PSScriptRoot 'Project.psm1' | Import-Module -NoClobber
}
if ($pristine.IsPresent -or !(Get-Module Autofix)) {
    Write-Debug 'Import the Autofix module.'
    Join-Path $PSScriptRoot 'Autofix.psm1' | Import-Module -NoClobber
}

# ------------------------------------------------------------------------------

$srcDirs = 'samples', 'src', 'tests' | %{ Project\Get-RepositoryPath $_ }

if ($repair.IsPresent) {
    if ($yes.IsPresent -or (Read-Host 'Repair copyright headers? [y/N]') -eq 'y') {
        $srcDirs | Autofix\Repair-Copyright -WhatIf:$dryRun -v:$verbose
    }
}

if ($purge.IsPresent) {
    if ($yes.IsPresent -or (Read-Host 'Remove ''bin'' and ''obj'' directories? [y/N]') -eq 'y') {
        $srcDirs | Autofix\Remove-BinAndObj -WhatIf:$dryRun -v:$verbose
    }

    if ($yes.IsPresent -or (Read-Host 'Remove ''packages'' directory? [y/N]') -eq 'y') {
        $packagesDir = Project\Get-RepositoryPath 'packages' 
        if (Test-Path $packagesDir) {
            Remove-Item $packagesDir -Force -Recurse -ErrorAction SilentlyContinue -WhatIf:$dryRun
        }
    }

    if ($yes.IsPresent -or (Read-Host 'Remove ''work'' directory? [y/N]') -eq 'y') {
        $workDir = Project\Get-RepositoryPath 'work' 
        if (Test-Path $workDir) {
            Remove-Item $workDir -Force -Recurse -ErrorAction SilentlyContinue -WhatIf:$dryRun
        }
    }

    if ($yes.IsPresent -or (Read-Host 'Remove all untracked files? [y/N]') -eq 'y') {
        Project\Get-Git | Autofix\Remove-UntrackedItems -Path (Project\Get-RepositoryRoot) -WhatIf:$dryRun -v:$verbose
    }
}

# ------------------------------------------------------------------------------
