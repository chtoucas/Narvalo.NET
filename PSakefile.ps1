#
# WARNING: This file must stay at the root of the repository.
#

Properties {
  $project = '.\Make.proj'
  $options = '/verbosity:minimal', '/maxcpucount', '/nodeReuse:false'
  $logfile = "$PSScriptRoot\make.log"
  $normalFileLogger = '/fileLogger', "/fileloggerparameters:logfile=$logfile;verbosity=normal;encoding=utf-8"
  $detailedFileLogger = '/fileLogger', "/fileloggerparameters:logfile=$logfile;verbosity=detailed;encoding=utf-8"
}

Task default -depends Minimal

# Remove all bin and obj directories created by Visual Studio.
Task HardCleanVisualStudio {
  Remove-VisualStudioTmpFiles "$PSScriptRoot\samples\"
  Remove-VisualStudioTmpFiles "$PSScriptRoot\src\"
  Remove-VisualStudioTmpFiles "$PSScriptRoot\tests\"
}

# Remove the work directory.
Task HardCleanWorkDir {
  $workDir = "$PSScriptRoot\work\"

  if (Test-Path $workDir) {
    Remove-Item $workDir -Force -Recurse
  }
}

# Run tests for public projects in Debug configuration.
Task Minimal {
  MSBuild $options $project '/t:RunTests' '/p:SkipPrivateProjects=true;SkipBuildGeneratedVersion=true'
}

# Continuous integration build in Release configuration.
Task CI {
  MSBuild $options $detailedFileLogger $project '/t:Build;VerifyBuild;RunTests' '/p:Configuration=Release'
} -PostAction {
  Move-Item $logfile "$PSScriptRoot\work\artefacts\Release"
}

# Create packages for release: sign assembles and use Release configuration.
Task Publish -depends HardCleanWorkDir {
  MSBuild $options $normalFileLogger $project '/t:Clean;Build;VerifyBuild;RunTests;Publish' '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true'
} -PostAction {
  Move-Item $logfile "$PSScriptRoot\work\artefacts\Release"
}

# Lean build for Narvalo (Core).sln in Release configuration.
Task CoreSolution {
  MSBuild $options $project '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo (Core).sln'
}

# Lean build for Narvalo.sln in Release configuration.
Task MainSolution {
  MSBuild $options $project '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo.sln'
}

# Lean build for Narvalo (Miscs).sln in Release configuration.
Task MiscsSolution {
  MSBuild $options $project '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo (Miscs).sln'
}

# Lean build for Narvalo (Mvp).sln in Release configuration.
Task MvpSolution {
  MSBuild $options $project '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo (Mvp).sln'
}

# .SYNOPSIS
# Supprime les répertoires 'bin' and 'obj' créés par Visual Studio.
#
# .PARAMETER path
# Répertoire dans lequel résident les projets Visual Studio.
function Remove-VisualStudioTmpFiles {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  Get-ChildItem $path -Include bin,obj -Recurse |
    Where-Object { Remove-Item $_.FullName -Force -Recurse }
}
