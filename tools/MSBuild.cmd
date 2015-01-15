:: Wrapper for MSBuild.exe.

@echo off
@setlocal

:Setup

:: Uncomment the next line to remove any limitation on property functions.
@rem @set MSBUILDENABLEALLPROPERTYFUNCTIONS=1

@set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto Execute )

@set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto Execute )

@if defined VS120COMNTOOLS (
    @call "%VS120COMNTOOLS%\..\..\VC\vcvarsall.bat"
    @set MSBuild=MSBuild
    @goto Execute
)

:: As a last resort, we use the MSBuild installed alongside the .NET Framework.
:: It should work but it is not ideal. For instance, it is not aware of the
:: last installed version of the .NET SDK.
@set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild"

@if exist %MSBuild% (
    @goto Execute
) else (
    @goto NotFound
)

:Execute

@call %MSBuild% %*

@endlocal
@exit /B %ERRORLEVEL%

:NotFound

@echo.
@echo *** Unable to find a suitable version of MSBuild ***
@echo.

@endlocal
@exit /B 1
