:: Launcher for Narvalo.proj.
::
:: Usage: build [Target?] [Configuration?] [OnlyNuGetProjects?]
::
:: Without options, it is equivalent to: build all projects in Release mode,
:: verify the assemblies and finally run the tests.
::
:: Examples:
:: - Same as default but only for the most stable projects:
::  build Default Release OnlyNuGetProjects
:: - Call "Build" target, Debug configuration:
::	build Build Debug
:: - Download NuGet:
::  build DownloadNuGet
:: - Restore NuGet packages:
::  build DownloadNuGet

@echo off

:Bootstrap

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

@echo *** Unable to find a suitable version of MSBuild ***
@exit /B 1
@goto :eof

:Build

@setlocal
@pushd %~dp0

@set BuildFile=.\Narvalo.proj
@set LogFile=.\build.log

:: MSBuild command line with options.
::  /v:m             -> Default logger only displays minimal information.
::  /fl /flp:logfile -> Detailed log saved to %LogFile%.
::  /m               -> Use up to the number of processors in the computer.
::  /nr:false        -> Disable the re-use of MSBuild nodes.
@set MSBuildWithOptions=%MSBuild% %BuildFile% /nologo /v:m /fl /flp:logfile=%LogFile%;verbosity=detailed;encoding=utf-8 /m /nr:false

:: No options, run default Build.
@if "%1"=="" ( @goto BuildDefaults )

:: Process options.
@set Target=%1
@set Configuration=Release
@set OnlyNuGetProjects=false

@if "%2"=="" ( @goto BuildCustom )
@set Configuration=%2

@if "%3"=="" ( @goto BuildCustom )
@if "%3"=="OnlyNuGetProjects" (
    @set OnlyNuGetProjects=true
    @goto BuildCustom
)

@goto BuildCustom

:BuildDefaults

@echo Running default Build

%MSBuildWithOptions%

@if %ERRORLEVEL% neq 0 ( @goto BuildFailure )
@goto BuildSuccess

:BuildCustom

@echo Running custom Build with Target=%Target% Configuration=%Configuration% OnlyNuGetProjects=%OnlyNuGetProjects%

%MSBuildWithOptions% /t:%Target% /p:OnlyNuGetProjects=%OnlyNuGetProjects%;Configuration=%Configuration%

@if %ERRORLEVEL% neq 0 ( @goto BuildFailure )
@goto BuildSuccess

:BuildFailure

@echo.
@echo *** Build failed ***
@echo.
@goto End

:BuildSuccess

@echo.
@echo Congratulations, Build successful!
@echo.
@goto End

:End

@move /y %LogFile% work\ > nul 2>&1
@popd
@endlocal
@exit /B %ERRORLEVEL%
