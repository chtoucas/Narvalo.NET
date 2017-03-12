Overview
========

- [Configuring a new Project](new-project.md)
- [Coding Rules](coding-guidelines.md)
- [Versioning](versioning.md)
- [Developer Operations](devops.md)

Prerequisites
-------------

- Visual Studio Community 2017
- Modeling SDK (part of the Visual Studio installer) for T4 integration
- PowerShell v4

Optional components:
- [DocFX](https://dotnet.github.io/docfx/) to build the documentation.

Project Layout
--------------

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
- `MvpSample.sln` contains sample MVP projects.
- `tools\Maintenance.sln` contains documentation, settings, maintenance scripts
  and various helper projects:
  * MyGet, private NuGet server.
  * NuGetAgent, a NuGet publishing tool.

All tasks are fully automated with MSBuild, PowerShell and F# scripts.

### Projects

Library                   | Dependencies
--------------------------|-----------------------------------------------------
Narvalo.Build             | -
Narvalo.Common            | Core, Fx
Narvalo.Core              | -
Narvalo.Finance           | Core
Narvalo.Fx                | Core
Narvalo.Mvp               | Core
Narvalo.Mvp.Web           | Core, Mvp
Narvalo.Web               | Core, Common, Fx
