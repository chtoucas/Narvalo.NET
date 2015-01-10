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
    [Alias('y')] [switch] $Yes,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

Import-Module (Join-Path $PSScriptRoot 'Narvalo.Local.psm1') -Force
$module = Import-LocalModule 'Narvalo.Project' $pristine.IsPresent -Args (Get-Item $PSScriptRoot).Parent.FullName

if (!$noLogo.IsPresent) {
    $version = $module.Version

    if ($force.IsPresent) {
        Write-Host "Checkup, version $version. Copyright (c) Narvalo.Org.`n"
        Write-Warning '''Force'' mode on, any modification will be permanent.'
        if ((Read-Host 'Are you sure you wish to continue? [y/N]') -ne 'y') {
            Write-Host "Cancelling on use request.`n" -ForeGround Green
            Exit 0
        }
    } else {
        Write-Host "Checkup (DRY RUN), version $version. Copyright (c) Narvalo.Org.`n"
        Write-Warning 'Safe mode on, no modifications to the repository will happen. Use the option ''-Force'' when you are ready.'
        Write-Host ''
    }
}

if ($yes.IsPresent) {
    Write-Warning '''Yes'' mode on, ALL tasks will be processed WITHOUT any prior confirmation.'
    if ((Read-Host 'Are you sure you wish to continue? [y/N]') -ne 'y') {
        Write-Host "Cancelling on use request.`n" -ForeGround Green
        Exit 0
    }
}

function Write-Header($message) { Write-Host $message -ForeGround DarkCyan }
function Write-TaskCompleted { Write-Host 'Task completed.' -ForeGround Green }

$verbose = $PSBoundParameters.ContainsKey('Verbose')
#$whatIf = $WhatIfPreference
$dryRun = !$force.IsPresent

if ($analyze.IsPresent) {
    $analyzeMessage = @"
Ready to proceed to the analysis tasks.

"@

    Write-Header $analyzeMessage

    Write-Warning 'Nothing yet :-('
    Write-Host ''
}

if ($purge.IsPresent) {
    $purgeMessage = @"
Ready to proceed to the cleanup tasks. You will be offered the following options:
- Remove 'bin' and 'obj' directories created by Visual Studio.
- Remove the NuGet packages directory.
- Remove the 'work' directory created by the build script.
- Remove files untracked by git.
- Remove the locally installed tools: nuget.exe and 7za.exe.

"@

    Write-Header $purgeMessage

    if ($yes.IsPresent -or (Read-Host 'Remove ''bin'' and ''obj'' directories? [y/N]') -eq 'y') {
        'samples', 'src', 'tests' | Remove-BinAndObj -WhatIf:$dryRun -v:$verbose
        Write-TaskCompleted
    }

    if ($yes.IsPresent -or (Read-Host 'Remove ''packages'' directory? [y/N]') -eq 'y') {
        $packagesDir = Get-LocalPath 'packages' 
        if (Test-Path $packagesDir) {
            Remove-Item $packagesDir -Force -Recurse -WhatIf:$dryRun
        }
        Write-TaskCompleted
    }

    if ($yes.IsPresent -or (Read-Host 'Remove ''work'' directory? [y/N]') -eq 'y') {
        $workDir = Get-LocalPath 'work' 
        if (Test-Path $workDir) {
            Remove-Item $workDir -Force -Recurse -WhatIf:$dryRun
        }
        Write-TaskCompleted
    }

    if ($yes.IsPresent -or (Read-Host 'Remove the locally installed tools? [y/N]') -eq 'y') {
        $7zip = Get-7Zip
        if (Test-Path $7zip) {
            Remove-Item $7zip -WhatIf:$dryRun
        }
        $nuget = Get-NuGet
        if (Test-Path $nuget) {
            Remove-Item $nuget -WhatIf:$dryRun
        }
        Write-TaskCompleted
    }

    if ($yes.IsPresent -or (Read-Host 'Remove untracked files (unsafe)? [y/N]') -eq 'y') {
        $git = (Get-Git)

        if ($git -ne $null) {
            $git | Remove-UntrackedItems -WhatIf:$dryRun -v:$verbose
        }
        Write-TaskCompleted
    }

    Write-Host ''
}

if ($repair.IsPresent) {
    $repairMessage = @"
Ready to proceed to the repair tasks. You will be offered the following options:
- Scan the repository for any C# source files missing a copyright header then repair them. 
  *** This operation will stress the file system ****

"@

    Write-Header $repairMessage

    if ($yes.IsPresent -or (Read-Host 'Repair copyright headers? [y/N]') -eq 'y') {
        'samples', 'src', 'tests' | Repair-Copyright -WhatIf:$dryRun -v:$verbose
        Write-TaskCompleted
    }

    Write-Host ''
}

# ------------------------------------------------------------------------------
