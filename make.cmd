@echo off

@PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\make.ps1' %*; if ($psake.build_success -eq $false) { exit 1 } else { exit 0 }"

@exit /B %ERRORLEVEL%
