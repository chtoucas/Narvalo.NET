@echo off
@pushd %~dp0
@setlocal

:Build
@rem Find the most recent 32bit MSBuild.exe on the system. Require v12.0
@rem (installed with VS2013) or later since .NET 4.0 is not supported.
@rem Also handle x86 operating systems, where %ProgramFiles(x86)% is not
@rem defined. Always quote the %MSBuild% value when setting the variable
@rem and never quote %MSBuild% references.
@set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@if not exist %MSBuild% @set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"

@if "%1" == "" @goto BuildDefaults
@goto BuildCustom

:BuildCustom
%MSBuild% Build.proj /nologo /v:m /fl /flp:logfile=.\build.log;verbosity=detailed;encoding=utf-8 /m /nr:false /t:%*
@if %ERRORLEVEL% neq 0 @goto Failure
@goto Success

:BuildDefaults
%MSBuild% Build.proj /nologo /v:m /fl /flp:logfile=.\build.log;verbosity=detailed;encoding=utf-8 /m /nr:false
@if %ERRORLEVEL% neq 0 @goto Failure
@goto Success

:Failure
@echo.
@echo *** BUILD FAILED ***
@goto End

:Success
@echo.
@echo *** BUILD SUCCESSFUL ***
@goto end

:End
@popd
@endlocal
