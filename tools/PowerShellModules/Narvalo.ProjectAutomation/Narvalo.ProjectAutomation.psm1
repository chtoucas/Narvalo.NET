<#
.SYNOPSIS
    PowerShell module to manage the Narvalo.NET project.
.DESCRIPTION
    Provides helpers to automate various development tasks:
    - Install or uninstall external tools.
    - Fix the most common problems.
    - Run the build script.
    - Clean up the repository.
#>

Set-StrictMode -Version Latest

<#
.SYNOPSIS
    Validate a path for use as a project root.
.DESCRIPTION
    The Approve-ProjectRoot cmdlet validates that the path exists and is absolute.
.PARAMETER Path
    Specifies a path to be approved.
.INPUTS
    None.
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
    -Description 'Path to the local repository for the Narvalo.NET project.'
    
New-Variable -Name CopyrightHeader `
    -Value '// Copyright (c) Narvalo.Org.',
        'All rights reserved. See LICENSE.txt in the project root for license information.' `
    -Scope Script `
    -Option ReadOnly `
    -Description 'C# copyright header.'
     
New-Variable -Name DefaultNuGetVerbosity `
    -Value 'normal' `
    -Scope Script `
    -Option ReadOnly `
    -Description 'Default NuGet verbosity.'

#$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
#    Remove-Variable -Name ProjectRoot -Scope Script -Force
#}

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
    Specifies the message to be written to a host.
.INPUTS
    None.
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
.PARAMETER NoWarn
    f present, do not display warnings.
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
        [AllowNull()]
        [string] $Git,

        [switch] $Abbrev,
        [switch] $NoWarn
    )
    
    Write-Verbose 'Getting the last git commit hash.'

    if ($git -eq $null) {
        if (!$noWarn.isPresent) {
            Write-Warning 'The git parameter being $null, we can''t retrieve the git commit hash.' 
        }

        return ''
    }
    
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
    System.String. Get-PSakeModulePath returns a string that contains the path to the PSake module.
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
    If present, override any previous installed 7-Zip.
.INPUTS
    None.
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
    If present, override any previous installed NuGet.
.INPUTS
    None.
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
    Process the analysis tasks.
.PARAMETER NoConfirm
    If $true, do not ask for any confirmation.
#>
function Invoke-AnalyzeTask {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param([Parameter(Mandatory = $true)] [bool] $NoConfirm)
    
    if ($noConfirm -or (Confirm-Yes 'Analyze C# project files for common problems?')) {
        Measure-CSharpProjects 'src', 'samples', 'test'
    }
}

<#
.SYNOPSIS
    Process the cleanup tasks.
.PARAMETER Yes
    If $true, do not ask for any confirmation.
#>
function Invoke-PurgeTask {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param([Parameter(Mandatory = $true)] [bool] $NoConfirm)

    # Add a new task: remove ignored files?
    
    if ($noConfirm -or (Confirm-Yes 'Remove ''bin'' and ''obj'' directories?')) {
        Remove-BinAndObj 'samples', 'src', 'tests'
    }

    if ($noConfirm -or (Confirm-Yes 'Remove ''packages'' directory?')) {
        Remove-LocalItem 'packages' -Recurse
    }

    if ($noConfirm -or (Confirm-Yes 'Remove ''work'' directory?')) {
        Remove-LocalItem 'work' -Recurse
    }

    if ($noConfirm -or (Confirm-Yes 'Remove the locally installed tools?')) {
        Remove-LocalItem -Path (Get-7Zip)
        Remove-LocalItem -Path (Get-NuGet)
    }

    if ($noConfirm -or (Confirm-Yes 'Remove untracked files (unsafe)?')) {
        Get-Git | Remove-UntrackedItems -NoWarn
    }
}

<#
.SYNOPSIS
    Process the repair tasks.
.PARAMETER Yes
    If $true, do not ask for any confirmation.
#>
function Invoke-RepairTask {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param([Parameter(Mandatory = $true)] [bool] $NoConfirm)

    if ($noConfirm -or (Confirm-Yes 'Repair copyright headers?')) {
        Repair-Copyright 'samples', 'src', 'tests'
    }
}

<#
.SYNOPSIS
    Analyze project files.
.PARAMETER PathList
    Specifies the list of paths, relative to the project root, to traverse.
