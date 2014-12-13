#
# WARNING: This file must stay at the root of the repository.
#

Properties {
  $project = '.\Narvalo.proj'
  $options = '/verbosity:minimal', '/maxcpucount', '/nodeReuse:false'
  $logfile = "$PSScriptRoot\make.log"
  $normalFileLogger = '/fileLogger', "/fileloggerparameters:logfile=$logfile;verbosity=normal;encoding=utf-8"
  $detailedFileLogger = '/fileLogger', "/fileloggerparameters:logfile=$logfile;verbosity=detailed;encoding=utf-8"
  $workDir = "$PSScriptRoot\work\"
  $workArtefactsDir = "$workDir\artefacts\"
}

Task default -depends Minimal

# Remove the work directory.
Task Clean {
  if (Test-Path $workDir) {
    Remove-Item $workDir -Force -Recurse
  }
}

# Run tests for public projects in Debug configuration.
Task Minimal {
  MSBuild $options $project '/t:RunTests' '/p:SkipPrivateProjects=true;BuildGeneratedVersion=false'
}

# Continuous integration build in Release configuration.
Task CI {
  MSBuild $options $detailedFileLogger $project '/t:Build;VerifyBuild;RunTests;Package' '/p:Configuration=Release;SignAssembly=true'
} -PostAction {
  # We assume this is a terminal task. No more log to be written.
  Move-Item $logfile $workArtefactsDir
}

# Create packages for release: sign assembles and use Release configuration.
Task Package -depends Clean {
  MSBuild $options $normalFileLogger $project '/t:Clean;Build;VerifyBuild;RunTests;Package' '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true'
} -PostAction {
  # We assume this is a terminal task. No more log to be written.
  Move-Item $logfile $workArtefactsDir
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
