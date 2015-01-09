:: Quick runner to try out the build system.
::
:: To run this script, you MUST create your personal MSBuild response file: tryout.rsp.
:: If the file TryOut.proj exists, use it. If not, you MUST define it in your response file.
::
:: Sample response file:
::
:: # MSBuild project
:: .\tools\Make.proj
::
:: # Targets
:: /t:Build
::
:: # Properties
:: /p:Configuration=Debug
:: /p:SkipCodeContractsReferenceAssembly=true
:: /p:BuildGeneratedVersion=false
:: /p:SignAssembly=false
:: /p:VisibleInternals=true
:: /p:CustomProject=src\Narvalo.Core\Narvalo.Core.csproj
::
:: # Console parameters
:: /verbosity:minimal
:: /maxcpucount
:: /nodeReuse:false
::
:: # File logger
:: /fileLogger
:: /fileloggerparameters:logfile=tryout.log;verbosity=normal;encoding=utf-8
::

@echo on
@setlocal

:Setup

@set RepositoryRoot=%~dp0

@set ProjectFile=%RepositoryRoot%\TryOut.proj

@set RspFile=%RepositoryRoot%\tryout.rsp

@if not exist %RspFile% (
  @set ErrMsg=To run this script, you MUST create the MSBuild response file: %RspFile%
  @goto Failure
)

@if not exist %ProjectFile% (
  @set ProjectFile=
)

:Build

@call "%RepositoryRoot%\tools\MSBuild.cmd" ^
    @"%RspFile%" ^
    "%ProjectFile%"

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

@endlocal
@rem @pause
@exit /B %ERRORLEVEL%
