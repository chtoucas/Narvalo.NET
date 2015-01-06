#Requires -Version 4.0

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
#
# .NOTES
# Aliases: ??
#
# .LINK
# https://pscx.codeplex.com/
function Invoke-NullCoalescing {
    param(
        [Parameter(Mandatory, Position=0)]
        [AllowNull()]
        [scriptblock]
        $PrimaryExpr,

        [Parameter(Mandatory, Position=1)]
        [scriptblock]
        $AlternateExpr,

        [Parameter(ValueFromPipeline, ParameterSetName='InputObject')]
        [psobject]
        $InputObject
    )

    Process {
        if ($pscmdlet.ParameterSetName -eq 'InputObject') {
            if ($PrimaryExpr -eq $null) {
                Foreach-Object $AlternateExpr -InputObject $InputObject
            }
            else {
                $result = Foreach-Object $PrimaryExpr -input $InputObject
                if ($result -eq $null) {
                    Foreach-Object $AlternateExpr -InputObject $InputObject
                }
                else {
                    $result
                }
            }
        }
        else {
            if ($PrimaryExpr -eq $null) {
                &$AlternateExpr
            }
            else {
                $result = &$PrimaryExpr
                if ($result -eq $null) {
                    &$AlternateExpr
                }
                else {
                    $result
                }
            }
        }
    }
}

# .SYNOPSIS
# Mimic the ?: operator from C#.
#
# .NOTES
# Aliases: ?:
#
# .LINK
# https://pscx.codeplex.com/
function Invoke-Ternary {
    param(
        [Parameter(Mandatory, Position=0)]
        [scriptblock]
        $Condition,

        [Parameter(Mandatory, Position=1)]
        [scriptblock]
        $TrueBlock,

        [Parameter(Mandatory, Position=2)]
        [scriptblock]
        $FalseBlock,

        [Parameter(ValueFromPipeline, ParameterSetName='InputObject')]
        [psobject]
        $InputObject
    ) 
        
    Process { 
        if ($pscmdlet.ParameterSetName -eq 'InputObject') {
            Foreach-Object $Condition -input $InputObject | Foreach { 
                if ($_) { 
                    Foreach-Object $TrueBlock -InputObject $InputObject 
                }
                else {
                    Foreach-Object $FalseBlock -InputObject $InputObject 
                }
            }
        }
        elseif (&$Condition) {
            &$TrueBlock
        } 
        else {
            &$FalseBlock
        }
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
