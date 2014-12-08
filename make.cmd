:: Simple runner for Narvalo.proj.
::
:: Usage: make [Target?] [Configuration?] [SolutioName?]

@echo off
@setlocal
@pushd %~dp0

@set MSBuild=.\tools\MSBuild.cmd
@set BuildFile=.\Narvalo.proj

@if "%1"=="" ( @goto BuildDefault )
@if "%2"=="" ( @goto BuildTarget )
@if "%3"=="" ( @goto BuildTargetAndConfiguration )
@goto BuildSolution

:BuildDefault
@call %MSBuild% %BuildFile%
@goto End

:BuildTarget
@call %MSBuild% %BuildFile% /t:%1
@goto End

:BuildTargetAndConfiguration
@call %MSBuild% %BuildFile% /t:%1 /p:Configuration=%2
@goto End

:BuildSolution
@call %MSBuild% %BuildFile% /t:%1 /p:Configuration=%2;SolutionName=%3
@goto End

:End
@popd
@endlocal
@exit /B %ERRORLEVEL%
