Set-StrictMode -Version Latest

function Approve-ProjectRoot {
    param([Parameter(Mandatory = $true)] [string] $path) 

    if (![System.IO.Path]::IsPathRooted($path)) {
        throw 'When importing the ''Narvalo.Project'' module, you MUST specify an absolute path for the Narvalo.NET project repository.'
    }

    if (!(Test-Path $path)) {
        throw 'When importing the ''Narvalo.Project'' module, you MUST specify an existing directory for the Narvalo.NET project repository.'
    }

    return $path
}

if ($args.Length -ne 1) {
    throw 'When importing the ''Narvalo.Project'' module, you MUST specify the Narvalo.NET project repository, e.g. ''Import-Module Narvalo.Project -Args $projectRoot''.'
}

Set-Variable -Name ProjectRoot `
    -Value (Approve-ProjectRoot $args[0]) `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Path to the local repository for the Narvalo.NET project.'

#$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
#    Remove-Variable -Name ProjectRoot -Scope Script -Force
#}

# ------------------------------------------------------------------------------
# Public functions
# ------------------------------------------------------------------------------

<# 
.SYNOPSIS
    Exit with the error code 1.
.PARAMETER Message
    The message to be written.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Exit-Error {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $Message
    )
    
    Write-Host "`n", $message, "`n" -BackgroundColor Red -ForegroundColor Yellow
    Exit 1
}

<# 
.SYNOPSIS
    Get the path to the 7-Zip command.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-7Zip returns a string that contains the path to the 7-Zip executable.
#>
function Get-7Zip {
    [CmdletBinding()]
    param([switch] $Install)

    $sevenZip = Get-ProjectItem 'tools\7za.exe'

    if ($install.IsPresent -and !(Test-Path $sevenZip)) {
        Install-7Zip $sevenZip | Out-Null
    }

    return $sevenZip
}

<# 
.SYNOPSIS
    Get the path to the installed git command.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-Git returns a string that contains the path to the git command 
    or $null if git is nowhere to be found.
#>
function Get-Git {
    [CmdletBinding()]
    param()

    Write-Verbose 'Finding the installed git command.'

    $git = (Get-Command "git.exe" -CommandType Application -TotalCount 1 -ErrorAction SilentlyContinue)

    if ($git -eq $null) { 
        Write-Warning 'git.exe could not be found in your PATH. Please ensure git is installed.'
        return $null
    } else {
        return $git.Path
    }
}

<#
.SYNOPSIS
    Get the last git commit hash.
.PARAMETER Abbrev
    If present, finds the abbreviated commit hash.
.PARAMETER Git
    Specifies the path to the Git executable.
.INPUTS
    The path to the Git executable.
.OUTPUTS
    System.String. Get-GitCommitHash returns a string that contains the git commit hash.
.NOTES
    If anything fails, returns an empty string.
#>
function Get-GitCommitHash {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $Git,

        [switch] $Abbrev
    )

    Write-Verbose 'Getting the last git commit hash.'

    if ($abbrev.IsPresent) {
        $fmt = '%h'
    } else {
        $fmt = '%H'
    }

    $hash = ''

    try {
        Write-Debug 'Call git.exe log.'
        $hash = . $git log -1 --format="$fmt" 2>&1
    } catch {
        Write-Warning "Git command failed: $_"
    }

    $hash
}

<#
.SYNOPSIS
    Get the path to the NuGet command.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-NuGet returns a string that contains the path to the NuGet executable.
#>
function Get-NuGet {
    [CmdletBinding()]
    param([switch] $Install)

    $nuget = Get-ProjectItem 'tools\nuget.exe'

    if ($install.IsPresent -and !(Test-Path $nuget)) {
        Install-NuGet $nuget | Out-Null
    }

    $nuget
}

<#
.SYNOPSIS
    Combine the repository path and several parts.
.PARAMETER PathList
    Specifies the elements to append to the repository path.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-ProjectItem returns a string that contains the resulting path.
#>
function Get-ProjectItem {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [AllowEmptyString()]
        [Alias('p')] [string] $Path,

        [switch] $Resolve
    )

    if ($path -eq '') {
       $ProjectRoot
    } else {
        Join-Path -Path $ProjectRoot -ChildPath $path -Resolve:$resolve.IsPresent
    }
}

<#
.SYNOPSIS
    Get the path to the PSake module.
.PARAMETER NoVersion
    If present, do not include a version independent path.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-PSakeModulePath returns a string that contains the path to the PSake module.
