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
# Install a web resource if it is not already installed.
#
# .PARAMETER Force
# If present, override the previous installed resource if any.
#
# .OUTPUTS
# System.String. Install-WebResource returns a string that contains the path to the installed resource.
function Install-WebResource {
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
        echo "Installing $name."

        try {
            Write-Debug "Download $uri to $outFile."
            Invoke-WebRequest $uri -OutFile $outFile
        } catch {
            throw "Unabled to download $name."
        }
    }

    $outFile
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
        Install-WebResource,
        Invoke-NullCoalescing,
        Invoke-Ternary `
    -Alias `
        ??,
        ?:
