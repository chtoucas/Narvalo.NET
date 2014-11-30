@echo off

@if "%1"=="Release" ( @goto Release )
@goto Debug

:Debug
@call "%~dp0\build.cmd" Debug BuildFast Stable
@goto :eof

:Release
@call "%~dp0\build.cmd" Release BuildFast Stable
@goto :eof
