:: Script to create a self-signed certificate.

@echo off
@setlocal

:Setup

@set SdkVersion=v7.1A

@set MakeCert="%ProgramFiles(x86)%\Microsoft SDKs\Windows\%SdkVersion%\Bin\makecert.exe"
@set Pvk2pfx="%ProgramFiles(x86)%\Microsoft SDKs\Windows\%SdkVersion%\Bin\pvk2pfx.exe"

@if not exist %MakeCert% (
    @set Message=*** Looks like makecert.exe is not installed on your system ***

    @goto NotFound
)

@if not exist %Pvk2pfx% (
    @set Message=*** Looks like pvk2pfx.exe is not installed on your system ***

    @goto NotFound
)

@goto Execute

:Execute

%MakeCert% ^
  -n "CN=CARoot" ^
  -r ^
  -pe ^
  -a sha512 ^
  -len 4096 ^
  -cy authority ^
  -sv CARoot.pvk ^
  CARoot.cer

@if %ERRORLEVEL% neq 0 (
    @set Message=*** makecert.exe failed ***

    @goto End
)

%Pvk2pfx% ^
  -pvk CARoot.pvk ^
  -spc CARoot.cer ^
  -pfx CARoot.pfx ^
  -po Test123

@if %ERRORLEVEL% neq 0 (
    @set Message=*** pvk2pfx.exe failed ***

    @goto End
)

@set Message=Self-signed certificate successfuly created

@goto End

:NotFound

@echo.
@echo %Message%
@echo.

@endlocal
@exit /B 1

:End

@echo.
@echo %Message%
@echo.

@endlocal
@exit /B %ERRORLEVEL%
