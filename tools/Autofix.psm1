#Requires -Version 3.0

Set-StrictMode -Version Latest

Join-Path $PSScriptRoot 'Helpers.psm1' | Import-Module
Join-Path $PSScriptRoot 'Project.psm1' | Import-Module -NoClobber

# ------------------------------------------------------------------------------
# Private variables
# ------------------------------------------------------------------------------

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
# .OUTPUTS
# None.
#
# .NOTES
# We do not remove ignored files (see -x and -X options from git).
# 
# .LINK
# http://git-scm.com/docs/git-clean
function Remove-UntrackedItems {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false, Position = 0, ValueFromPipeline = $true)] $git,
        [Parameter(Mandatory = $true)] [Alias('n')] [bool] $dryRun
    )

    # Sadly, it will keep the response in the console history.
    $yn = Read-Host 'Beware this will permanently delete all untracked files. Are you sure [y/N]?'
    if ($yn -ne 'y') {
        # NB: This comparison is case insensitive.
        Write-Verbose 'Cancelling on user request.'
        Exit 0
    }
    
    $git = ?? $git { Project\Get-Git }
    if ($git -eq $null) { return }

    Write-Host 'Cleaning repository...'

    try {
        Push-Location (Get-RepositoryRoot)

        # We ignore folders named 'Internal' (some of them only contain linked files).
        if ($dryRun) {
            . $git clean -d -n -e 'Internal'
        } else {
            # Force cleanup, ie ignore clean.requireForce
            . $git clean -d -f -e 'Internal'
        }
    } catch {
        Exit-Error "Unabled to clean repository: $_"
    } finally {
        Pop-Location
    }
}

# .SYNOPSIS
# Remove the 'bin' and 'obj' directories created by Visual Studio.
#
# .PARAMETER Path
# Specifies the path to a directory.
#
# .OUTPUTS
# None.
function Remove-VisualStudioTmpFiles {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)
  
  if (!(Test-Path $path)) { return }
  
  Write-Verbose "Removing VS tmp files in $path..."

  gci $path -Include bin,obj -Recurse | ? { rm $_.FullName -Force -Recurse -WhatIf }
}

# .SYNOPSIS
# Add a copyright header to all C# files missing one.
#
# .OUTPUTS
# None.
function Repair-Copyright {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)] [bool] $dryRun)

    $items = 'samples', 'src', 'tests' | 
        % { (Project\Get-RepositoryPath $_) } | 
        % { Find-MissingCopyright -Path $_ }

    if ($items -eq $null) {
        Write-Host 'Copyright header: no problem found.' -ForeGround Green
        return
    }

    $count = ($items | measure).Count

    Write-Host "Copyright header: found $count files without a (c)." -ForeGround Red

    if ($dryRun) { return }

    $items | % { Add-Copyright $_.FullName }
}

# ------------------------------------------------------------------------------
# Private methods
# ------------------------------------------------------------------------------

# .SYNOPSIS
# Add a copyright header to a file.
#
# .PARAMETER Path
# The file to repair.
#
# .OUTPUTS
# None.
function Add-Copyright {
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
# Find C# files without copyright header.
#
# .PARAMETER Path
# Specifies the path to the directory to traverse.
#
# .OUTPUTS
# None.
function Find-MissingCopyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [Alias('p')] [string] $path
    )

    Write-Verbose 'Find all C# source files, ignoring designer generated files and temporary build directories.'

    gci $path -Recurse -Filter *.cs -Exclude *.Designer.cs |
        ? { -not ($_.FullName.Contains('bin\') -or $_.FullName.Contains('obj\')) } | 
        ? { -not (Test-Copyright $_.FullName) } | 
        select FullName
}

function Test-Copyright {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string] $path
    )

    Write-Verbose "Processing $path."

    $line = Get-Content -LiteralPath $path -TotalCount 1;

    return $line -and $line.StartsWith("// Copyright")
}

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Remove-UntrackedItems,
    Remove-VisualStudioTmpFiles,
    Repair-Copyright
