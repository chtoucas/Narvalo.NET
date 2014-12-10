:: Lean runner for Make (Dev).proj.
::
:: Usage: quick [Release?]
::
:: WARNING: This script might fail if the packages were not previously restored.

@echo off

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Make.proj" /t:Build /p:LeanRun=true;ProjectFile=.\src\Narvalo.Core\Narvalo.Core.csproj

@exit /B %ERRORLEVEL%
