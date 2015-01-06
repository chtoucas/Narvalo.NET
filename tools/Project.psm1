#Requires -Version 4.0
#Requires -Modules Helpers

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------
# Private variables
# ------------------------------------------------------------------------------

[string] $SCRIPT:RepositoryRoot = (Get-Item $PSScriptRoot).Parent.FullName

# ------------------------------------------------------------------------------
# Public methods
# ------------------------------------------------------------------------------

# .SYNOPSIS
# Get the path to the 7-Zip command.
#
# .OUTPUTS
# System.String. Get-7Zip returns a string that contains the path to the 7-Zip executable.
function Get-7Zip {
    [CmdletBinding()]
    param()
    
    Get-RepositoryPath 'tools', '7za.exe'
}

# .SYNOPSIS
# Get the path to the installed git command.
#
# .OUTPUTS
# System.String. Get-Git returns a string that contains the path to the git command 
# or $null if git is nowhere to be found.
function Get-Git {
    [CmdletBinding()]
    param()

    Write-Verbose 'Finding the installed git command.'

    $git = (Get-Command "git.exe" -CommandType Application -ErrorAction SilentlyContinue)

    if ($git -eq $null) { 
        Write-Warning 'git.exe could not be found in your PATH. Please ensure git is installed.'
        return $null
    } else {
        return $git.Path
    }
}

# .SYNOPSIS
# Get the last git commit hash.
#
# .PARAMETER Abbrev
# If present, finds the abbreviated commit hash.
#
# .PARAMETER Git
# Specifies the path to the Git executable.
#
# .INPUTS
# The path to the Git executable.
#
# .OUTPUTS
# System.String. Get-GitCommitHash returns a string that contains the git commit hash.
# 
# .NOTES
# If anything fails, returns an empty string.
function Get-GitCommitHash {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $Git,

        [switch] $Abbrev
    )

    Write-Verbose 'Getting the last git commit hash.'

    $fmt = ?: { $abbrev.IsPresent } { '%h' } { '%H' }

    $hash = ''

    try {
        Write-Debug 'Call git.exe log.'
        $hash = . $git log -1 --format="$fmt" 2>&1
    } catch {
        Write-Warning "Git command failed: $_"
    }

    $hash
}

# .SYNOPSIS
# Get the path to the NuGet command.
#
# .OUTPUTS
# System.String. Get-NuGet returns a string that contains the path to the NuGet executable.
function Get-NuGet {
    [CmdletBinding()]
    param()

    Get-RepositoryPath 'tools', 'nuget.exe'
}

# .SYNOPSIS
# Get the path to the PSake module.
#
# .PARAMETER NoVersion
# If present, do not include a version independent path.
#
# .OUTPUTS
# System.String. Get-PSakeModulePath returns a string that contains the path to the PSake module.
function Get-PSakeModulePath {
    [CmdletBinding()]
    param([switch] $NoVersion)
    
    if ($noVersion.IsPresent) {
        Get-RepositoryPath 'packages\psake\tools\psake.psm1'
    } else {
        (ls (Get-RepositoryPath 'packages\psake.*\tools\psake.psm1') | select -First 1)
    }
}

# .SYNOPSIS
# Combine the repository path and several parts.
#
# .PARAMETER PathList
# Specifies the elements to append to the repository path.
#
# .OUTPUTS
# System.String. Get-RepositoryPath returns a string that contains the resulting path.
function Get-RepositoryPath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [Alias('p')] [string[]] $PathList
    )
    
    Join-Path -Path $SCRIPT:RepositoryRoot -ChildPath (Join-PathList $pathList)
}

# .SYNOPSIS
# Return the path to the repository.
#
# .OUTPUTS
# System.String. Get-RepositoryRoot returns a string that contains the path
# to the repository.
function Get-RepositoryRoot {
    $SCRIPT:RepositoryRoot
}

# .SYNOPSIS
# Install 7-Zip if it is not already installed.
#
# .PARAMETER Force
# If present, override the previous installed 7-Zip if any.
#
# .OUTPUTS
# System.String. Install-7Zip returns a string that contains the path to the 7-Zip executable.
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

# .SYNOPSIS
# Install NuGet if it is not already installed.
#
# .PARAMETER Force
# If present, override the previous installed NuGet if any.
#
# .OUTPUTS
# System.String. Install-NuGet returns a string that contains the path to the NuGet executable.
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

# .SYNOPSIS
# Install PSake.
#
# .PARAMETER NuGet
# Specifies the path to the NuGet executable.
#
# .INPUTS
# The path to the NuGet executable.
# 
# .OUTPUTS
# System.String. Install-PSake returns a string that contains the path to the PSake module.
#
# .NOTES
# This function is rather slow to execute.
function Install-PSake {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $NuGet
    )
    
    echo 'Installing PSake.'
    
    try {
        Write-Debug 'Call nuget.exe install psake.'
        . $nuget install psake `
           -ExcludeVersion `
           -OutputDirectory (Get-RepositoryPath 'packages') `
           -ConfigFile (Get-RepositoryPath '.nuget', 'NuGet.Config') `
           -Verbosity quiet 2>&1
    } catch {
        throw "'nuget.exe install' failed: $_"
    }

    Get-PSakeModulePath -NoVersion
}

# .SYNOPSIS
# Restore packages.
#
# .PARAMETER NuGet
# Specifies the path to the NuGet executable.
#
# .INPUTS
# The path to the NuGet executable.
#
# .OUTPUTS
# None.
function Restore-SolutionPackages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $NuGet
    )
    
    echo 'Restoring solution packages.'
    
    try {
        Write-Debug 'Call nuget.exe restore.'
        . $nuget restore (Get-RepositoryPath '.nuget', 'packages.config') `
            -PackagesDirectory (Get-RepositoryPath 'packages') `
            -ConfigFile (Get-RepositoryPath '.nuget', 'NuGet.Config') `
            -Verbosity quiet 2>&1
    } catch {
        throw "'nuget.exe restore' failed: $_"
    }
}

# ------------------------------------------------------------------------------
# Private methods
# ------------------------------------------------------------------------------

function Join-PathList {
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [Alias('p')] [string[]] $PathList
    )

    $count = $pathList.Count - 1

    $result = $pathList[0]

    for ($i = 1; $i -le $count; $i++) { 
        $result = Join-Path $result $pathList[$i]
    }

    $result
}

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Get-7Zip,
    Get-Git, 
    Get-GitCommitHash, 
    Get-NuGet, 
    Get-PSakeModulePath,
    Get-RepositoryPath, 
    Get-RepositoryRoot,
    Install-7Zip,
    Install-NuGet, 
    Install-PSake, 
    Restore-SolutionPackages
