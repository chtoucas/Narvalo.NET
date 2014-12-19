:: Quick and dirty runner.

@echo off

:Build

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\tools\Make.Quick.proj" /t:Build /v:m /m /nr:false

@if %ERRORLEVEL% neq 0 ( @goto BuildFailure )
@goto BuildSuccess

:BuildFailure

@echo.
@echo *** Build failed ***
@echo.
@goto End

:BuildSuccess

@echo.
@echo Congratulations, Build successful!
@echo.
@goto End

:End

@exit /B %ERRORLEVEL%
@pause
