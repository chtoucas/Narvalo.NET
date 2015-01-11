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
    [Alias('h')] [switch] $Help,
    [switch] $NoLogo,
    [switch] $Pristine
)

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

function Confirm-Continue {
    if ((Read-Host 'Are you sure you wish to continue? [y/N]') -ne 'y') {
        Write-Host "Cancelling on use request.`n" -ForeGround Green
        Exit 0
    }
}

# ------------------------------------------------------------------------------

if (!(Get-Module Narvalo.Local)) {
    Import-Module (Join-Path $PSScriptRoot 'Narvalo.Local.psm1')
}
$module = Import-LocalModule 'Narvalo.ProjectManagement' $pristine.IsPresent -Args (Get-Item $PSScriptRoot).Parent.FullName

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

# $WhatIfPreference
# $ConfirmPreference
# $confirm = $PSBoundParameters.ContainsKey('Confirm') `
#     -and [bool] $PSBoundParameters.Item('Confirm') -eq $true
 
$whatIf  = !$force.IsPresent       
   
if ($analyze.IsPresent) {
    Write-Host -ForeGround DarkCyan @"

Ready to proceed to the analysis tasks.

"@

    Invoke-AnalyzeTask -NoConfirm $yes.IsPresent -WhatIf:$whatIf
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

    Invoke-PurgeTask -NoConfirm $yes.IsPresent -WhatIf:$whatIf
}

if ($repair.IsPresent) {
    Write-Host -ForeGround DarkCyan @"

Ready to proceed to the repair tasks. You will be offered the following options:
- Scan the repository for any C# source files missing a copyright header then repair them. 
  *** This operation puts a lot of stress on the file system ****

"@

    Invoke-RepairTask -NoConfirm $yes.IsPresent -WhatIf:$whatIf
}

# ------------------------------------------------------------------------------
