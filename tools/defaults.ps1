
Properties {
  $project = '.\Narvalo.proj'

  $options = '/nologo', '/v:minimal', '/fl',
    '/flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8',
    '/m', '/nodeReuse:false'
}

Task default -depends Build

Task Help {
  Write-Host 'Sorry, help not yet available...'
}

Task Clean {
  MSBuild $options $project /t:Clean
}

Task Build {
  MSBuild $options $project /t:Build
}

Task Rebuild {
  MSBuild $options $project /t:Rebuild
}

Task FastBuild {
  MSBuild $options $project /t:Build '/p:AnalyzeSource=false;Analyze=false;RunTests=false'
}

Task StyleCop {
  MSBuild $options $project /t:StyleCop '/p:AnalyzeSource=true;Analyze=false;RunTests=false'
}

Task Test {
  MSBuild $options $project /t:Build '/p:AnalyzeSource=false;Analyze=false;RunTests=true'
}

Task Package {
  MSBuild $options $project /t:Package
}

Task PackageForMvp {
  MSBuild $options $project /t:PackageForMvp
}
