:: Runner for Narvalo.proj.

@echo off

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Narvalo.proj" %*

@exit /B %ERRORLEVEL%
