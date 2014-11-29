@ECHO off
@PUSHD %~dp0
@SETLOCAL

:Build
@rem Find the most recent 32bit MSBuild.exe on the system. Require v12.0
@rem (installed with VS2013) or later since .NET 4.0 is not supported.
@rem Also handle x86 operating systems, where %ProgramFiles(x86)% is not
@rem defined. Always quote the %MSBuild% value when setting the variable
@rem and never quote %MSBuild% references.
@SET MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@IF not exist %MSBuild% @SET MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"

@IF "%1" == "" @GOTO BuildDefaults
@GOTO BuildCustom

:BuildCustom
%MSBuild% Build.proj /nologo /v:m /fl /flp:logfile=.\build.log;verbosity=detailed;encoding=utf-8 /m /nr:false /t:%* /p:Production=true;Configuration=Release
@IF %ERRORLEVEL% neq 0 @GOTO Failure
@GOTO Success

:BuildDefaults
%MSBuild% Build.proj /nologo /v:m /fl /flp:logfile=.\build.log;verbosity=detailed;encoding=utf-8 /m /nr:false /p:Production=true;Configuration=Release
@IF %ERRORLEVEL% neq 0 @GOTO Failure
@GOTO Success

:Failure
@ECHO.
@ECHO *** BUILD FAILED ***
@ECHO.
@POPD
@ENDLOCAL
@EXIT /B 1

:Success
@ECHO.
@ECHO *** BUILD SUCCESSFUL ***
@ECHO.
@POPD
@ENDLOCAL
@EXIT /B 0
