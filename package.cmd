@ECHO off

:InstallFake
".\tools\NuGet\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"

:Package
"packages\FAKE\tools\Fake.exe" package.fsx
