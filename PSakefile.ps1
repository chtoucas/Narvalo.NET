
Properties {
  $options = '/nologo', '/v:minimal', '/fl',
    '/flp:logfile=.\msbuild.log;verbosity=normal;encoding=utf-8',
    '/m', '/nodeReuse:false'
}

Task default -depends Quick

Task Help {
  Write-Host 'Not yet implemented...'
}

Task CleanVisualStudio {
  Write-Host 'Not yet implemented...'
}

Task HardClean {
  Write-Host 'Not yet implemented...'
}

Task Quick {
  MSBuild $options '.\Make.proj' '/t:RunTests' '/p:Configuration=Debug;LeanRun=true;SkipPrivateProjects=true'
}

Task BuildCore {
  MSBuild $options '.\Make.proj' '/t:Build;VerifyBuild;RunTests' '/p:Configuration=Debug;SolutionFile=.\Narvalo (Core).sln'
}

Task BuildMvp {
  MSBuild $options '.\Make.proj' '/t:Build;VerifyBuild;RunTests' '/p:Configuration=Debug;SolutionFile=.\Narvalo (Mvp).sln'
}

Task BuildMiscs {
  MSBuild $options '.\Make.proj' '/t:Build;VerifyBuild;RunTests' '/p:Configuration=Debug;SolutionFile=.\Narvalo (Miscs).sln'
}

Task CI {
  MSBuild $options '.\Make.proj' '/t:Build;VerifyBuild;RunTests' '/p:Configuration=Release'
}

Task Publish -depends HardClean {
  MSBuild $options '.\Make.proj' '/t:Clean;Build;VerifyBuild;RunTests' '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true'
}
