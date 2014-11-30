:: Default Build script.
::
:: Usage:
:: - Default Build: build.cmd
:: - Custom Build:  build.cmd [Configuration] [Target] [Stable?]

@echo off
:: @cls
@pushd %~dp0
@setlocal

@echo.


:FindMSBuild
@set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto PrepareForBuild )

@set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto PrepareForBuild )

@if defined VS120COMNTOOLS (
  @call "%VS120COMNTOOLS%\..\..\VC\vcvarsall.bat"
  @set MSBuild=MSBuild
  @goto PrepareForBuild
)

:: As a last resort, we use the MSBuild installed alongside .NET.
:: This is not perfect since it might be outdated.
:: For instance it won't use the last installed version of the .NET SDK.
@set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild"
@if exist %MSBuild% ( @goto PrepareForBuild )

@echo.
@echo *** Unable to find a suitable version of MSBuild ***
@echo.
@goto :eof


:PrepareForBuild
@set BuildFile=.\Build.proj
@set LogFile=.\build.log

:: MSBuild command line with options.
::  /v:m             -> Default logger only displays minimal information.
::  /fl /flp:logfile -> Detailed log saved to %LogFile%.
::  /m               -> Use up to the number of processors in the computer.
::  /nr:false        -> Disable the re-use of MSBuild nodes.
@set MSBuildCommand=%MSBuild% %BuildFile% /nologo /v:m /fl /flp:logfile=%LogFile%;verbosity=detailed;encoding=utf-8 /m /nr:false


:Build
@if "%1"=="" ( @goto BuildDefaults )

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
@echo.

%MSBuildCommand%

@if %ERRORLEVEL% neq 0 ( @goto BuildFailure )
@goto BuildSuccess


:BuildCustom
@echo Running custom Build: Configuration=%Configuration% Target=%Target% Production=%Production%
@echo.

%MSBuildCommand% /t:%Target% /p:Production=%Production%;Configuration=%Configuration%

@if %ERRORLEVEL% neq 0 ( @goto BuildFailure )
@goto BuildSuccess


:BuildFailure
@echo.
@echo *** Build failed ***
@goto End


:BuildSuccess
@echo.
@echo Congratulations, Build successful!
@goto End


:End
@move /y %LogFile% work\ > nul 2>&1
@echo.
@popd
@endlocal
@exit /B %ERRORLEVEL%
