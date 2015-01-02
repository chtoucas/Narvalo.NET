@echo off
@setlocal

@echo.

:Setup

@set ToolsRoot=%~dp0
@set WorkRoot=%ToolsRoot%\..\work

@set SevenZip=%ToolsRoot%\7za.exe

@set ProjectFile=%ToolsRoot%\MyGet\MyGet.csproj
@set PackageFile=%WorkRoot%\myget.7z
@set StageDir=%WorkRoot%\myget\

:Cleanup

@if exist %PackageFile% (
  @del /Q %PackageFile%
)

@if exist %StageDir% (
  @rd %StageDir% /S /Q
)

:Download7Zip

@if not exist %SevenZip% (
  @echo Downloading 7zip...
  powershell -NoProfile -ExecutionPolicy Bypass -Command "Invoke-WebRequest -Uri 'http://narvalo.org/7z936.exe' -OutFile '%SevenZip%'"
)

@if %ERRORLEVEL% neq 0 (
  @goto Failure
)

:Build

@echo Building MyGet...

:: Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
:: Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
@call "%ToolsRoot%\MSBuild.cmd" /nologo /v:q /m /nr:false "%ProjectFile%" /t:Clean;Build /p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0

@if %ERRORLEVEL% neq 0 (
  @goto Failure
)

:Zip

@echo Zipping MyGet...

@call "%SevenZip%" -mx9 a "%PackageFile%" "%StageDir%" > nul 2>&1

@if %ERRORLEVEL% neq 0 (
  @goto Failure
)
@goto Success

:Failure

@echo.
@echo *** Build failed ***
@echo.
@goto End

:Success

@echo.
@echo Congratulations, Build successful! 
@echo You will find the zip file here: %PackageFile%
@echo.
@goto End

:End

@endlocal
@exit /B %ERRORLEVEL%
