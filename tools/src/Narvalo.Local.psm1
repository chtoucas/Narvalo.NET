Set-StrictMode -Version Latest

New-Variable -Name LocalModulesPath `
    -Value (Join-Path (Get-Item $PSScriptRoot).Parent.FullName 'src') `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Path to the local modules.'

# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Import a local module.
.DESCRIPTION
    Import a local module into the global scope.
.PARAMETER Name
    Specifies the name of the module.
.PARAMETER Pristine
    If true, force re-import of the module into the current session.
.PARAMETER ArgumentList
    Specifies arguments that are passed to the module.
.INPUTS
    None.
.OUTPUTS
    System.Management.Automation.PSModuleInfo. Import-LocalModule returns
    the System.Management.Automation.PSModuleInfo object that represents the imported module.
.EXAMPLE
    $module = Import-LocalModule 'Narvalo.ProjectAutomation' -Args $PSScriptRoot
    Import the 'Narvalo.ProjectAutomation' module.
#>
function Import-LocalModule {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string] $Name,
        [Parameter(Mandatory = $false, Position = 1)] [bool] $Pristine = $false,
        [Parameter(Mandatory = $false, Position = 2)] [Alias('Args')] [PSObject[]] $ArgumentList
    )

    Write-Verbose "Importing local module '$name'."

    if ($pristine) {
        Write-Verbose "Pristine switch on, force unload of '$name'."
        Write-Debug "Unload the module '$name'."
        Get-Module $name | Remove-Module -Force
    }

    $module = (Get-Module -Name $name)

    if ($module -eq $null) {
        $moduleName = Join-Path $LocalModulesPath "$name\$name.psd1"

        Write-Debug "Import the module '$name' into the global scope."
        $module = Import-Module $moduleName -NoClobber -Global -PassThru -Args $ArgumentList
    }

    $module
}

# ------------------------------------------------------------------------------

Export-ModuleMember -Function Import-LocalModule

# ------------------------------------------------------------------------------
