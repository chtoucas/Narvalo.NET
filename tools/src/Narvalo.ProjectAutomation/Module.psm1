<#
.SYNOPSIS
    PowerShell module to manage the project Narvalo.NET.
.DESCRIPTION
    Provides helpers to automate various development tasks:
    - Install or uninstall external tools.
    - Fix the most common problems.
    - Clean up the repository.
    - Helpers for the build script.
#>

Set-StrictMode -Version Latest

<#
.SYNOPSIS
    Validate a path for use as a project root.
.DESCRIPTION
    The Approve-ProjectRoot cmdlet validates that the path exists and is absolute.
.PARAMETER Path
    Specifies a path to be approved.
.OUTPUTS
    System.String. Approve-ProjectRoot returns a string that contains the approved path.
#>
function Approve-ProjectRoot {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)] [string] $Path)

    if (![System.IO.Path]::IsPathRooted($path)) {
        throw 'When importing the ''Narvalo.ProjectAutomation'' module, ',
            'you MUST specify an absolute path for the Narvalo.NET project repository.'
    }

    if (!(Test-Path $path)) {
        throw 'When importing the ''Narvalo.ProjectAutomation'' module,',
            'you MUST specify an existing directory for the Narvalo.NET project repository.'
    }

    return $path
}

if ($args.Length -ne 1) {
    throw 'When importing the ''Narvalo.ProjectAutomation'' module,',
        'you MUST specify the Narvalo.NET project repository,',
        'e.g. ''Import-Module Narvalo.ProjectAutomation -Args $projectRoot''.'
}

New-Variable -Name ProjectRoot `
    -Value (Approve-ProjectRoot $args[0]) `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Path to the local repository for the project Narvalo.NET.'

New-Variable -Name DefaultNuGetVerbosity `
    -Value 'normal' `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Default NuGet verbosity.'

# ------------------------------------------------------------------------------
# Public functions
# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Exit current process gracefully.
.DESCRIPTION
    Depending on the specified error code, display a colorful message for success
    or failure then exit the current process.
.PARAMETER ExitCode
    Specifies the exit code.
.PARAMETER Message
    Specifies the message to be written to the host.
.INPUTS
    The message to be written to the host.
.OUTPUTS
    None.
#>
function Exit-Gracefully {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Message,

        [Parameter(Mandatory = $false, Position = 1)]
        [int] $ExitCode = 0
    )

    if ($exitCode -eq 0) {
        $backgroundColor = 'DarkGreen'
    } else {
        $backgroundColor = 'Red'
    }

    Write-Host $message -BackgroundColor $backgroundColor -ForegroundColor Yellow

    Exit $exitCode
}

<#
.SYNOPSIS
    Get the path to the locally installed 7-Zip command.
.DESCRIPTION
    Get the path to the locally installed 7-Zip command.
    Optionally install the 7-Zip commmand in the local repository.
.PARAMETER Install
    If present and the command does not exist, install 7-Zip.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-7Zip returns a string that contains the path to the 7-Zip executable.
#>
function Get-7Zip {
    [CmdletBinding()]
    param([switch] $Install)

    $sevenZip = Get-LocalPath 'tools\7za.exe'

    if ($install.IsPresent -and !(Test-Path $sevenZip)) {
        Install-7Zip $sevenZip
    }

    return $sevenZip
}

<#
.SYNOPSIS
    Get the path to the system git command.
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
    Get the last git commit hash of the local repository.
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
        Push-Location $script:ProjectRoot

        Write-Debug 'Call git.exe log.'
        $hash = . $git log -1 --format="$fmt" 2>&1
    } catch {
        Write-Warning "Git command failed: $_"
    } finally {
        Pop-Location
    }

    $hash
}

<#
.SYNOPSIS
    Get the git status.
.PARAMETER Git
    Specifies the path to the Git executable.
.PARAMETER Short
    If present, use the short-format.
.INPUTS
    The path to the Git executable.
.OUTPUTS
    System.String. Get-GitStatus returns a string that contains the git status.
.NOTES
    If anything fails, returns $null.
