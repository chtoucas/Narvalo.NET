#Requires -Version 3.0

# .SYNOPSIS
# Run the PSake build script.
#
# .PARAMETER Task
# The task to be executed.
#
# .PARAMETER Verbosity
# Specifies the amount of information displayed by MSBuild.
# You can specify the following verbosity levels: 
#   q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]. 
#
# .PARAMETER Reload
# If present, force re-import of custom modules into the current session.
#
# .INPUTS
# The task to be executed.
#
# .OUTPUTS
# None.
#
# .EXAMPLE
# make.ps1 -Verbosity detailed
# Default task with detailed informations: 
#
# .EXAMPLE
# make.ps1 CodeAnalysis quiet
# Quiet run of the Code Analysis task:
#
# .EXAMPLE
# make.ps1 Retail
# Create retail packages with the default verbosity level:
#
# .NOTES
# Contrary to PSake, you can not specify a task list.
#
# .LINK
# https://github.com/psake/psake

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)]
    [string] $task = 'default',
    [Parameter(Mandatory = $false, Position = 1)]
    [string] $verbosity = 'minimal',
    [switch] $reload
)

Set-StrictMode -Version Latest

# Force reload of the Narvalo & PSake modules.
if ($Reload) {
    Get-Module Narvalo | Remove-Module
    Get-Module psake | Remove-Module
}

# Import Narvalo module.
if (!(Get-Module Narvalo)) {
    Join-Path $PSScriptRoot 'tools\Narvalo.psm1' | Import-Module 
}

# Restore packages.
Install-NuGet | Restore-SolutionPackages

# Import PSake.
if (!(Get-Module psake)) {
    Get-PSakeModulePath | Import-Module
}

Invoke-PSake (Get-RepositoryPath 'PSakefile.ps1') $task -Parameters @{ 'verbosity' = $verbosity; }
