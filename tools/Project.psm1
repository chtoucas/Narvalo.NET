#Requires -Version 3.0

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------
# Private variables
# ------------------------------------------------------------------------------

[string] $script:RepositoryRoot = (Get-Item $PSScriptRoot).Parent.FullName
[string] $script:copyrightHeader = '// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.'

# ------------------------------------------------------------------------------
# Public methods
# ------------------------------------------------------------------------------

# .SYNOPSIS
# Remove untracked files from the working tree.
#
# .PARAMETER DryRun
# Don't actually remove anything, just show what would be done.
#
# .NOTES
# We do not remove ignored files (see -x and -X options from git).
# 
# .LINK
# http://git-scm.com/docs/git-clean
function Clear-Repository {
    [CmdletBinding()]
    param(
        [Alias('n')] [switch] $dryRun
    )

    $yn = Read-Host 'Beware this will permanently delete all untracked files. Are you sure [y/N]?'
    if ($yn -ne 'y') {
        # NB: This is case insensitive.
        Write-Host 'Cancelling on user request.'
        Exit 0
    }

    if (!(Assert-GitAvailable)) { return }

    Write-Verbose 'Cleaning repository...'

    try {
        Push-Location $script:RepositoryRoot

        # We ignore folders named 'Internal' (some of them only contain linked files).
        if ($dryRun) {
            git.exe clean -d -n -e 'Internal'
        } else {
            # Force cleanup, ie ignore clean.requireForce
            git.exe clean -d -f -e 'Internal'
        }
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
# Find csharp files without copyright header.
#
# .PARAMETER Directory
# The directory to traverse.
#
# .OUTPUTS
# None.
function Find-FilesWithoutCopyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('d')] [string] $directory
    )

    # Find all csharp source files, ignoring designer generated files.
    Get-ChildItem $directory -Recurse -Filter *.cs -Exclude *.Designer.cs |
        # Ignore temporary build directories.
        ? { -not ($_.FullName.Contains('bin\') -or $_.FullName.Contains('obj\')) } | 
        ? { Assert-MissingCopyright $_.FullName } | 
        select FullName
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
        (Get-ChildItem (Get-RepositoryPath 'packages\psake.*\tools\psake.psm1') | select -First 1)
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
    
    if (!(Assert-GitAvailable)) { return }

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
# Add a copyright header to all csharp files missing one.
#
# .PARAMETER Directory
# The directory to traverse.
#
# .OUTPUTS
# None.
function Repair-FilesWithoutCopyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('d')] [string] $directory
    )

    Find-FilesWithoutCopyright $directory | % { Repair-MissingCopyright $_.FullName }
}

# .SYNOPSIS
# Add a copyright header to a file.
#
# .PARAMETER Path
# The file to repair.
#
# .OUTPUTS
# None.
function Repair-MissingCopyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [Alias('p')] [string] $path
    )

    Write-Host "Adding copyright header to $path..."

    $lines = New-Object System.Collections.ArrayList(, (Get-Content -LiteralPath $path))
    $lines.Insert(0, '')
    $lines.Insert(0, $script:copyrightHeader)
    $lines | Set-Content -LiteralPath $path -Encoding UTF8
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

function Assert-GitAvailable {
    if (!(Get-Command "git.exe" -CommandType Application -ErrorAction SilentlyContinue)) { 
       Write-Warning 'This function requires the git command to be in your PATH.'
       return $false
    }

    return $true
}

function Assert-MissingCopyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string] $path
    )

    Write-Verbose "Processing $path..."

    $line = Get-Content -LiteralPath $path -totalCount 1;

    return !$line -or !$line.StartsWith("// Copyright")
}

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
    Find-FilesWithoutCopyright,
    Get-GitCommitHash, 
    Get-PSakeModulePath,
    Get-RepositoryPath, 
    Install-7Zip,
    Install-NuGet, 
    Install-PSake, 
    Repair-FilesWithoutCopyright,
    Repair-MissingCopyright,
    Restore-SolutionPackages
