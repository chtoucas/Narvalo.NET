:: Alias for MSBuild.

@echo off
@setlocal

:Bootstrap

:: Uncomment the next line to remove some limitations on property functions.
@rem @set MSBUILDENABLEALLPROPERTYFUNCTIONS=1

@set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto Build )

@set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto Build )

@if defined VS120COMNTOOLS (
  @call "%VS120COMNTOOLS%\..\..\VC\vcvarsall.bat"
  @set MSBuild=MSBuild
  @goto Build
)

:: As a last resort, we use the MSBuild installed alongside the .NET Framework.
:: It should work but it is not ideal. For instance, it won't be aware of the
:: last installed version of the .NET SDK.
@set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild"
@if exist %MSBuild% ( @goto Build )

:MSBuildNotFound

@echo *** Unable to find a suitable version of MSBuild ***
@endlocal
@exit /B 1
@goto :eof

:Build

%MSBuild% %*

@endlocal
@exit /B %ERRORLEVEL%
