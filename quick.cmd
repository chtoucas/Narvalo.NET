:: Quick and dirty runner for Make.proj.

@echo off
@setlocal

@set Parameters=SignAssembly=true;VisibleInternals=false
@set Targets=FullBuild
@set Verbosity=minimal

:Build

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Quick.proj" /t:%Targets% /p:%Parameters% /v:%Verbosity% /m /nr:false %LogParams%

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

@endlocal
@exit /B %ERRORLEVEL%
