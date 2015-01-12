Set-StrictMode -Version Latest

Add-Type -AssemblyName 'System.IO.Compression.FileSystem'

# ------------------------------------------------------------------------------

<# 
.SYNOPSIS
    Uncompress a ZIP file.
.PARAMETER Path
    Specifies the path to the file to be uncompressed.
.INPUTS
    The path to the file to be uncompressed.
.OUTPUTS
    None.
#>
function Expand-ZipArchive {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $Path,
        [Parameter(Mandatory = $true, Position = 1)] [string] $OutPath
    )

    Write-Verbose 'Unzipping...'
    [System.IO.Compression.ZipFile]::ExtractToDirectory($path, $outPath)
    Write-Verbose 'done'
}

<# 
.SYNOPSIS
    Merge UTF-8 files.
.DESCRIPTION
    This function allows to merge UTF-8(Y) files.
    Prior to merging the files, erase the output file if it already exists.
.PARAMETER PathList
    Speficies a list of paths of files to be merged.
.PARAMETER OutPath
    Sepcifies the path of the merged file.
.NOTES
    This function guarantees that no BOM wil end up in the middle of the file.
#>
function Merge-Utf8Files {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string[]] $PathList,
        [Parameter(Mandatory = $true, Position = 1)] [string] $OutPath
    )

    if (Test-Path $outPath) { Remove-Item $outPath }

    $encoding = New-Object System.Text.UTF8Encoding($false)

    foreach ($path in $pathList) {
        Write-Verbose "Processing file '$path'."
        $content = [System.IO.File]::ReadAllLines($path)
        [System.IO.File]::AppendAllLines($outPath, $content, $encoding)
    }
}

# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Expand-ZipArchive,
    Merge-Utf8Files

# ------------------------------------------------------------------------------
