#Requires -Version 3.0

# .SYNOPSIS
# Run the PSake build script.
#
# .PARAMETER TaskList
# The list of tasks to be executed.
#
# .PARAMETER Verbosity
# Specifies the amount of information displayed by MSBuild.
# You can use the following verbosity levels: 
#   q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]. 
#
# .PARAMETER Force
# If present, force re-import of custom modules into the current session.
#
# .PARAMETER Help
# If present, display help then exit.
#
# .PARAMETER Quiet
# If present, force MSBuild to run quietly.
#
# .PARAMETER Retail
# If present, packages/assemblies are built for retail.
#
# .INPUTS
# The task to be executed.
#
# .OUTPUTS
# None.
#
# .EXAMPLE
# make.ps1 -Verbosity detailed
# Default task with detailed informations.
#
# .EXAMPLE
# make.ps1 CodeAnalysis quiet
# Quiet run of the Code Analysis task.
#
# .EXAMPLE
# make.ps1 -r Package 
# Create retail packages with the default verbosity level.
#
# .LINK
# https://github.com/psake/psake

param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [Alias('t')] [string[]] $taskList = @(),

    [Parameter(Mandatory = $false, Position = 1)]
    [Alias('v')] [string] $verbosity = 'minimal',

    [Alias('r')] [switch] $retail,
    [Alias('h')] [switch] $help,
    [Alias('q')] [switch] $quiet,
    [Alias('f')] [switch] $force
)

Set-StrictMode -Version Latest

# Force MSBuild to run quietly?
if ($quiet) {
    $verbosity = 'quiet'
}

# Force re-import of the Project & PSake modules?
if ($force) {
    Get-Module Project | Remove-Module
    Get-Module psake | Remove-Module
}

# Import Narvalo module?
if (!(Get-Module Project)) {
    Join-Path $PSScriptRoot 'tools\Project.psm1' | Import-Module 
}

# Path to the PSake script.
$PSakefile = (Get-RepositoryPath 'PSakefile.ps1')

# Restore packages.
Install-NuGet | Restore-SolutionPackages

# Import PSake.
if (!(Get-Module psake)) {
    Get-PSakeModulePath | Import-Module
}

# Display help?
if ($help) {
    #Get-Help $MyInvocation.MyCommand.Path
    Write-Host ''
    Write-Host 'LIST OF AVAILABLE TASKS' -ForeGround Green
    Invoke-PSake $PSakefile -NoLogo -Docs
    Exit 0
}

# Display logo.
if ($retail) {
    $logo = 'Executing PSake (RETAIL mode)...'
} else {
    $logo = 'Executing PSake...'
}

Write-Host $logo -ForegroundColor Green

# Invoke PSake.
Invoke-PSake $PSakefile `
    -NoLogo `
    -TaskList $taskList `
    -Parameters @{ 'verbosity' = $verbosity; 'retail' = $retail }