#>
function Get-PSakeModulePath {
    [CmdletBinding()]
    param([switch] $NoVersion)
    
    if ($noVersion.IsPresent) {
        Get-ProjectItem 'packages\psake\tools\psake.psm1'
    } else {
        (ls (Get-ProjectItem 'packages\psake.*\tools\psake.psm1') | select -First 1)
    }
}

<#
.SYNOPSIS
    Install 7-Zip if it is not already installed.
.PARAMETER Force
    If present, override the previous installed 7-Zip if any.
.INPUTS
    None.
.OUTPUTS
    System.String. Install-7Zip returns a string that contains the path to the 7-Zip executable.
#>
function Install-7Zip {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('o')] [string] $OutFile,

        [switch] $Force
    )

    [System.Uri] $uri = 'http://narvalo.org/7z936.exe'
    $uri | Install-WebResource -Name '7-Zip' -o $outFile -Force:$force
}

<#
.SYNOPSIS
    Install NuGet if it is not already installed.
.PARAMETER Force
    If present, override the previous installed NuGet if any.
.INPUTS
    None.
.OUTPUTS
    System.String. Install-NuGet returns a string that contains the path to the NuGet executable.
#>
function Install-NuGet {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('o')] [string] $OutFile,

        [switch] $Force
    )

    [System.Uri] $uri = 'https://nuget.org/nuget.exe'
    $uri | Install-WebResource -Name 'NuGet' -o $outFile -Force:$force
}

<#
.SYNOPSIS
    Install PSake.
.PARAMETER NuGet
    Specifies the path to the NuGet executable.
.INPUTS
    The path to the NuGet executable.
.OUTPUTS
    System.String. Install-PSake returns a string that contains the path to the PSake module.
.NOTES
    This function is rather slow to execute.
#>
function Install-PSake {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $NuGet
    )
    
    Write-Verbose 'Installing PSake.'
    
    try {
        Write-Debug 'Call nuget.exe install psake.'
        . $nuget install psake `
           -ExcludeVersion `
           -OutputDirectory (Get-ProjectItem 'packages') `
           -ConfigFile (Get-ProjectItem '.nuget\NuGet.Config') `
           -Verbosity quiet 2>&1
    } catch {
        throw "'nuget.exe install' failed: $_"
    }

    Get-PSakeModulePath -NoVersion
}

<#
.SYNOPSIS
    Restore packages.
.PARAMETER NuGet
    Specifies the path to the NuGet executable.
.INPUTS
    The path to the NuGet executable.
.OUTPUTS
    None.
#>
function Restore-SolutionPackages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $NuGet
    )
    
    Write-Verbose 'Restoring solution packages.'
    
    try {
        Write-Debug 'Call nuget.exe restore.'
        . $nuget restore (Get-ProjectItem '.nuget\packages.config') `
            -PackagesDirectory (Get-ProjectItem 'packages') `
            -ConfigFile (Get-ProjectItem '.nuget\NuGet.Config') `
            -Verbosity quiet 2>&1
    } catch {
        throw "'nuget.exe restore' failed: $_"
    }
}

<#
.SYNOPSIS
    Stop any running MSBuild process.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Stop-AnyMSBuildProcess {
    Write-Debug 'Ensure there is no concurrent MSBuild running.'
    Get-Process -Name 'msbuild' -ErrorAction SilentlyContinue | %{ Stop-Process $_.ID -Force }
}

# ------------------------------------------------------------------------------
# Private functions
# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Install a web resource if it is not already installed.
.PARAMETER Force
    If present, override the previous installed resource if any.
.OUTPUTS
    System.String. Install-WebResource returns a string that contains the path to the installed resource.
#>
function Install-WebResource {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)] 
        [System.Uri] $Uri,

        [Parameter(Mandatory = $true, Position = 1, ValueFromPipeline = $true)] 
        [Alias('o')] [string] $OutFile,

        [Parameter(Mandatory = $true, Position = 2)] 
        [string] $Name,

        [switch] $Force
    )
    
    if (!$force -and (Test-Path $outFile -PathType Leaf)) {
        Write-Verbose "$name is already installed."
    } else {
        Write-Verbose "Installing $name."

        try {
            Write-Debug "Download $uri to $outFile."
            Invoke-WebRequest $uri -OutFile $outFile
        } catch {
            throw "Unabled to download $name."
        }
    }

    $outFile
}

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember `
    -Function `
        Exit-Error,
        Get-7Zip,
        Get-Git, 
        Get-GitCommitHash, 
        Get-NuGet, 
        Get-ProjectItem,
        Get-PSakeModulePath,
        Install-7Zip,
        Install-NuGet, 
        Install-PSake,
        Restore-SolutionPackages,
        Stop-AnyMSBuildProcess
