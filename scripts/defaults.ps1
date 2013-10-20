
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
  MSBuild $options $project /t:Build '/p:RunTests=false;Analyze=false'
}

Task Milestone -depends ReadMilestoneConfig {
  MSBuild $options $project /t:Milestone $msproperties
}

Task Package {
  MSBuild $options $project /t:Package
}

Task ReadMilestoneConfig {
  $configPath = $(Get-Location).Path + '\..\etc\Milestone.config'

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $milestone = $config.Milestone

  $script:msproperties = "/p:Milestone=$milestone";
}