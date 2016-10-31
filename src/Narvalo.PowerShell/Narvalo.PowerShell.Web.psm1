Set-StrictMode -Version Latest

# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Return the path to MSDeploy.
#>
function Get-WebDeployInstallPath {
    $regKey = 'HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy'

    if (!(Test-Path $regKey)) {
        throw 'MSDeploy is not installed.'
    }

    # REVIEW: Use (ls $regKey | select -Last 1) if there are more than one version
    # of MSDeploy installed.

    return (Get-ItemProperty $regKey).InstallPath
}

<#
.SYNOPSIS
    Import WebAdministration as module or snap-in.
.LINK
    http://stackoverflow.com/questions/1924217/powershell-load-webadministration-in-ps1-script-on-both-iis-7-and-iis-7-5/4325963#4325963
#>
function Import-WebAdministration {
    [CmdletBinding()]
    param()

    $regKey = 'HKLM:\SOFTWARE\Microsoft\InetStp'

    if (!(Test-Path $regKey)) {
        throw 'Can not import WebAdministration, IIS is not installed.'
    }

    $iisVersion = Get-ItemProperty $regKey

    if ($iisVersion.MajorVersion -eq 7) {
        if ($iisVersion.MinorVersion -ge 5) {
            Write-Verbose 'IIS 7.5 is installed. Will import ''WebAdministration'' as a module.'

            if (!(Get-Module WebAdministration)) {
                Import-Module WebAdministration
            }
        } else {
            Write-Verbose 'IIS 7.5 is not installed. Will import ''WebAdministration'' as a snap-in.'

            if (!(Get-PSSnapIn | ?{ $_.Name -eq 'WebAdministration' })) {
                Add-PSSnapIn WebAdministration
            }
        }
    } else {
        throw 'Can not import WebAdministration: installed IIS is too old. At least IIS version 7.0 is required.'
    }
}

# ------------------------------------------------------------------------------

Export-ModuleMember -Function `
    Get-WebDeployInstallPath,
    Import-WebAdministration

# ------------------------------------------------------------------------------
