:: Quick and dirty runner for Narvalo.proj.

@echo off
@setlocal

@set Parameters=Configuration=Debug;ProjectFile=.\src\Narvalo.Core\Narvalo.Core.csproj
@set Targets=Build
@set Verbosity=minimal

:Build

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Narvalo.proj" /t:%Targets% /p:%Parameters% /v:%Verbosity% /m /nr:false %LogParams%

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
