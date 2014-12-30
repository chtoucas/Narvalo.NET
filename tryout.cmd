:: Quick and dirty runner.
::
:: To run this script, you MUST create your personal MSBuild response file: tryout.rsp.
:: A "good" practice is to include a file logger to %LogFile%.
::
:: Sample response file:
:: # MSBuild response file
::
:: # Project to build
:: .\tools\Make.TryOut.proj
::
:: # Targets
:: /t:Build
::
:: # Properties
:: /p:Configuration=Debug
:: /p:BuildGeneratedVersion=false
:: /p:SignAssembly=false
:: /p:VisibleInternals=true
::
:: # Console parameters
:: /verbosity:minimal
:: /maxcpucount
:: /nodeReuse:false
::
:: # File logger
:: /fileLogger
:: /fileloggerparameters:logfile=%LogFile%;verbosity=normal;encoding=utf-8
::

@echo off
@setlocal

@set RspFile="%~dp0\tryout.rsp"
@set WorkDir="%~dp0\work"
@set LogFile="%WorkDir%\tryout.log"

:Setup

:: Remove work directory.
@rem @rd %WorkDir% /S /Q

@if not exist %WorkDir% (
  @mkdir %WorkDir%
)

@if not exist %RspFile% (
  @set ErrMsg=To run this script, you MUST create the MSBuild response file: tryout.rsp
  @goto Failure
)

:Build

@call "%~dp0\tools\MSBuild.cmd" @%RspFile%

@if %ERRORLEVEL% neq 0 (
  @set ErrMsg=Build failed
  @goto Failure
)
@goto Success

:Failure

@echo.
@echo *** %ErrMsg% ***
@echo.
@goto End

:Success

@echo.
@echo Congratulations, Build successful!
@echo.
@goto End

:End

:: If the work directory exists, move the log file there.
@rem @if exist %WorkDir% (
@rem   @move /y %LogFile% %WorkDir% > nul 2>&1
@rem )

@endlocal
@pause
@exit /B %ERRORLEVEL%
