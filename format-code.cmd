:: Formatage automatique du code source.
::
:: WARNING: Only works on my computer (path to CodeFormatter.exe is hard-coded).
:: There is no NuGet package yet.
::
:: Currently disabled:
:: - It does not work for C# 7.0
:: - The tool also fails with an error CS1702 on our solution: it passes if we
::   comment out the CodeAnalysisRuleSet property in Narvalo.Common.props. In
::   fact, the problem seems to be with etc\Strict.ruleset.

@echo off
@setlocal

:: @goto Disabled

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

:Disabled

@echo.
@echo *** CodeFormatter.exe does not yet work w/ C# 7.0 ***
@echo.

@endlocal
@exit /B 1