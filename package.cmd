
@echo off

".\tools\NuGet\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"

"packages\FAKE\tools\Fake.exe" package.fsx
