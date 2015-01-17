:: Quick runner to try out the build system.
::
:: To run this script, you MUST create your personal MSBuild response file: tryout.rsp.
:: If the file TryOut.proj exists, use it. If not, you MUST define it in your response file.
::
:: Sample response file:
::  # MSBuild project
::    .\tools\Make.proj
::  # Targets
::    /t:Build
::  # Properties
::    /p:Configuration=Debug
::    /p:BuildGeneratedVersion=false
::    /p:SignAssembly=false
::    /p:SkipCodeContractsReferenceAssembly=true
::    /p:VisibleInternals=true
::    /p:CustomProject=src\Narvalo.Core\Narvalo.Core.csproj
::  # Console parameters
::    /verbosity:minimal
::    /maxcpucount
::    /nodeReuse:false
::  # File logger
::    /fileLogger
::    /fileloggerparameters:logfile=tryout.log;verbosity=normal;encoding=utf-8
::
:: Another option:
::  # MSBuild project
::    %RepositoryRoot%\tools\Make.Common.targets
::  # Properties
::    /p:ProjectsToBuild=%RepositoryRoot%\src\Narvalo.Core\Narvalo.Core.csproj

:: Do not turn off command-echoing. This script is mainly used for debugging
:: the build system.
@echo on
@setlocal

:Setup

@set RepositoryRoot=%~dp0\..

@set ProjectFile=%~dp0\TryOut.proj

@if not exist %ProjectFile% (
    @set ProjectFile=
)

@set RspFile=%~dp0\tryout.rsp

@if exist %RspFile% (
    @goto Execute
) else (
    @goto NotFound
)

:Execute

@pushd

@call "%~dp0\MSBuild.cmd" @"%RspFile%" "%ProjectFile%"

@popd

@echo.

@if %ERRORLEVEL% equ 0 (
    @echo Congratulations, Build Successful!
) else (
    @echo *** Build failed ***
)

@echo.

@endlocal
@exit /B %ERRORLEVEL%

:NotFound

@echo.
@echo *** To run this script, you MUST create a MSBuild response file: %RspFile% ***
@echo.

@endlocal
@exit /B 1
