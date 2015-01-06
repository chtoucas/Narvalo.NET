#Requires -Version 4.0


# .SYNOPSIS
# Perform maintenance tasks.
#
# .PARAMETER Analyze
# If present, process analysis tasks.
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
# .INPUTS
# None.
#
# .OUTPUTS
# None.
#
# .EXAMPLE
# checkup.ps1 -Repair
# Process repair tasks but don't actually change anything.
#
# .EXAMPLE
# checkup.ps1 -Repair -Force
# Process repair tasks and do the actual work.
#
# .EXAMPLE
# checkup.ps1 -Purge -Yes
# Process cleanup tasks, do not ask for any confirmation, 
# but don't actually change anything.
#
# .NOTES
# On the TODO list:
# - Analyze: Find hidden VS files
# - Analyze: Find files ignored by git: git status -u --ignored
# - Purge: Git reset (WARNING: remove ALL ignored files)
# - Repair: Find DependentUpon & SubType files
# - Repair: Find files ignored by StyleCop

[CmdletBinding()]
param(
    [switch] $Analyze,
    [switch] $Purge,
    [Alias('fix')] [switch] $Repair,

    [Alias('f')] [switch] $Force,
    [Alias('y')] [switch] $Yes,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

$verbose = $PSBoundParameters.ContainsKey('Verbose')
#$whatIf = $WhatIfPreference
$dryRun = !$force.IsPresent

if (!$noLogo.IsPresent) {
    if ($dryRun) {
        Write-Host "Checkup (DRY RUN). Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Checkup. Copyright (c) Narvalo.Org.`n"
    }
}

# ------------------------------------------------------------------------------

if ($pristine.IsPresent) {
    Write-Debug 'Unload Helpers, Project & Checkup modules.'
    Get-Module Helpers | Remove-Module
    Get-Module Project | Remove-Module
    Get-Module Checkup | Remove-Module
}

if ($pristine.IsPresent -or !(Get-Module Helpers)) {
    Write-Debug 'Import the Helpers module.'
    Join-Path $PSScriptRoot 'Helpers.psm1' | Import-Module
}
if ($pristine.IsPresent -or !(Get-Module Project)) {
    Write-Debug 'Import the Project module.'
    Join-Path $PSScriptRoot 'Project.psm1' | Import-Module -NoClobber
}
if ($pristine.IsPresent -or !(Get-Module Checkup)) {
    Write-Debug 'Import the Checkup module.'
    Join-Path $PSScriptRoot 'Checkup.psm1' | Import-Module -NoClobber
}

# ------------------------------------------------------------------------------

$srcDirs = 'samples', 'src', 'tests' | %{ Project\Get-RepositoryPath $_ }

if ($analyze.IsPresent) {
    echo 'Analyze: nothing yet :-('
}

if ($purge.IsPresent) {
    if ($yes.IsPresent -or (Read-Host 'Remove ''bin'' and ''obj'' directories? [y/N]') -eq 'y') {
        $srcDirs | Checkup\Remove-BinAndObj -WhatIf:$dryRun -v:$verbose
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
        $git = (Project\Get-Git)

        if ($git -ne $null) {
            $git | Checkup\Remove-UntrackedItems -Path (Project\Get-RepositoryRoot) -WhatIf:$dryRun -v:$verbose
        }
    }

    if ($yes.IsPresent -or (Read-Host 'Remove local tools ''NuGet'' & ''7-Zip''? [y/N]') -eq 'y') {
        Remove-Item (Project\Get-7Zip) -ErrorAction SilentlyContinue -WhatIf:$dryRun
        Remove-Item (Project\Get-NuGet) -ErrorAction SilentlyContinue -WhatIf:$dryRun
    }
}

if ($repair.IsPresent) {
    if ($yes.IsPresent -or (Read-Host 'Repair copyright headers? [y/N]') -eq 'y') {
        $srcDirs | Checkup\Repair-Copyright -WhatIf:$dryRun -v:$verbose
    }
}

# ------------------------------------------------------------------------------
