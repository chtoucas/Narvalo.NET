#Requires -Version 3.0

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------
# Private variables
# ------------------------------------------------------------------------------

[string] $script:RepositoryRoot = (Get-Item $PSScriptRoot).Parent.FullName

# ------------------------------------------------------------------------------
# Public methods
# ------------------------------------------------------------------------------

# .SYNOPSIS
# Remove untracked files from the working tree.
#
# .PARAMETER Directories
# If present, remove untracked directories in addition to untracked files.
#
# .PARAMETER Force
# If present, if the Git configuration variable clean.requireForce is not set 
# to false, git clean will refuse to run unless given -f, -n or -i.
#
# .PARAMETER DryRun
# Don't actually remove anything, just show what would be done.
#
# .LINK
# http://git-scm.com/docs/git-clean
# http://stackoverflow.com/questions/61212/remove-local-untracked-files-from-my-current-git-branch
function Clear-Repository {
    [CmdletBinding()]
    param(
        [Alias('d')] [switch] $directories,
        [Alias('f')] [switch] $force,
        [Alias('n')] [switch] $dryRun
    )

    $yn = Read-Host 'Beware this will permanently delete all untracked files. Are you sure [y/N]?'
    if ($yn -ne 'y') {
        Write-Host 'Cancelling on user request.'
        Exit 0
    }

    if (Get-Command "git.exe" -ErrorAction SilentlyContinue) { 
       Write-Host "Git found"
    } else {
       Write-Error "Git not found"
    }

    Write-Verbose 'Cleaning repository...'

    $opts = 'clean'
    if ($directories) { $opts = $opts, '-d' }
    if ($force) { $opts = $opts, '-f' }
    if ($dryRun) { $opts = $opts, '-n' }

    try {
        Push-Location $script:RepositoryRoot

        git.exe clean -d -n
    } catch {
        Exit-Error "Unabled to clean repository: $_"
    } finally {
        Pop-Location
    }
}

# .SYNOPSIS
# Exit with the error code 1.
#
# .PARAMETER Message
# The message to be written.
#
# .OUTPUTS
# None.
function Exit-Error {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $message
    )
    
    Write-Host ''
    Write-Host $message -BackgroundColor Red -ForegroundColor Yellow
    Write-Host ''
    Exit 1
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
    param([switch] $noVersion)

    if ($noVersion.IsPresent) {
        Get-RepositoryPath 'packages\psake\tools\psake.psm1'
    } else {
        (Get-ChildItem (Get-RepositoryPath 'packages\psake.*\tools\psake.psm1') | Select-Object -First 1)
    }
}

# .SYNOPSIS
# Combine the repository path and several parts.
#
# .PARAMETER Parts
# Specifies the elements to append to the repository path.
#
# .OUTPUTS
# System.String. Get-RepositoryPath returns a string that contains the resulting path.
function Get-RepositoryPath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string[]] $parts
    )

    Get-RepositoryRoot | Join-Path -ChildPath (Join-Multiple $parts)
}

# .SYNOPSIS
# Get the last git commit hash.
#
# .PARAMETER Abbrev
# If present, finds the abbreviated commit hash.
#
# .OUTPUTS
# System.String. Get-GitCommitHash returns a string that contains the git commit hash.
# 
# .NOTES
# If the git command fails, returns an empty string.
function Get-GitCommitHash {
    [CmdletBinding()]
    param([switch] $abbrev)

    if ($abbrev.IsPresent) {
        $fmt = '%h'
    } else {
        $fmt = '%H'
    }

    try {
        $hash = git.exe log -1 --format="$fmt"
    } catch {
        Write-Warning "Unabled to get the last git commit hash: $_"
        $hash = ''
    }

    $hash
}

# .SYNOPSIS
# Install 7-Zip.
#
# .OUTPUTS
# System.String. Install-7Zip returns a string that contains the path to the 7-Zip executable.
function Install-7Zip {
    [CmdletBinding()]
    param()
    
    Write-Verbose 'Installing 7-Zip...'
    
    $sevenZip = Get-RepositoryPath 'tools', '7za.exe'

    [System.Uri] 'http://narvalo.org/7z936.exe' | Download-Source -Path $sevenZip

    $sevenZip
}

# .SYNOPSIS
# Install NuGet.
#
# .OUTPUTS
# System.String. Install-7Zip returns a string that contains the path to the NuGet executable.
function Install-NuGet {
    [CmdletBinding()]
    param()
    
    Write-Verbose 'Installing NuGet...'
    
    $nuget = Get-RepositoryPath 'tools', 'NuGet.exe'

    [System.Uri] 'https://nuget.org/nuget.exe' | Download-Source -Path $nuget

    $nuget
}

# .SYNOPSIS
# Install PSake.
#
# .PARAMETER NuGet
# The path to the NuGet executable.
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
        [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)] [string] $nuget
    )

    Write-Verbose 'Installing PSake...'
    
    if ($nuget -eq $null) {
        $nuget = Install-NuGet
    }

    & $nuget install psake `
       -ExcludeVersion 
       -OutputDirectory (Get-RepositoryPath 'packages') `
       -ConfigFile (Get-RepositoryPath '.nuget', 'NuGet.Config') `
       -Verbosity quiet

    Get-PSakeModulePath -NoVersion
}

# .SYNOPSIS
# Restore packages.
#
# .PARAMETER NuGet
# The path to the NuGet executable.
#
# .INPUTS
# The path to the NuGet executable.
#
# .OUTPUTS
# None.
function Restore-SolutionPackages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)] [string] $nuget
    )

    Write-Verbose 'Restoring solution packages...'
    
    if ($nuget -eq $null) {
        $nuget = Install-NuGet
    }

    & $nuget restore (Get-RepositoryPath '.nuget', 'packages.config') `
        -PackagesDirectory (Get-RepositoryPath 'packages') `
        -ConfigFile (Get-RepositoryPath '.nuget', 'NuGet.Config') `
        -Verbosity quiet
}

# ------------------------------------------------------------------------------
# Private methods
# ------------------------------------------------------------------------------

function Download-Source {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [System.Uri] $source,
        [Parameter(Mandatory = $true, Position = 1)] [string] $path
    )

    if (!(Test-Path $path -PathType Leaf)) {
        Write-Verbose "Downloading $source..."

        Invoke-WebRequest $source -OutFile $path
    }
}

function Get-RepositoryRoot {
    $script:RepositoryRoot
}

function Join-Multiple {
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string[]] $paths
    )

    $count = $paths.Count - 1

    $result = $paths[0]

    for ($i = 1; $i -le $count; $i++) { 
        $result = Join-Path $result $paths[$i]
    }

    $result
}

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Clear-Repository,
    Exit-Error,
    Get-GitCommitHash, 
    Get-PSakeModulePath,
    Get-RepositoryPath, 
    Install-7Zip,
    Install-NuGet, 
    Install-PSake, 
    Restore-SolutionPackages
