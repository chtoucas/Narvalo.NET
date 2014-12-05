:: Lean runner for Narvalo.NuGet.proj.
::
:: Usage: quick [Release?]
::
:: WARNING: This script might fail if the packages were not previously restored.

@echo off
@setlocal

@set Configuration=Debug
@if "%1"=="Release" (
  @set Configuration=Release
)

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Narvalo.NuGet.proj" /t:RunTests /p:LeanRun=true;Configuration=%Configuration%

@endlocal
@exit /B %ERRORLEVEL%
