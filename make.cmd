@echo off

@PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\make.ps1' %*"

@exit /B %ERRORLEVEL%
