:: A. Non-retail packages
::  - List previous versions
::  - Delete all versions but last
::  - Publish package
::  - Restore packages
::  - Update Edge
::  - Compile Edge
:: B. Retail packages
::

@goto :eof

@echo Find all versions of a package
%~dp0\tools\nuget.exe list Narvalo.Core.EDGE ^
  -AllVersions ^
  -Prerelease ^
  -Source http://narvalo.org/myget/nuget/ ^
  -Verbosity detailed

@exit 0

@echo Delete a version of a package
%~dp0\tools\nuget.exe delete Narvalo.Core.EDGE 0.19.1-T8828252 ^
  -Source http://narvalo.org/myget/ ^
  -Verbosity detailed

@exit 0

@echo Publish packages
%~dp0\tools\nuget.exe push %~dp0\work\packages\*.nupkg ^
  -Source http://narvalo.org/myget/ ^
  -Verbosity detailed

@pushd %~dp0\tools\Edge

@echo Restore packages for Edge.csproj
..\nuget.exe restore .\packages.config ^
  -ConfigFile ..\.nuget\NuGet.config ^
  -Verbosity detailed ^
  -PackagesDirectory %~dp0\..\packages\

@echo Update packages for Edge.csproj
..\nuget.exe update .\packages.config ^
  -Source http://narvalo.org/myget/nuget/ ^
  -Prerelease ^
  -Verbosity detailed ^
  -RepositoryPath %~dp0\..\packages\

@popd

@exit 0
