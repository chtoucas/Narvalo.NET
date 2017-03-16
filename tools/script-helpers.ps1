# This file requires that $script:ProjectRoot is correctly initialized.

New-Variable -Name ProjectRoot `
    -Value (Join-Path $PSScriptRoot '..') `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Path to the local repository for the project Narvalo.NET.'

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
        [int] $ExitCode = 1
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
    Get the path for a package tool.
.OUTPUTS
    System.String. Find-PkgTool returns a string that contains the path to the tool.
#>
function Find-PkgTool {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Directory,

        [Parameter(Mandatory = $true, Position = 1)]
        [string] $Pkg,

        [Parameter(Mandatory = $true, Position = 2)]
        [string] $Tool
    )

    $matches = @($Directory |
        Get-ChildItem -Directory |
        Where-Object { $_.FullName -match $pkg })

    if ($matches.Count -eq 0) { Exit-Gracefully "No package found matching $pkg." }
    if ($matches.Count -ne 1) { Exit-Gracefully "More than one package found matching $pkg"}

    return $matches | ForEach-Object { Join-Path -Path $_.FullName -ChildPath $tool -Resolve }
}

function Get-DocFXExe {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Directory
    )

    return $Directory | Find-PkgTool -Pkg 'docfx.console.*' -Tool 'tools\docfx.exe'
}

<#
.SYNOPSIS
    Get the path to the system git command.
.INPUTS
    None.
.OUTPUTS
    System.String. Get-GitExe returns a string that contains the path to the git command
    or $null if git is nowhere to be found.
#>
function Get-GitExe {
    [CmdletBinding()]
    param()

    Write-Verbose 'Finding the installed git command.'

    $git = (Get-Command "git.exe" -CommandType Application -TotalCount 1 -ErrorAction SilentlyContinue)

    if ($git -eq $null) {
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
    Get the path to the locally installed MSBuild command.
.OUTPUTS
    System.String. Get-MSBuildExe returns a string that contains the path to the MSBuild executable.
#>
function Get-MSBuildExe {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $VSWhere
    )

    # See https://github.com/Microsoft/vswhere/wiki/Find-MSBuild
    # Apparently, the documentation (or vswhere) is wrong, it gives us the path to VS not MSBuild.
    $VisualStudio = . $VSWhere -latest -products * `
        -requires Microsoft.Component.MSBuild -property installationPath
    $MSBuild = Join-Path $VisualStudio 'MSBuild\15.0\Bin\MSBuild.exe'
    if (!(Test-Path $MSBuild)) {
        Exit-Gracefully 'Unable to locate MSBuild' `
    }

    return $MSBuild
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
    System.String. Get-NuGetExe returns a string that contains the path to the NuGet executable.
#>
function Get-NuGetExe {
    [CmdletBinding()]
    param([switch] $Install)

    $nuget = Get-LocalPath 'tools\nuget.exe'

    if ($install.IsPresent -and !(Test-Path $nuget)) {
        Install-NuGet $nuget
    }

    $nuget
}

function Get-OpenCoverExe {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Directory
    )

    return $Directory | Find-PkgTool -Pkg 'OpenCover.*' -Tool 'tools\OpenCover.Console.exe'
}

function Get-ReportGeneratorExe {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Directory
    )

    return $Directory | Find-PkgTool -Pkg 'ReportGenerator.*' -Tool 'tools\ReportGenerator.exe'
}

function Get-VSWhereExe {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Directory
    )

    return $Directory | Find-PkgTool -Pkg 'vswhere.*' -Tool 'tools\vswhere.exe'
}

function Get-XunitExe {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string] $Directory
    )

    return $Directory | Find-PkgTool -Pkg 'xunit.runner.console.*' -Tool 'tools\xunit.console.exe'
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

<#
.SYNOPSIS
    Restore packages.
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

        [Parameter(Mandatory = $false, Position = 3)]
        [ValidateSet('', 'quiet', 'normal', 'detailed')]
        [string] $Verbosity
    )

    if ($verbosity -eq '') {
        $verbosity = 'normal'
    }

    $nuget = Get-NuGetExe -Install

    try {
        Write-Debug 'Call nuget.exe restore.'
        . $nuget restore $source `
            -PackagesDirectory $packagesDirectory `
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
    param()

    Restore-Packages -Source (Get-LocalPath 'etc\packages.config') `
        -PackagesDirectory (Get-LocalPath 'packages') `
        -Verbosity 'quiet'
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
    Get-Process |
        Where-Object { $_.ProcessName -eq 'msbuild' } |
        ForEach-Object { Stop-Process $_.ID -Force }
}

# ------------------------------------------------------------------------------
