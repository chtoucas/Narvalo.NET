:: Build the documentation.
:: Same options as docfx:
:: - Extract language metadata
::   make.cmd metadata
:: - Generate documentation
::   make.cmd build
:: - Generate documentation then start a local HTTP server
::   make.cmd serve www
::
:: WARNING: Only works on my computer (the path to docfx.exe is hardcoded).

@echo off
@setlocal

:Setup

@set DocFX="C:\opt\apps\docfx\docfx.exe"

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