#>
function Measure-CSharpProjects {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('p')] [string[]] $PathList
    )

    PROCESS {
        $pathList | 
            %{ Get-LocalPath $_ } | 
            %{ ls $_ -Recurse -Filter *.csproj } |
            %{ Measure-ProjectFile $_ }
    }
}

<#
.SYNOPSIS
    Analyze a project file for common problems.
.PARAMETER File
    Specifies the path to a project file.
#>
function Measure-ProjectFile {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)] [System.IO.FileInfo] $File)

    Write-Verbose "Analyzing the project '$($file.Name)'."

    $content = [xml](cat $file.FullName)
    
    # If .NET supported XPath 2.0, we could use a simpler XPath query 
    # (namespace removed for the sake of simplification):
    #   /Project/ItemGroup/Compile[lower-case(./ExcludeFromStyleCop)="true"]
    $nodes = $content | 
        Select-Xml `
            –Namespace @{ m = 'http://schemas.microsoft.com/developer/msbuild/2003' } `
            -XPath '/m:Project/m:ItemGroup/m:Compile[translate(./m:ExcludeFromStyleCop, "TRUE", "true") = "true"]'

    if ($nodes -ne $null) {
        Write-Output ('In the project ''{0}'', there are {1} file(s) excluded from StyleCop:' -f $file.Name, $nodes.Length)
        $nodes | %{ Write-Output ('- {0}' -f $_.Node.Include) }
    } else {
        Write-Verbose "In the project $($file.Name), no file is excluded from StyleCop."
    }
}

function Publish-Packages {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [string] $Directory,
        
        [switch] $Retail
    )

    $helper = Get-LocalPath 'tools\NuGetHelper\bin\Release\NuGetHelper.exe'

    if (!(Test-Path $helper)) {
        Exit-Gracefully -ExitCode 1 `
            'Before calling this function, make sure to build the NuGetHelper project in Release configuration.'
    }

    . $helper $Directory $Retail
}

<#
.SYNOPSIS
    Remove the 'bin' and 'obj' directories created by Visual Studio.
.PARAMETER PathList
    Specifies the list of paths, relative to the project root, to traverse.
.INPUTS
    The list of paths, relative to the project root, to traverse.
.OUTPUTS
    None.
#>
function Remove-BinAndObj {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('p')] [string[]] $PathList
    )

    BEGIN { }

    PROCESS {
        $pathList | %{ Get-LocalPath $_ } | %{
            if (!(Test-Path $_)) { return }

            Write-Verbose "Processing directory '$_'."

            ls $_ -Include bin,obj -Recurse | ?{ rm $_.FullName -Force -Recurse }
        }
    }
}

function Remove-LocalItem {
    [CmdletBinding(DefaultParametersetName = 'Relative', SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ParameterSetName = 'Relative')]  
        [string] $RelativePath,

        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true, ParameterSetName = 'Absolute')]  
        [string] $Path,

        [switch] $Recurse
    )

    if ($PsCmdlet.ParameterSetName -eq 'Relative') {
        $path = Get-LocalPath $relativePath
    } 

    if (Test-Path $path) {
        rm $path -Force -Recurse:$recurse.IsPresent
    }
}

<#
.SYNOPSIS
    Remove files untracked by git.
.PARAMETER Git
    Specifies the path to the git executable.
.PARAMETER NoWarn
    f present, do not display warnings.
.INPUTS
    The path to the git executable.
.OUTPUTS
    None.
.NOTES
    We do not remove ignored files (see -x and -X options from git).
.LINK
    http://git-scm.com/docs/git-clean
#>
function Remove-UntrackedItems {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [AllowNull()]
        [string] $Git,

        [switch] $NoWarn
    )
    
    if ($git -eq $null) {
        if (!$noWarn.isPresent) {
            Write-Warning 'Git being $null, we can not proceed.' 
        }

        return
    }
    
    try {
        Push-Location $script:ProjectRoot

        # We exclude all folders named 'Internal' (some of them only contain linked files).
        if ($PSCmdlet.ShouldProcess($script:ProjectRoot, 'Calling git to permanently delete all untracked files')) {
            . $git clean -d -f -e 'Internal'
        } else {
            . $git clean -d -n -e 'Internal'
        }
    } catch {
        Write-Error "Unabled to remove untracked files: $_"
    } finally {
        Pop-Location
    }
}

<#
.SYNOPSIS
    Add a copyright header to all C# files missing one.
.INPUTS
    None.
.OUTPUTS
    System.String. Repair-Copyright returns a string that contains a short
    explanation of what has been done.
#>
function Repair-Copyright {
    [CmdletBinding(SupportsShouldProcess = $true)] 
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('p')] [string[]] $PathList
    )
    
    BEGIN {
        $count = 0
    }

    PROCESS {
        $items = $pathList | %{ Get-LocalPath $_ } | %{ Find-MissingCopyright -Path $_ }

        $count += ($items | measure).Count

        $items | %{ Add-Copyright $_.FullName }
    }

    END {
        if ($count -eq 0) {
            Write-Output 'No missing copyright header found :-)'
        } else {
            Write-Output "Repaired $count file(s) without a copyright header!"
        }
    }
}

<#
.SYNOPSIS
    Restore packages for the Edge project.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Restore-PackagesForEdge {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false, Position = 0)] 
        [string] $Verbosity
    )
    
    Write-Verbose 'Restoring packages for the Edge project.'

    Restore-Packages -Source (Get-LocalPath 'tools\Edge\packages.config') `
        -PackagesDirectory (Get-LocalPath 'tools\packages') `
        -ConfigFile (Get-LocalPath 'tools\.nuget\NuGet.Config') `
        -Verbosity $verbosity
}

