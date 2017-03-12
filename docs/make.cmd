:: Build the documentation.
:: Same options as docfx:
:: - Extract language metadata
::   make.cmd metadata
:: - Generate documentation
::   make.cmd build
:: - Generate documentation then start a local HTTP server
::   make.cmd serve _www

@echo off
@setlocal

:Setup

@set DocFX="..\packages\docfx.console.2.13.1\tools\docfx.exe"

@if not exist %DocFX% (
    @goto NotFound
)

:Format

@call %DocFX% %*

@endlocal
@exit /B %ERRORLEVEL%

:NotFound

@echo.
@echo *** CodeFormatter.exe is not where it is supposed to be ***
@echo.

@endlocal
@exit /B 1