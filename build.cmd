@echo off
pushd %~dp0
setlocal

:RestorePackages
@REM We must restore the NuGet packages first because our MSBuild depends on them.
".\tools\NuGet\nuget.exe" restore "Narvalo (Core).sln" "/Verbosity" "quiet"

:Build

@REM Find the most recent 32bit MSBuild.exe on the system. Require v12.0 (installed with VS2013) or later since .NET 4.0
@REM is not supported. Also handle x86 operating systems, where %ProgramFiles(x86)% is not defined. Always quote the
@REM %MSBuild% value when setting the variable and never quote %MSBuild% references.
set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
if not exist %MSBuild% @set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"

if "%1" == "" goto BuildDefaults

%MSBuild% tools\Build.proj /nologo /v:minimal /fl /flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8 /m /nodeReuse:false /t:%*
if %ERRORLEVEL% neq 0 goto BuildFail
goto BuildSuccess

:BuildDefaults
%MSBuild% tools\Build.proj /nologo /v:minimal /fl /flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8 /m /nodeReuse:false
if %ERRORLEVEL% neq 0 goto BuildFail
goto BuildSuccess

:BuildFail
echo.
echo *** BUILD FAILED ***
goto End

:BuildSuccess
echo.
echo **** BUILD SUCCESSFUL ***
goto end

:End
popd
endlocal