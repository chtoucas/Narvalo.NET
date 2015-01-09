#Requires -Version 4.0

<#
.SYNOPSIS
    Perform maintenance tasks.
.PARAMETER Analyze
    If present, process analysis tasks.
.PARAMETER Force
    If present, do the actual work, otherwise only display what would have been done.
.PARAMETER NoLogo
    If present, don't display the startup banner.
.PARAMETER Pristine
    If present, force re-import of local modules into the current session.
.PARAMETER Purge
    If present, process cleanup tasks.
.PARAMETER Repair
    If present, process repair tasks.
.PARAMETER Yes
    If present, do not ask for any confirmation.
.INPUTS
    None.
.OUTPUTS
    None.
.EXAMPLE
    checkup.ps1 -Repair
    Process repair tasks but don't actually change anything.
.EXAMPLE
    checkup.ps1 -Repair -Force
    Process repair tasks and do the actual work.
.EXAMPLE
    checkup.ps1 -Purge -Yes
    Process cleanup tasks, do not ask for any confirmation, but don't actually change anything.
.NOTES
    On the TODO list:
    - Analyze: Find hidden VS files
    - Analyze: Find files ignored by git: git status -u --ignored
    - Purge: Git reset (WARNING: remove ALL ignored files)
    - Repair: Find DependentUpon & SubType files
    - Repair: Find files ignored by StyleCop
#>

[CmdletBinding()]
param(
    [switch] $Analyze,
    [switch] $Purge,
    [Alias('fix')] [switch] $Repair,

    [Alias('f')] [switch] $Force,
    [Alias('gr')] [switch] $GlobalRegistry,
    [Alias('y')] [switch] $Yes,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

$verbose = $PSBoundParameters.ContainsKey('Verbose')
#$whatIf = $WhatIfPreference
$dryRun = !$force.IsPresent

# ------------------------------------------------------------------------------

trap { 
    $message = 'A fatal error ''{0}'' of type ''{1}'' occured: {2}' -f
        $_.CategoryInfo.Category,
        $_.Exception.GetType().FullName,
        $_.Exception.Message

    Write-Warning $message

    break
}

# ------------------------------------------------------------------------------

Import-Module (Join-Path $PSScriptRoot 'Narvalo.Repository.psm1') -Force -Verbose

$module = Import-RepositoryModule 'Narvalo.Project' $pristine.IsPresent -Args (Get-Item $PSScriptRoot).Parent.FullName

# ------------------------------------------------------------------------------

if (!$noLogo.IsPresent) {
    $version = $module.Version

    if ($dryRun) {
        Write-Host "Checkup (DRY RUN) v$version. Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Checkup v$version. Copyright (c) Narvalo.Org.`n"
    }
}

# ------------------------------------------------------------------------------

$srcDirs = 'samples', 'src', 'tests' | %{ Get-ProjectItem $_ }

if ($analyze.IsPresent) {
    echo 'Analyze: nothing yet :-('
}

if ($purge.IsPresent) {
    if ($yes.IsPresent -or (Read-Host 'Remove ''bin'' and ''obj'' directories? [y/N]') -eq 'y') {
        $srcDirs | Remove-BinAndObj -WhatIf:$dryRun -v:$verbose
    }

    if ($yes.IsPresent -or (Read-Host 'Remove ''packages'' directory? [y/N]') -eq 'y') {
        $packagesDir = Get-ProjectItem 'packages' 
        if (Test-Path $packagesDir) {
            Remove-Item $packagesDir -Force -Recurse -ErrorAction SilentlyContinue -WhatIf:$dryRun
        }
    }

    if ($yes.IsPresent -or (Read-Host 'Remove ''work'' directory? [y/N]') -eq 'y') {
        $workDir = Get-ProjectItem 'work' 
        if (Test-Path $workDir) {
            Remove-Item $workDir -Force -Recurse -ErrorAction SilentlyContinue -WhatIf:$dryRun
        }
    }

    if ($yes.IsPresent -or (Read-Host 'Remove all untracked files? [y/N]') -eq 'y') {
        $git = (Get-Git)

        if ($git -ne $null) {
            $git | Remove-UntrackedItems -Path (Get-ProjectItem '') -WhatIf:$dryRun -v:$verbose
        }
    }

    if ($yes.IsPresent -or (Read-Host 'Remove local tools ''NuGet'' & ''7-Zip''? [y/N]') -eq 'y') {
        Remove-Item (Get-7Zip) -ErrorAction SilentlyContinue -WhatIf:$dryRun
        Remove-Item (Get-NuGet) -ErrorAction SilentlyContinue -WhatIf:$dryRun
    }
}

if ($repair.IsPresent) {
    if ($yes.IsPresent -or (Read-Host 'Repair copyright headers? [y/N]') -eq 'y') {
        $srcDirs | Repair-Copyright -WhatIf:$dryRun -v:$verbose
    }
}

# ------------------------------------------------------------------------------
