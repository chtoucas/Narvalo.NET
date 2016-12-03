:: Formatage automatique du code source.
::
:: WARNING: Only works on my computer (the path to CodeFormatter.exe is hardcoded).

@echo off
@setlocal

:Setup

@set CodeFormatter="C:\opt\apps\CodeFormatter\CodeFormatter.exe"
@set SolutionFile=%~dp0\Narvalo.sln
@set CopyrightHeader=%~dp0\etc\CopyrightHeader.txt

@if not exist %CodeFormatter% (
    @goto NotFound
)

:Format

@call %CodeFormatter% %SolutionFile% /copyright:%CopyrightHeader% /nounicode

@endlocal
@exit /B %ERRORLEVEL%

:NotFound

@echo.
@echo *** CodeFormatter.exe is not where it is supposed to be ***
@echo.

@endlocal
@exit /B 1