


@goto :eof

:Find
@echo Find all versions of a package
%~dp0\tools\nuget.exe list Narvalo.Core.EDGE ^
  -AllVersions ^
  -Prerelease ^
  -Source http://narvalo.org/myget/nuget/ ^
  -Verbosity detailed

@goto :eof

:Delete

@echo Delete a version of a package
%~dp0\tools\nuget.exe delete Narvalo.Core.EDGE 0.19.1-T8828252 ^
  -Source http://narvalo.org/myget/ ^
  -Verbosity detailed

@goto :eof

:Publish

@echo Publish packages
%~dp0\tools\nuget.exe push %~dp0\work\packages\*.nupkg ^
  -Source http://narvalo.org/myget/ ^
  -Verbosity detailed

@goto :eof
