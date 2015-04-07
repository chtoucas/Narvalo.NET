:: Script to create a self-signed certificate.

@echo off
@setlocal

:Setup
:: Do not use %~dp0\.. gendarme doe not like double backslash.
@set RepositoryRoot=%~dp0..

@set Gendarme=%RepositoryRoot%\packages\Mono.Gendarme.2.11.0.20121120\tools\gendarme.exe
@set ConfigFile=%RepositoryRoot%\etc\gendarme.xml
@set IgnoreFile=%RepositoryRoot%\etc\gendarme.ignore
@set LogFile=%RepositoryRoot%\gendarme.log
@set Limit=100
@set RuleSet=narvalo-strict

@echo %ConfigFile%

@if not exist %Gendarme% (
    @set Message=*** Looks like gendarme.exe is not installed on your system ***

    @goto NotFound
)

@goto Execute

:Execute

@call %Gendarme% ^
  --v ^
  --console ^
  --limit %Limit% ^
  --config %ConfigFile% ^
  --set %RuleSet% ^
  --severity all ^
  --confidence all ^
  --ignore "%IgnoreFile%" ^
  --log "%LogFile%" ^
  "%RepositoryRoot%\work\bin\Release+Closed\Narvalo.Core.dll" ^
  "%RepositoryRoot%\work\bin\Release+Closed\Narvalo.Common.dll"

@if %ERRORLEVEL% neq 0 (
    @set Message=*** gendarme.exe failed ***

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
