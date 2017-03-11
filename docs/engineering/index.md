Overview
========

- [Configuring a new Project](new-project.md)
- [Coding Rules](coding-guidelines.md)
- [Build Infrastructure](build.md)
- [Versioning](versioning.md)
- [Developer Operations](devops.md)

Prerequisites
-------------

- Visual Studio Community 2017
- PowerShell v4

Optional components:
- [Code Contracts extension for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970)
- [.NET Portability Analyzer](https://visualstudiogallery.msdn.microsoft.com/1177943e-cfb7-4822-a8a6-e56c7905292b)
  to help evaluate portability across .NET platforms.
- .NET Framework v3.5, installing it from the _optional Windows components_
  is sufficient. The reason is that the MSBuild scripts for Code Contracts
  require MSBuild v3.5; see [CC Issue](https://github.com/Microsoft/CodeContracts/issues/353)

Components necessary to run the build scripts:
- Code Contracts (see above).
- Visual Extensibility Tools (VS optional component) provides T4 integration in MSBuild.

Optional necessary to run the build scripts:
- Tools and Windows SDK (VS optional component) for PEVerify.exe and SecAnnotate.exe.
- [DocFX](https://dotnet.github.io/docfx/) to build the documentation.

Obsolete requirements:
- [Microsoft Visual Studio 2013 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=40758),
  prerequisite for the Modeling SDK (see below).
- [Modeling SDK for Microsoft Visual Studio 2013](http://www.microsoft.com/en-us/download/details.aspx?id=40754),
  provides T4 integration in MSBuild.
- Microsoft .NET 4.5.1 Developer Pack(?),  Microsoft Windows SDK for Windows 8.1(?)
  or .NET Framework SDK for PEVerify.exe.

Project Layout
--------------

- `.nuget`, NuGet configuration.
- `data`, external data.
- `docs`, documentation.
- `etc`, shared configurations.
- `packages`, local repository of NuGet packages.
- `samples`, sample projects.
- `src`, source directory.
- `src\NuGet`, NuGet projects.
- `tests`, test projects.
- `tools`, build and maintenance scripts.
- `work`, temporary directory created during cmdline builds.

Solutions
---------

There are two solutions.
- `Narvalo.sln` contains all projects.
- `tools\Maintenance.sln` contains documentation, settings, maintenance scripts
  and various helper projects:
  * MyGet, private NuGet server.
  * NuGetAgent, a NuGet publishing tool.
  * Narvalo.ProjectAutomation, PowerShell project.

Most developments are done in C#. Three build configurations are available:
- Debug configuration is optimized for development.
- Release configuration is optimized for safety.
- CodeContracts is solely for Code Contracts analysis.

All tasks are fully automated with MSBuild, PowerShell (PSake) and F# scripts.

### Projects

- Libraries
  * Narvalo.Cerbere
  * Narvalo.Common
  * Narvalo.Core
  * Narvalo.Finance
  * Narvalo.Fx
  * Narvalo.Money
  * Narvalo.Web
- MVP Framework
  * Narvalo.Mvp
  * Narvalo.Mvp.Web
- Developer Tools
  * Narvalo.Build
- Samples
  * Edufun
  * MvpCommandLine
  * MvpWebForms
- Test projects
- For internal use only
  * Narvalo.T4
