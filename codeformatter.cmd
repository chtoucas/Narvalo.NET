:: Formatage automatique du code source.
::
:: WARNING: Only works on my computer (path to CodeFormatter.exe is hard-coded).

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
@echo *** Path to CodeFormatter.exe is wrong ***
@echo.

@endlocal
@exit /B 1