#>
function Get-GitStatus {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Git,

        [switch] $Short
    )

    Write-Verbose 'Getting the git status.'

    if ($short.IsPresent) {
        $opts = '-s'
    } else {
        $opts = ''
    }

    $status = $null

    try {
        Push-Location $script:ProjectRoot

        Write-Debug 'Call git.exe status.'
        $status = . $git status $opts 2>&1

        if ($status -eq $null) {
            $status = ''
        }
    } catch {
        Write-Warning "Git command failed: $_"
    } finally {
        Pop-Location
    }

    $status
}

<#
.SYNOPSIS
    Combine the repository path and a child path.
.PARAMETER Path
    Specifies the path to append to the value of the repository path.
.PARAMETER Resolve
    If present, attempt to resolve the resulting path.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-LocalPath returns a string that contains the resulting path.
#>
function Get-LocalPath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [AllowEmptyString()]
        [Alias('p')] [string] $Path,

        [switch] $Resolve
    )

    if ($path -eq '') {
       $script:ProjectRoot
    } else {
        Join-Path -Path $script:ProjectRoot -ChildPath $path -Resolve:$resolve.IsPresent
    }
}

<#
.SYNOPSIS
    Get the path to the locally installed NuGet command.
.DESCRIPTION
    Get the path to the locally installed NuGet command.
    Optionally install the NuGet commmand in the local repository.
.PARAMETER Install
    If present and the command does not exist, install NuGet.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-NuGet returns a string that contains the path to the NuGet executable.
#>
function Get-NuGet {
    [CmdletBinding()]
    param([switch] $Install)

    $nuget = Get-LocalPath 'tools\nuget.exe'

    if ($install.IsPresent -and !(Test-Path $nuget)) {
        Install-NuGet $nuget
    }

    $nuget
}

<#
.SYNOPSIS
    Get the path to the PSake module.
.PARAMETER NoVersion
    If present, do not include a version agnostic path.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-PSakeModulePath returns a string that contains the path
    to the PSake module.
#>
function Get-PSakeModulePath {
    [CmdletBinding()]
    param([switch] $NoVersion)

    if ($noVersion.IsPresent) {
        Get-LocalPath 'packages\psake\tools\psake.psm1'
    } else {
        (ls (Get-LocalPath 'packages\psake.*\tools\psake.psm1') | select -First 1)
    }
}

<#
.SYNOPSIS
    Install 7-Zip.
.PARAMETER Force
    If present, override any previously installed 7-Zip.
.PARAMETER OutFile
    Specifies the path where 7-Zip will be installed.
.INPUTS
    The path where 7-Zip will be installed.
.OUTPUTS
    None.
#>
function Install-7Zip {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('o')] [string] $OutFile,

        [switch] $Force
    )

    [System.Uri] $uri = 'http://narvalo.org/7z936.exe'
    $uri | Install-RemoteItem -Name '7-Zip' -o $outFile -Force:$force.IsPresent
}

<#
.SYNOPSIS
    Install NuGet.
.PARAMETER Force
    If present, override any previously installed NuGet.
.PARAMETER OutFile
    Specifies the path where NuGet will be installed.
.INPUTS
    The path where NuGet will be installed.
.OUTPUTS
    None.
#>
function Install-NuGet {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('o')] [string] $OutFile,

        [switch] $Force
    )

    [System.Uri] $uri = 'https://nuget.org/nuget.exe'
    $uri | Install-RemoteItem -Name 'NuGet' -o $outFile -Force:$force.IsPresent
}

<#
.SYNOPSIS
    Install PSake.
.PARAMETER Verbosity
    Specifies the verbosity level for the underlying NuGet command-line.
.INPUTS
    None.
.OUTPUTS
    System.String. Install-PSake returns a string that contains the path to the PSake module.
.NOTES
    This function is rather slow to execute.
