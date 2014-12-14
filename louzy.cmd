:: Quick and dirty runner for Make.proj.

@echo off
@setlocal

@set Parameters=Configuration=Release;NoVisibleInternals=true;SignAssembly=true;ProjectFile=.\src\Narvalo.Core\Narvalo.Core.csproj
@set Targets=Package
@set Verbosity=minimal

:Build

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Make.proj" /t:%Targets% /p:%Parameters% /v:%Verbosity% /m /nr:false %LogParams%

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
