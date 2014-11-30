:: Simple script to build the most stable projects and run the tests.
:: Only useful during development. It might even fail (see description
:: of the BuildFast target in Build.proj).
::
:: Usage: buildfast [Release?]

@echo off
@setlocal

@set Configuration=Debug
@if "%1"=="Release" ( @set Configuration=Release )

@call "%~dp0\build.cmd" BuildFast %Configuration% OnlyNuGetProjects

@endlocal
@exit /B %ERRORLEVEL%
