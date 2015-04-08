:: Script to create a self-signed certificate.

@echo off
@setlocal

:Setup

@set RepositoryRoot=%~dp0..

@set ReportGenerator=%RepositoryRoot%\packages\ReportGenerator.2.1.4.0\ReportGenerator.exe
@set XUnit=%RepositoryRoot%\packages\xunit.runner.console.2.0.0\tools\xunit.console.exe
@set ReportFile=%RepositoryRoot%\work\log\opencover.xml
@set ReportDir=%RepositoryRoot%\work\log\opencover
@set Filters=+Narvalo.Core;+Narvalo.Common;+Narvalo.Web
::@set Filters=+Narvalo.Core;+Narvalo.Common;+Narvalo.Web

@if not exist %ReportGenerator% (
    @set Message=*** Looks like ReportGenerator.exe is not installed on your system ***

    @goto NotFound
)

@if not exist %XUnit% (
    @set Message=*** Looks like xunit.console.exe is not installed on your system ***

    @goto NotFound
)

@goto Execute

:Execute

@call %ReportGenerator% ^
  -verbosity:Info ^
  -filters:%Filters% ^
  -reports:%ReportFile%  ^
  -targetdir:%ReportDir%

@if %ERRORLEVEL% neq 0 (
    @set Message=*** ReportGenerator.exe failed ***

    @goto End
)

@set Message=Report successfuly created

@goto End

:NotFound

@echo.
@echo %Message%
@echo.

@endlocal
@exit /B 1

:End

@echo.
@echo %Message%
@echo.

@endlocal
@exit /B %ERRORLEVEL%