#>
function Install-PSake {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false, Position = 0)]
        [ValidateSet('quiet', 'normal', 'detailed')]
        [string] $Verbosity = $script:DefaultNuGetVerbosity
    )

    Write-Verbose 'Installing PSake.'

    $nuget = Get-NuGet -Install

    try {
        Write-Debug 'Call nuget.exe install psake.'
        . $nuget install psake `
           -ExcludeVersion `
           -OutputDirectory (Get-LocalPath 'packages') `
           -ConfigFile (Get-LocalPath '.nuget\NuGet.Config') `
           -Verbosity $verbosity 2>&1
    } catch {
        throw "'nuget.exe install' failed: $_"
    }

    Get-PSakeModulePath -NoVersion
}

<#
.SYNOPSIS
    Restore packages.
.PARAMETER ConfigFile
    Specifies the path to the NuGet config.
.PARAMETER PackagesDirectory
    Specifies the path to the packages directory.
.PARAMETER Source
    Specifies the list of packages sources to use.
.PARAMETER Verbosity
    Specifies the verbosity level for the underlying NuGet command-line.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Restore-Packages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [Alias('s')] [string] $Source,

        [Parameter(Mandatory = $true, Position = 1)]
        [string] $PackagesDirectory,

        [Parameter(Mandatory = $true, Position = 2)]
        [string] $ConfigFile,

        [Parameter(Mandatory = $false, Position = 3)]
        [ValidateSet('', 'quiet', 'normal', 'detailed')]
        [string] $Verbosity
    )

    if ($verbosity -eq '') {
        $verbosity = $script:DefaultNuGetVerbosity
    }

    $nuget = Get-NuGet -Install

    try {
        Write-Debug 'Call nuget.exe restore.'
        . $nuget restore $source `
            -PackagesDirectory $packagesDirectory `
            -ConfigFile $configFile `
            -Verbosity $verbosity 2>&1
    } catch {
        throw "'nuget.exe restore' failed: $_"
    }
}

<#
.SYNOPSIS
    Restore solution packages.
.PARAMETER Verbosity
    Specifies the verbosity level for the underlying NuGet command-line.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Restore-SolutionPackages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false, Position = 0)]
        [string] $Verbosity
    )

    Write-Verbose 'Restoring solution packages.'

    Restore-Packages -Source (Get-LocalPath '.nuget\packages.config') `
        -PackagesDirectory (Get-LocalPath 'packages') `
        -ConfigFile (Get-LocalPath '.nuget\NuGet.Config') `
        -Verbosity $verbosity
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
    [CmdletBinding()]
    param()

    Write-Debug 'Stop any concurrent MSBuild running.'
    Get-Process | ?{ $_.ProcessName -eq 'msbuild' } | %{ Stop-Process $_.ID -Force }
}

# ------------------------------------------------------------------------------
# Private functions
# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Requests confirmation from the user.
.PARAMETER Query
    Specifies the query to be displayed to the user.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Confirm-Yes {
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [string] $Query
    )

    while ($true) {
        $answer = (Read-Host $query, '[y/N]')

        if ($answer -eq '' -or $answer -eq 'n') {
            return $false
        } elseif ($answer -eq 'y') {
            return $true
        }
    }
}

<#
.SYNOPSIS
    Install a web resource if it is not already installed.
.PARAMETER Force
    If present, override the previous installed resource if any.
.OUTPUTS
    None.
#>
function Install-RemoteItem {
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
        Write-Verbose "Installing ${name}."

        # We could use
        #   Invoke-WebRequest $uri -OutFile $outFile
        # but it displays a very ugly progress bar.
        try {
            Write-Debug "Download $uri to $outFile."
            $webClient = New-Object System.Net.WebClient
            $webClient.DownloadFile($uri, $outFile)
        } catch {
            throw "Unabled to download ${name}: $_"
        }
    }
}

# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Exit-Gracefully,
    Get-7Zip,
    Get-Git,
    Get-GitCommitHash,
    Get-GitStatus,
    Get-LocalPath,
    Get-NuGet,
    Get-PSakeModulePath,
    Install-7Zip,
    Install-NuGet,
    Install-PSake,
    Restore-Packages,
    Restore-SolutionPackages,
    Stop-AnyMSBuildProcess

# ------------------------------------------------------------------------------
