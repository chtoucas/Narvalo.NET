# ------------------------------------------------------------------------------
# Public functions
# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Convert a MSBuild verbosity level to a NuGet verbosity level.
.PARAMETER Verbosity
    Specifies the MSBuild verbosity.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function ConvertTo-NuGetVerbosity {
    param([Parameter(Mandatory = $true, Position = 0)] [string] $Verbosity)

    switch ($verbosity) {
        'q'          { return 'quiet' }
        'quiet'      { return 'quiet' }
        'm'          { return 'normal' }
        'minimal'    { return 'normal' }
        'n'          { return 'normal' }
        'normal'     { return 'normal' }
        'd'          { return 'detailed' }
        'detailed'   { return 'detailed' }
        'diag'       { return 'detailed' }
        'diagnostic' { return 'detailed' }

        default      { return 'normal' }
    }
}

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
    $uri | _Install-RemoteItem -Name 'NuGet' -o $outFile -Force:$force.IsPresent
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
    Install a web resource if it is not already installed.
.PARAMETER Force
    If present, override the previous installed resource if any.
.OUTPUTS
    None.
#>
function _Install-RemoteItem {
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
