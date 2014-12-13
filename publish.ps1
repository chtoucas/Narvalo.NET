#Requires -Version 3.0

Set-StrictMode -Version Latest

#-- Configuration --#

$nugetExe = "$PSScriptRoot\tools\NuGet\NuGet.exe"
$packagesDir = "$PSScriptRoot\work\artefacts"

$packages = @(
  #@{ 'Name' = 'Narvalo.Build'; 'Path' = "$packagesDir\Narvalo.Build.1.0.0.nupkg" }
  #@{ 'Name' = 'Narvalo.Core'; 'Path' = "$packagesDir\Narvalo.Core.0.19.0.nupkg" }
  #@{ 'Name' = 'Narvalo.Common'; 'Path' = "$packagesDir\Narvalo.Common.0.19.0.nupkg" }
  #@{ 'Name' = 'Narvalo.Web'; 'Path' = "$packagesDir\Narvalo.Web.0.19.0.nupkg" }
  #@{ 'Name' = 'Narvalo.Mvp'; 'Path' = "$packagesDir\Narvalo.Mvp.1.0.0-alpha.nupkg" }
  #@{ 'Name' = 'Narvalo.Mvp.Web'; 'Path' = "$packagesDir\Narvalo.Mvp.Web.1.0.0-alpha.nupkg" }
)

#-- Sanity checks --#

if (!(Test-Path $nugetExe)) {
  Write-Error "This script requires a local copy of the NuGet executable!"
  Exit
}

#-- Publish --#

foreach ($package in $packages) {
  if (!(Test-Path $($package.Path))) {
	Write-Error "The package $($package.Name) does not exist!"
	Exit
  }
}

foreach ($package in $packages) {
  Write-Host "Publishing package $($package.Name)..." -ForegroundColor 'Yellow'
  
  & "$nugetExe" push $($package.Path)
}