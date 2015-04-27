Build Infrastructure
====================

The main build script is `make.ps1`. It comes with complete documentation:
- Use `make.ps1 -h` to display the help.
- Use `make.ps1 -docs` to display the list of available tasks.

Releasing a NuGet package
-------------------------

To release a new version of all core packages:

1. Review shared version: `Narvalo.CurrentVersion.props` or local version props file.
2. Review individual versions in `src\NuGet`, if they differ from the shared one.
3. Review individual package descriptions in `src\NuGet`.
4. Build and publish the packages: `make.ps1 -r pushcore`.
5. Tag the repository. For instance, for version 1.1.0
```
git tag -a core-1.1.0 -m 'Core Version 1.1.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin core-1.1.0
```

Build Infrastructure
--------------------

The main build script is `make.ps1` at the root of the repository.

Add the project to Make.Foundations.proj when it is ready for publication.

### Global settings

Three build configurations are available:
- Debug configuration is optimized for development.
- Release configuration is optimized for safety.
- CodeContracts is solely for Code Contracts analysis.

Add relevant files as linked files to the "Properties" folder
- Shared assembly informations: `etc\AssemblyInfo.Common.cs`
- Code Analysis dictionary: `etc\CodeAnalysisDictionary.xml` with build action _CodeAnalysisDictionary_
- Strong Name Key: `etc\Narvalo.snk`

In Debug mode:
- "Build" panel, treat all warnings as errors.
- "Build" panel, check for arithmetic overflow/underflow.
- "Code Analysis" panel, use "Narvalo Rules".

In Release mode:
- "Build" panel, suppress compiler warnings 1591.
- "Build" panel, treat all warnings as errors.
- "Build" panel, output XML documentation file.
- "Code Analysis" panel, use "Narvalo Rules" and enable Code Analysis on Build.

For all modes:
- "Signing" panel, sign the assembly using the "etc\Narvalo.snk" key.
- "Build" panel, uncheck "Prefer 32-bit" if checked.
- "Code Analysis" panel, uncheck "Suppress results from generated code (managed only)".
