#Requires -Version 3.0

# Samples:
# - Default task with detailed informations: 
#   make.ps1 -verbosity detailed
# - Quiet run of the Code Analysis task:
#   make.ps1 CodeAnalysis quiet
# - Create retail packages, default verbosity level:
#   make.ps1 Retail

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false, Position = 0)]
    [string] $task = 'default',
    [Parameter(Mandatory = $false, Position = 1)]
    [string] $verbosity = 'minimal'
)

Set-StrictMode -Version Latest

Get-Module psake | Remove-Module
Import-Module (Get-ChildItem "$PSScriptRoot\packages\psake.*\tools\psake.psm1" | Select-Object -First 1)

Invoke-psake "$PSScriptRoot\PSakefile.ps1" $task -parameters @{ 'verbosity' = $verbosity; }
