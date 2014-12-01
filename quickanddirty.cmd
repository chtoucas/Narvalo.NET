:: Simple script to build the most stable projects and run the tests.
:: Only useful during development. See the description of the 
:: QuickAndDirtyBuildThenTest target in Narvalo.proj.
::
:: Usage: quickanddirty [Release?]

@echo off
@setlocal

@set Configuration=Debug
@if "%1"=="Release" ( @set Configuration=Release )

@call "%~dp0\build.cmd" QuickAndDirtyBuildThenTest %Configuration% OnlyNuGetProjects

@endlocal
@exit /B %ERRORLEVEL%
