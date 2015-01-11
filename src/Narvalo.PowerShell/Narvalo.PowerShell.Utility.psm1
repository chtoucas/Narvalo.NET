Set-StrictMode -Version Latest

Add-Type -AssemblyName 'System.IO.Compression.FileSystem'

# .SYNOPSIS
# Décompresse un fichier au format ZIP.
#
# .PARAMETER path
# Chemin du fichier à décompresser.
function Expand-ZipArchive {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $file,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  Write-Output -NoNewline 'Unzipping...'
  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $extractPath)
  Write-Output 'done'
}

# .SYNOPSIS
# Fusionne des fichiers UTF-8.
#
# .DESCRIPTION
# Cette fonction permet de fusionner des fichiers UTF-8(Y).
#
# Si un des fichiers à fusionner contient un BOM, ce dernier
# n'apparaîtra pas au milieu du fichier fusionné.
#
# Si le fichier fusionné existe déjà il est écrasé.
#
# .PARAMETER inFiles
# Liste des fichiers à fusionner.
#
# .PARAMETER outFile
# Nom du fichier fusionné.
function Merge-Utf8Files {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [array] $inFiles,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  if (Test-Path $outFile) { Remove-Item $outFile }

  $encoding = New-Object System.Text.UTF8Encoding($false)

  foreach ($filePath in $inFiles) {
    $content = [System.IO.File]::ReadAllLines($filePath)
    [System.IO.File]::AppendAllLines($outFile, $content, $encoding)
  }
}

Export-ModuleMember -Function *