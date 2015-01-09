#Requires -Version 3.0

Set-StrictMode -Version Latest

Add-Type -AssemblyName 'Microsoft.SqlServer.Smo, Version=10.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91'

# ------------------------------------------------------------------------------
# Public functions
# ------------------------------------------------------------------------------

function Export-Data {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $tables = $database.Tables | Where { $_.IsSystemObject -eq $false }

  if ($tables.count -eq 0) {
    Write-Verbose 'No data to export.'
    return
  }

  $scripter = New-Scripter -Server $server -OutFile $outFile

  $opts = $scripter.Options
  $opts.ScriptData = $true
  $opts.ScriptSchema = $false
  $opts.NoCommandTerminator = $true;

  $scripter.EnumScript($tables)
}

function Export-DbCreation {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 1)]
    [string] $outFile
  )

  $database.Script() | Out-File $outFile
}

function Export-Tables {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $tables = $database.Tables | Where { $_.IsSystemObject -eq $false }

  if ($tables.count -eq 0) {
    Write-Verbose 'There are no tables to export.'
    return
  }

  Write-Verbose "Exporting $($tables.Count) tables."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  $opts = $scripter.Options
  $opts.ClusteredIndexes = $true
  $opts.Default = $true
  $opts.DriAll = $true
  $opts.DriIncludeSystemNames = $false
  $opts.IncludeIfNotExists = $false
  $opts.Indexes = $true
  $opts.NoCollation = $true
  $opts.NonClusteredIndexes = $true
  $opts.Permissions = $false
  $opts.SchemaQualify = $false
  $opts.SchemaQualifyForeignKeysReferences = $false
  $opts.ScriptDrops = $false
  $opts.ScriptOwner = $false
  $opts.WithDependencies = $false

  foreach ($tbl in $tables) {
    $scripter.Script($tbl)
  }
}

function Export-StoredProcedures {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $procedures = $database.StoredProcedures | Where { $_.IsSystemObject -eq $false }

  if ($procedures.Count -eq 0) {
    Write-Verbose 'There are no stored procedures to export.'
    return
  }

  Write-Verbose "Exporting $($procedures.Count) stored procedures."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  $opts = $scripter.Options

  foreach ($procedure in $procedures) {
    # Si la procédure stockée existe déjà, on la supprime.
    $opts.IncludeIfNotExists = $true
    $opts.ScriptDrops = $true
    $scripter.Script($procedure)

    # Création de la procédure stockée.
    $opts.IncludeIfNotExists = $false
    $opts.ScriptDrops = $false
    $scripter.Script($procedure)
  }
}

function Export-Views {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $views = $database.Views | Where { $_.IsSystemObject -eq $false }

  if ($views.count -eq 0) {
    Write-Verbose 'There are no views to export.'
    return
  }

  Write-Verbose "Exporting $($views.Count) views."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($view in $views) {
    $scripter.Script($view)
  }
}

function Export-UserDefinedFunctions {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $udfs = $database.UserDefinedFunctions | Where { $_.IsSystemObject -eq $false }

  if ($udfs.count -eq 0) {
    Write-Verbose 'There are no user defined functions to export.'
    return
  }

  Write-Verbose "Exporting $($udfs.Count) views."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($udf in $udfs) {
    $scripter.Script($udf)
  }
}

function Export-Triggers {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $triggers = $database.Triggers | Where { $_.IsSystemObject -eq $false }

  if ($triggers.count -eq 0) {
    Write-Verbose 'There are no triggers to export.'
    return
  }

  Write-Verbose "Exporting $($triggers.Count) views."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($trigger in $triggers) {
    $scripter.Script($trigger)
  }
}

function Export-TableTriggers {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($tbl in $db.Tables) {
    foreach ($trigger in $tbl.triggers) {
      $scripter.Script($trigger)
    }
  }
}

# ------------------------------------------------------------------------------
# Private functions
# ------------------------------------------------------------------------------

function New-Scripter {
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [string] $outFile
  )

  # Cf. http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.management.smo.scriptingoptions.aspx
  $opts = New-Object Microsoft.SqlServer.Management.Smo.ScriptingOptions
  $opts.AllowSystemObjects = $false
  $opts.AppendToFile = $true
  $opts.FileName = $outFile
  $opts.ToFileOnly = $true

  $scripter = New-Object Microsoft.SqlServer.Management.Smo.Scripter
  $scripter.Server = $server
  $scripter.Options = $opts

  return $scripter
}

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember -function Export-*