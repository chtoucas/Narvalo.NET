:: Script to create a self-signed certificate.

@echo on
@setlocal

:Setup

@set RepositoryRoot=%~dp0..

@set OpenCover=%RepositoryRoot%\packages\OpenCover.4.5.3723\OpenCover.Console.exe
@set XUnit=%RepositoryRoot%\packages\xunit.runner.console.2.0.0\tools\xunit.console.exe
@set ReportFile=%RepositoryRoot%\work\log\opencover.xml

@if not exist %OpenCover% (
    @set Message=*** Looks like OpenCover.Console.exe is not installed on your system ***

    @goto NotFound
)

@if not exist %XUnit% (
    @set Message=*** Looks like xunit.console.exe is not installed on your system ***

    @goto NotFound
)

@goto Execute

:Execute

@call %OpenCover% ^
  -register:user ^
  -filter:"+[Narvalo*]* -[Xunit*]*" ^
  -output:%ReportFile% ^
  -target:%XUnit% ^
  -targetargs:"%RepositoryRoot%\work\bin\Debug+Closed\Narvalo.Facts.dll -nologo -noshadow"

@if %ERRORLEVEL% neq 0 (
    @set Message=*** OpenCover.exe failed ***

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
