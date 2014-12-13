:: Quick and dirty runner for Make.proj.

@echo off
@setlocal

@set Parameters=Configuration=Debug;BuildGeneratedVersion=true;SignAssembly=true;ProjectFile=.\src\Narvalo.Core\Narvalo.Core.csproj
@set Targets=Publish
@set Verbosity=minimal

:Build

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Make.proj" /t:%Targets% /p:%Parameters% /v:%Verbosity% /m /nr:false

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

:: If the artefacts directory exists, move the log file there.
@rem @set ArtefactsDir="%~dp0\..\work\artefacts\"
@rem @if exist %ArtefactsDir% (
@rem 	@move /y %LogFile% %ArtefactsDir% > nul 2>&1
@rem )

@endlocal
@exit /B %ERRORLEVEL%
