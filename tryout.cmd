:: Quick runner to try out the build system.
::
:: To run this script, you MUST create your personal MSBuild response file: tryout.rsp.
::
:: Sample response file:
::
:: # MSBuild project
:: %RepositoryRoot%\tools\Make.Common.targets
::
:: # Targets
:: /t:Build
::
:: # Properties
:: /p:Configuration=Debug
:: /p:BuildGeneratedVersion=false
:: /p:SignAssembly=false
:: /p:VisibleInternals=true
:: /p:ProjectsToBuild=%RepositoryRoot%\src\Narvalo.Core\Narvalo.Core.csproj
::
:: # Console parameters
:: /verbosity:minimal
:: /maxcpucount
:: /nodeReuse:false
::
:: # File logger
:: /fileLogger
:: /fileloggerparameters:logfile=%WorkDir%\tryout.log;verbosity=normal;encoding=utf-8
::

@echo off
@setlocal

@set RepositoryRoot=%~dp0
@set WorkDir="%RepositoryRoot%\work"

@set RspName=tryout.rsp
@set RspFile="%RepositoryRoot%\%RspName%"

:Setup

:: Remove work directory.
@rem @rd %WorkDir% /S /Q

@if not exist %WorkDir% (
  @mkdir %WorkDir%
)

@if not exist %RspFile% (
  @set ErrMsg=To run this script, you MUST create the MSBuild response file: %RspName%
  @goto Failure
)

:Build

@call "%RepositoryRoot%\tools\MSBuild.cmd" @%RspFile%

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
@rem @pause
@exit /B %ERRORLEVEL%
