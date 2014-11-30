
@echo off
@setlocal

@set Configuration=Debug
@if "%1"=="Release" ( @set Configuration=Release )

@call "%~dp0\build.cmd" %Configuration% BuildFast Stable

@endlocal
@exit /B %ERRORLEVEL%
