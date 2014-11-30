
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
:: This is not perfect. For instance it won't be aware of the last installed
:: version of the .NET SDK.
@set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild"
@if exist %MSBuild% ( @goto Build )

@echo *** Unable to find a suitable version of MSBuild ***
@exit /B 1
@goto :eof

:Build

@setlocal
@pushd %~dp0

@set BuildFile=.\Build.proj
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
@set Configuration=%1
@set Target=Default
@set Production=false

@if "%2"=="" ( @goto BuildCustom )
@set Target=%2

@if "%3"=="" ( @goto BuildCustom )
@if "%3"=="Stable" (
    @set Production=true
    @goto BuildCustom
)

@goto BuildCustom

:BuildDefaults

@echo Running default Build

%MSBuildWithOptions%

@if %ERRORLEVEL% neq 0 ( @goto BuildFailure )
@goto BuildSuccess

:BuildCustom

@echo Running Build with Configuration=%Configuration% Target=%Target% Production=%Production%

%MSBuildWithOptions% /t:%Target% /p:Production=%Production%;Configuration=%Configuration%

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
