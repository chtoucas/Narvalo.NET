:: Simple script to build the most stable projects and run the tests.
::
:: Usage: quick [Release?]
::
:: WARNING: This script might fail if the packages were not previously restored.

@echo off
@setlocal

@set Configuration=Debug
@if "%1"=="Release" ( @set Configuration=Release )

@call "%~dp0\build.cmd" RunTests %Configuration% OnlyNuGetProjects

@endlocal
@exit /B %ERRORLEVEL%
