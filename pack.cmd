:: Quick and dirty runner for Make.proj.

@echo off
@setlocal

@rd "%~dp0\work\" /S /Q

@set ArtefactsDir="%~dp0\work\artefacts\"
@set LogFile=.\pack.log

:Build

@call "%~dp0\tools\MSBuild.cmd" "%~dp0\Make.proj" /t:Clean;Build;VerifyBuild;RunTests;Publish /p:Configuration=Release;SkipPrivateProjects=true;SignAssembly=true /v:m /m /nr:false /fileLogger /fileloggerparameters:logfile=%LogFile%;verbosity=normal;encoding=utf-8

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
@if exist %ArtefactsDir% (
	@move /y %LogFile% %ArtefactsDir% > nul 2>&1
)

@endlocal
@exit /B %ERRORLEVEL%
