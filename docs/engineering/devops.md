Developer Operations
====================

Updating
--------

### NuGet Updates

For package updates, use the Narvalo.sln solution.

**WARNING:** If the NuGet core framework is updated, do not forget to also
update `tools\nuget.exe` (I believe this is done automatically whenever we use
it to install/update packages):
```
tools\nuget.exe update -Self
```

### Visual Studio or Framework Updates

After upgrading Visual Studio or MSBuild, do not forget to update the
`VisualStudioVersion` property in both Make.Shared.props and PSakefile.ps1.
We might also need to update the `SDK40ToolsPath` property.

Other places to look at:
- `Framework` in PSakefile.ps1
- fsci.cmd (for F# updates)
- `TargetFrameworkVersion` in Narvalo.Common.props
- For NuGet packaging check the target lib.

### Copyright Year Update

A copyright appears in three places:
- `etc\AssemblyInfo.Common.cs`
- `LICENSE.txt`
- `tools\Make.CustomAfter.props` (this one is always up-to-date)

Releasing a NuGet package
-------------------------

Before going further, you MUST read [Versioning](versioning.md).

Checklist:
- The shared version: `etc\Narvalo.CurrentVersion.props`.
- Individual versions in `src\NuGet`, if they differ from the shared one.
- Individual package descriptions in `src\NuGet`.

To release a new version of a package (for instance the core packages):
1. Build and publish the packages: `make.ps1 -r pushcore`.
2. Tag the repository. For instance, for version 1.1.0
```
git tag -a release-1.1.0 -m 'Core Version 1.1.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin core-1.1.0
```
