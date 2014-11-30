@echo off
:: @cls
@pushd %~dp0
@setlocal

@set BuildFile=.\Build.proj
@set LogFile=.\build.log

@echo.

:InitEnvironment
:: Find the most recent 32bit MSBuild.exe on the system. Require v12.0
:: (installed with VS2013) or later.
:: Also handle x86 operating systems, where %ProgramFiles(x86)% is not
:: defined. Always quote the %MSBuild% value when setting the variable
:: and never quote %MSBuild% references.
@set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@if not exist %MSBuild% (
    @set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
)
@if not exist %MSBuild% (
    :: Use VS2012 environment tools?
    :: @call "%VS120COMNTOOLS%vsvars32.bat"
    :: Use the one installed with .NET framework?
    :: @set MSBuild="%windir%\microsoft.net\framework\v4.0.30319\MSBuild"
    @goto Fatal
)

:: MSBuild command line with options.
:: see http://msdn.microsoft.com/en-us/library/vstudio/ms164311%28v=vs.120%29.aspx
:: /v:m             -> Default logger only displays minimal information.
:: /fl /flp:logfile -> Detailed log saved in %LogFile%.
:: /m               -> Use up to the number of processors in the computer.
:: /nr:false        -> Disable the re-use of MSBuild nodes.
@set MSBuildCommand=%MSBuild% %BuildFile% /nologo /v:m /fl /flp:logfile=%LogFile%;verbosity=detailed;encoding=utf-8 /m /nr:false

:Build
@if "%1"=="" ( @goto BuildDefaults )

@set Configuration=%1
@set Target=Default
@set Production=false

@if "%2"=="" ( @goto BuildCustom )
@set Target=%2

@if "%3"=="" ( @goto BuildCustom )
@set Production=%3

@goto BuildCustom

:BuildDefaults
@echo Running Default Build
@echo.

%MSBuildCommand%
@if %ERRORLEVEL% neq 0 ( @goto Failure )

@goto Success

:BuildCustom
@echo Running Custom Build: Configuration=%Configuration% Target=%Target% Production=%Production%
@echo.

%MSBuildCommand% /t:%Target% /p:Production=%Production%;Configuration=%Configuration%
@if %ERRORLEVEL% neq 0 ( @goto Failure )

@goto Success

:Fatal
@echo.
@echo *** Unable to find a suitable version of MSBuild ***
@echo.
@goto :eof

:Failure
@echo.
@echo *** Build failed ***
@goto End

:Success
@echo.
@echo Congratulations, Build successful!
@goto End

:End
@move /y %LogFile% work\ > nul 2>&1
@echo.
@popd
@endlocal
@exit /B %ERRORLEVEL%
