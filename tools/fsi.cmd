:: Wrapper for fsi.exe.

@echo off
@setlocal

:Setup

@set FsiVersion=4.0
@set FrameworkVersion=v4.0

@set Fsi="%ProgramFiles(x86)%\Microsoft SDKs\F#\%FsiVersion%\Framework\%FrameworkVersion%\fsi.exe"

@if exist %Fsi% (
    @goto Execute
) else (
    @goto NotFound
)

:Execute

:: For all available options, see http://msdn.microsoft.com/en-us/library/dd233172.aspx
@call %Fsi% %*

@endlocal
@exit /B %ERRORLEVEL%

:NotFound

@echo.
@echo *** Looks like fsi.exe v%FsiVersion% is not installed on your system ***
@echo.

@endlocal
@exit /B 1
