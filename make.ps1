#Requires -Version 3.0

param($task = 'default')

Set-StrictMode -Version Latest

Get-Module psake | Remove-Module
Import-Module (Get-ChildItem "$PSScriptRoot\packages\psake.*\tools\psake.psm1" | Select-Object -First 1)

Invoke-psake "$PSScriptRoot\PSakefile.ps1" $task