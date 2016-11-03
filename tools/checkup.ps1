#Requires -Version 4.0

<#
.SYNOPSIS
    Perform maintenance tasks.
.PARAMETER Analyze
    If present, process analysis tasks.
.PARAMETER Force
    If present, do the actual work, otherwise only display what would have been done.
.PARAMETER Help
    If present, display the help then exit.
.PARAMETER NoLogo
    If present, don't display the startup banner.
.PARAMETER Pristine
    If present, force re-import of local modules into the current session.
.PARAMETER Purge
    If present, process cleanup tasks.
.PARAMETER Repair
    If present, process repair tasks.
.PARAMETER Yes
    If present, do not ask for any confirmation, except for safety warnings.
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
    It is fairly safe to run this script. By default it does NOT modify anything
    and only tells you what might be done.
    The only truly unsafe parameter is -Yes used in conjunction with -Force.
#>

[CmdletBinding()]
param(
    [switch] $Analyze,
    [switch] $Purge,
    [Alias('fix')] [switch] $Repair,

    [Alias('f')] [switch] $Force,
    [Alias('y')] [switch] $Yes,
    [Alias('h')] [switch] $Help,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

trap {
    Write-Host ('An unexpected error occured: {0}' -f $_.Exception.Message) `
        -BackgroundColor Red -ForegroundColor Yellow

    Exit 1
}

# ------------------------------------------------------------------------------

function Confirm-Continue {
    while ($true) {
        $answer = Read-Host 'Are you sure you wish to continue? [y/N]'

        if ($answer -eq '' -or $answer -eq 'n') {
            Write-Host "Stopping on user request.`n"
            Exit 0
        } elseif ($answer -eq 'y') {
            break
        }
    }
}

# ------------------------------------------------------------------------------

if (!(Get-Module Narvalo.Local)) {
    Import-Module (Join-Path $PSScriptRoot 'src\Narvalo.Local.psm1')
}
$module = Import-LocalModule 'Narvalo.ProjectAutomation' $pristine.IsPresent -Args (Get-Item $PSScriptRoot).Parent.FullName

if (!$noLogo.IsPresent) {
    $version = $module.Version

    if ($force.IsPresent) {
        Write-Host "Checkup, version $version. Copyright (c) Narvalo.Org.`n"
    } else {
        Write-Host "Checkup (DRY RUN), version $version. Copyright (c) Narvalo.Org.`n"
    }
}

if ($help.IsPresent) {
    Get-Help $MyInvocation.MyCommand.Path -Full
    Exit 0
}

if (!$analyze.IsPresent -and !$purge.IsPresent -and !$repair.IsPresent) {
    Exit-Gracefully -ExitCode 1 `
        'You should at least set one of the following switches: ''-Analyze'' or ''-Purge'' or ''-Repair''.'
}

if ($force.IsPresent) {
    Write-Warning '''Force'' mode on, any modification will be permanent.'
    Confirm-Continue

    if ($yes.IsPresent) {
        Write-Warning '''Yes'' mode on, ALL tasks will be processed WITHOUT any prior confirmation.'
        Confirm-Continue
    }
} else {
    Write-Warning 'Safe mode on, no modifications to the repository will happen. Use the option ''-Force'' when you are ready.'
}

$whatIf  = !$force.IsPresent

if ($analyze.IsPresent) {
    Write-Host -ForeGround DarkCyan @"

Ready to proceed to the analysis tasks. You will be offered the following options:
- Scan the C# project files for files ignored by StyleCop.

"@

    Invoke-AnalyzeTask -NoConfirm:$yes.IsPresent -WhatIf:$whatIf
}

if ($purge.IsPresent) {
    Write-Host -ForeGround DarkCyan @"

Ready to proceed to the cleanup tasks. You will be offered the following options:
- Remove 'bin' and 'obj' directories created by Visual Studio.
- Remove the 'packages' directory used by NuGet.
- Remove the 'work' directory created by the build script.
- Remove files untracked by git.
- Remove the locally installed tools: 'tools\nuget.exe' and 'tools\7za.exe'.

"@

    Invoke-PurgeTask -NoConfirm:$yes.IsPresent -WhatIf:$whatIf
}

if ($repair.IsPresent) {
    Write-Host -ForeGround DarkCyan @"

Ready to proceed to the repair tasks. You will be offered the following options:
- Scan the repository for any C# source files missing a copyright header then repair them.
  *** This operation puts a lot of stress on the file system ****

"@

    Invoke-RepairTask -NoConfirm:$yes.IsPresent -WhatIf:$whatIf
}

# ------------------------------------------------------------------------------
