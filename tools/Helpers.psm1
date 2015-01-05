#Requires -Version 3.0

Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------
# Public methods
# ------------------------------------------------------------------------------

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
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] 
        [string] $Message
    )
    
    Write-Host "`n", $message, "`n" -BackgroundColor Red -ForegroundColor Yellow
    Exit 1
}

# .SYNOPSIS
# Mimic the ?? operator from C# but not quite the same.
function Invoke-NullCoalescing {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] 
        [AllowNull()] $Value,

        [Parameter(Mandatory = $true, Position = 1)] 
        [PSObject] $Alternate
    )

    if ($value -ne $null) {
        $result = $value
    } else {
        if ($alternate -is [ScriptBlock]) {
            $result = & $alternate
        } else {
            $result = $alternate
        }
    }

    $result
}

# .SYNOPSIS
# Mimic the ?: operator from C#.
function Invoke-Ternary {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [bool] $Predicate,

        [Parameter(Mandatory = $true, Position = 1)]
        [AllowNull()]
        [PSObject] $Then,

        [Parameter(Mandatory = $true, Position = 2)] 
        [AllowNull()]
        [PSObject] $Otherwise
    )

    if ($predicate) {
        return $then
    } else {
        return $otherwise
    }
}

# ------------------------------------------------------------------------------
# Aliases
# ------------------------------------------------------------------------------

Set-Alias ?? Invoke-NullCoalescing
Set-Alias ?: Invoke-Ternary

# ------------------------------------------------------------------------------
# Exports
# ------------------------------------------------------------------------------

Export-ModuleMember `
    -Function `
        Exit-Error,
        Invoke-NullCoalescing,
        Invoke-Ternary `
    -Alias `
        ??,
        ?:
