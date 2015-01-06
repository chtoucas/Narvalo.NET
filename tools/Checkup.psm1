#Requires -Version 4.0
#Requires -Modules Helpers
#Requires -Modules Project

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------
# Private variables
# ------------------------------------------------------------------------------

[string] $SCRIPT:CopyrightHeader = '// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.'

# ------------------------------------------------------------------------------
# Public methods
# ------------------------------------------------------------------------------

# .SYNOPSIS
# Remove the 'bin' and 'obj' directories created by Visual Studio.
#
# .PARAMETER PathList
# Specifies the list of paths to traverse.
#
# .OUTPUTS
# None.
function Remove-BinAndObj {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [Alias('p')] [string[]] $PathList
    )

    BEGIN { }

    PROCESS {
        $pathList | %{
            if (!(Test-Path $_)) { return }

            Write-Verbose "Processing directory '$_'."

            ls $_ -Include bin,obj -Recurse | ?{ rm $_.FullName -Force -Recurse }
        }
    }
}

# .SYNOPSIS
# Remove untracked files.
#
# .PARAMETER Git
# Specifies the path to the Git executable.
#
# .INPUTS
# The path to the Git executable.
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
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $Git,

        [Parameter(Mandatory = $true, Position = 1)] 
        [Alias('p')] [string] $Path
    )
    
    if (!$git) {
        throw 'The ''git'' parameter can not be null or empty.'
    }
    
    Write-Verbose 'Removing untracked files.'

    try {
        Push-Location $path

        # We exclude all folders named 'Internal' (some of them only contain linked files).
        if ($PSCmdlet.ShouldProcess($path, 'Calling git to permanently delete all untracked files')) {
            . $git clean -d -f -e 'Internal'
        } else {
            . $git clean -d -n -e 'Internal'
        }
    } catch {
        Exit-Error "Unabled to remove untracked files: $_"
    } finally {
        Pop-Location
    }
}

# .SYNOPSIS
# Add a copyright header to all C# files missing one.
#
# .OUTPUTS
# None.
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
        $items = $pathList | %{ Find-MissingCopyright -Path $_ }

        $count += ($items | measure).Count

        $items | %{ Add-Copyright $_.FullName }
    }

    END {
        if ($count -eq 0) {
            echo 'No missing copyright header found :-)'
        } else {
            echo "Repaired $count file(s) without a copyright header!"
        }
    }
}

# ------------------------------------------------------------------------------
# Private methods
# ------------------------------------------------------------------------------

# .SYNOPSIS
# Add a copyright header to a file.
#
# .PARAMETER Path
# Specifies the file to repair.
#
# .OUTPUTS
# None.
function Add-Copyright {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [Alias('p')] [string] $Path
    )

    Write-Verbose "Adding copyright header to $path..."

    $lines = New-Object System.Collections.ArrayList(, (cat -LiteralPath $path))
    $lines.Insert(0, '')
    $lines.Insert(0, $SCRIPT:CopyrightHeader)
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
        [Alias('p')] [string] $Path
    )

    Write-Verbose 'Find all C# source files, ignoring designer generated files and temporary build directories.'

    ls $path -Recurse -Filter *.cs -Exclude *.Designer.cs |
        ?{ -not ($_.FullName.Contains('bin\') -or $_.FullName.Contains('obj\')) } | 
        ?{ -not (Test-Copyright $_.FullName) } | 
        select FullName
}

function Test-Copyright {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [Alias('p')] [string] $Path
    )

    Write-Verbose "Processing $path."

    $line = cat -LiteralPath $path -TotalCount 1;

    return $line -and $line.StartsWith('// Copyright')
}

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Remove-BinAndObj,
    Remove-UntrackedItems,
    Repair-Copyright,
    Yala
