:: Alias for MSBuild.

@echo on
@setlocal

:Bootstrap

:: Uncomment the next line to remove some limitations on property functions.
@rem @set MSBUILDENABLEALLPROPERTYFUNCTIONS=1

@set MSBuild="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto Build )

@set MSBuild="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
@if exist %MSBuild% ( @goto Build )

@if defined VS120COMNTOOLS (
  @call "%VS120COMNTOOLS%\..\..\VC\vcvarsall.bat"
  @set MSBuild=MSBuild
  @goto Build
)

:: As a last resort, we use the MSBuild installed alongside the .NET Framework.
:: It should work but it is not ideal. For instance, it won't be aware of the
:: last installed version of the .NET SDK.
@set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild"
@if exist %MSBuild% ( @goto Build )

@echo *** Unable to find a suitable version of MSBuild ***
@endlocal
@exit /B 1
@goto :eof

:Build

@set LogFile="%~dp0\..\msbuild.log"

:: MSBuild command line with options.
::  /v:m             -> Default logger only displays minimal information.
::  /fl /flp:logfile -> Detailed log saved to %LogFile%.
::  /m               -> Use up to the number of processors in the computer.
::  /nr:false        -> Disable the re-use of MSBuild nodes.
%MSBuild% %* /nologo /v:m /fl /flp:logfile=%LogFile%;verbosity=detailed;encoding=utf-8 /m /nr:false

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
@set ArtefactsDir="%~dp0\..\work\artefacts\"
@if exist %ArtefactsDir% ( 
	@move /y %LogFile% %ArtefactsDir% > nul 2>&1
)

@endlocal
@exit /B %ERRORLEVEL%