<#
.SYNOPSIS
    Restore packages for the "maintenance" solution.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Restore-MaintenancePackages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false, Position = 0)] 
        [string] $Verbosity
    )
    
    Write-Verbose 'Restoring packages for the "maintenance" solution.'

    Restore-Packages -Source (Get-LocalPath 'tools\Narvalo Maintenance.sln') `
        -PackagesDirectory (Get-LocalPath 'tools\packages') `
        -ConfigFile (Get-LocalPath 'tools\.nuget\NuGet.Config') `
        -Verbosity $verbosity
}

<#
.SYNOPSIS
    Restore solution packages.
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
    Add a copyright header to a file.
.PARAMETER Path
    Specifies the file to repair.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Add-Copyright {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [Alias('p')] [string] $Path
    )

    Write-Verbose "Adding copyright header to $path..."

    $lines = New-Object System.Collections.ArrayList(, (cat -LiteralPath $path))
    $lines.Insert(0, '')
    $lines.Insert(0, $script:CopyrightHeader)
    $lines | Set-Content -LiteralPath $path -Encoding UTF8
}

<#
.SYNOPSIS
    Requests confirmation from the user. 
.PARAMETER Query
    Specifies the query to be displayed to the user.
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
    Find C# files without copyright header.
.PARAMETER Path
    Specifies the path to the directory to traverse.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Find-MissingCopyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('p')] [string] $Path
    )

    Write-Verbose 'Find all C# source files, ignoring designer generated files and temporary build directories.'

    ls $path -Recurse -Filter *.cs -Exclude *.Designer.cs |
        ?{ -not ($_.FullName.Contains('bin\') -or $_.FullName.Contains('obj\')) } | 
        ?{ -not (Test-Copyright $_.FullName) } | 
        select FullName
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
    Return $true if the file contains a copyright header, $false otherwise.
.PARAMETER Path
    Specifies the file to test.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function Test-Copyright {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [Alias('p')] [string] $Path
    )

    Write-Verbose "Processing ${path}."

    $line = cat -LiteralPath $path -TotalCount 1;

    return $line -and $line.StartsWith('// Copyright')
}

# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Exit-Gracefully,
    Get-7Zip,
    Get-Git, 
    Get-GitCommitHash, 
    Get-LocalPath,
    Get-NuGet, 
    Get-PSakeModulePath,
    Install-7Zip,
    Install-NuGet, 
    Install-PSake,
    Invoke-AnalyzeTask,
    Invoke-PurgeTask,
    Invoke-RepairTask,
    Measure-ProjectFile,
    Publish-Packages,
    Remove-BinAndObj,
    Remove-LocalItem,
    Remove-UntrackedItems,
    Repair-Copyright,
    Restore-PackagesForEdge,
    Restore-MaintenancePackages,
    Restore-SolutionPackages,
    Stop-AnyMSBuildProcess
        
# ------------------------------------------------------------------------------
