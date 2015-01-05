#Requires -Version 3.0

# TODO:
# Check hidden files
# Check DependentUpon, SubType files
# Check status & ignored & untracked: git status -u --ignored
# Check files ignored by StyleCop
# Reset repository
# - Strong: Git reset (WARNING: remove ALL ignored files)
# - Light: Remove nuget.exe, 7zip.exe, FullClean, remove obj & bin, packages

# .SYNOPSIS
# Perform maintenance tasks.
[CmdletBinding()]
param(
    [switch] $fixCopyright,
    [switch] $softClean,

    [Alias('f')] [switch] $force,
    [switch] $noLogo,
    [switch] $pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

if ($pristine) {
    Write-Debug 'Unload Helpers, Project & Autofix modules.'
    Get-Module Helpers | Remove-Module
    Get-Module Project | Remove-Module
    Get-Module Autofix | Remove-Module
}

if (!(Get-Module Helpers)) {
    Write-Debug 'Import the Helpers module.'
    Join-Path $PSScriptRoot 'Helpers.psm1' | Import-Module
}
if (!(Get-Module Project)) {
    Write-Debug 'Import the Project module.'
    Join-Path $PSScriptRoot 'Project.psm1' | Import-Module -NoClobber
}
if (!(Get-Module Autofix)) {
    Write-Debug 'Import the Autofix module.'
    Join-Path $PSScriptRoot 'Autofix.psm1' | Import-Module -NoClobber
}

# ------------------------------------------------------------------------------

$dryRun = !$force

if (!$noLogo) {
    Write-Host (?: $dryRun 'Autofix (DRY RUN).' 'Autofix.'), "Copyright (c) Narvalo.Org.`n"
}

if ($fixCopyright) {
    #Repair-Copyright -DryRun $dryRun
}

if ($softClean) {
    'samples', 'src', 'tests' | 
        % { (Project\Get-RepositoryPath $_) } | 
        % { Autofix\Remove-VisualStudioTmpFiles -Path $_ -Verbose }
}

# ------------------------------------------------------------------------------
