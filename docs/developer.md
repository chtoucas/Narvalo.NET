Developer Guidelines
====================

- [Overview](#overview)
- [Coding Rules](#coding-rules)
- [Project Configuration](#project-configuration)
- [Localization](#localization)
- [Versioning](#versioning)
- [Packaging](#packaging)
- [Developer Operations](#developer-operations)

--------------------------------------------------------------------------------

Overview
--------

### Prerequisites

Requirements:
- Visual Studio Community 2017.
- Modeling SDK (part of the Visual Studio installer) needed for T4 integration
  with MSBuild.
- PowerShell v4+, F# v4.1+ and Git for the build scripts.

### Project Layout

- `data`, external data.
- `docs`, documentation.
- `etc`, shared configuration files.
- `samples`, sample projects.
- `src`, source directory.
  * `src\Packaging`, NuGet projects.
  * `src\Versioning`, assembly versions.
- `tests`, test projects.
- `tools`, build and maintenance scripts.
  * `tools\lib`, Visual Studio runtime dependencies.

Temporary directories:
- `packages`, local repository of NuGet packages.
- `work`, created by command-line builds.

There are three solutions:
- `Narvalo.sln` the main solution.
- `Narvalo.Light.sln` same as `Narvalo.sln` but without the test projects.
- `samples\MvpSamples.sln` contains sample MVP projects.
- `tools\Maintenance.sln` contains settings and maintenance scripts.

--------------------------------------------------------------------------------

Coding Rules
------------

We mostly follow the guidelines produced by the .NET teams:
[corefx](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/)
and [coreclr](https://github.com/dotnet/coreclr/blob/master/Documentation/coding-guidelines/).

To enforce the coding rules, we use [CodeFormatter](https://github.com/dotnet/codeformatter)
(currently disabled as the tool does not support C# 7.0 yet).
See [here](#developer-operations).

In addition:
- Consider using tasks: FIXME, HACK, TODO, REVIEW.
- For temporary strings, use `XXX`.
- For tests, consider using traits:
  * `Slow` for slow tests.
  * `Unsafe` for unsafe tests.

All Code Analysis suppressions must be justified and tagged:
- `[Ignore]` Only use this one to tag a false positive.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases.

--------------------------------------------------------------------------------

Project Configuration
---------------------

### Initialize a C# project

The following procedure enables us to centralize all settings:
- Create a project and add it to `Narvalo.sln`, `Narvalo.Light.sln` and `tools\Make.proj`.
- Add the following line at the bottom of the project file, BEFORE the Microsoft targets:
```xml
<Import Project="..\..\tools\Narvalo.Common.props" />
```
- Remove all sections about Debug and Release configurations.
- Remove all properties already configured globally.

A typical project file should then look like this:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <ProjectGuid>{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}</ProjectGuid>
    <RootNamespace>Narvalo.XXX</RootNamespace>
    <AssemblyName>Narvalo.XXX</AssemblyName>
  </PropertyGroup>
    ...
  <Import Project="..\..\tools\Narvalo.Common.props" />
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
```

Clean up the assembly information file (`Properties\AssemblyInfo.cs`):
```csharp
// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("The assembly title.")]
[assembly: AssemblyDescription("The assembly description.")]
```
For the assembly version see [here](#versioning).

Optionally, give access to internals to the test project:
```csharp
#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.XXX.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
```

### How to override the global settings

Create a property file `Narvalo.XXX.props` for instance with the following
content:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <CodeAnalysisRuleSet>MinimumRecommendedRules.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>

  <Target Name="_WarnOnTemporaryOverriddenSettings" BeforeTargets="Build">
    <Warning Text="We temporarily override the Source Analysis settings for $(AssemblyName)."
             Condition=" '$(SourceAnalysisEnabled)' == 'true' "/>
    <Warning Text="We temporarily override the Code Analysis settings for $(AssemblyName)."
             Condition=" '$(RunCodeAnalysis)' == 'true' " />
  </Target>
</Project>
```

### Special Cases

#### Portable Class Library

The long term plan is to move to .NET Standard projects.

We target at least .NET 4.5, Windows 8 and Windows Phone 8.1:
- **Profile259** (.NET Framework 4.5, ASP.NET Core 1.0, Windows 8, Windows Phone 8.1, Windows Phone Silverlight 8):
  * NuGet PCL: `portable-net45+netcore45+wpa81+wp8`
  * For MSBuild: `TargetFrameworkVersion=v4.5`.
  * Supported by .NET Standard 1.0.
- **Profile111** (.NET Framework 4.5, ASP.NET Core 1.0, Windows 8, Windows Phone 8.1):
  * NuGet PCL: `portable-net45+netcore45+wpa81`
  * For MSBuild: `TargetFrameworkVersion=v4.5`.
  * Supported by .NET Standard 1.1.
- **Profile151** (.NET Framework 4.5.1, ASP.NET Core 1.0, Windows 8.1, Windows Phone 8.1):
  * NuGet PCL: `portable-net451+netcore451+wpa81`
  * For MSBuild: `TargetFrameworkVersion=v4.6`.
  * Supported by .NET Standard 1.2.

Notable additions:
- .NET Standard 1.1 (Profile111) vs 1.0 (Profile259):
  * `System.Collections.Concurrent.ConcurrentDictionary`.
- .NET Standard 1.2 (Profile151) vs 1.1 (Profile111):
  * `System.Threading.Timer`.

See also: `src\Packaging\PortableProfiles.props`.

#### Desktop application

Desktop applications should include a .ini with:
```
[.NET Framework Debugging Control]
GenerateTrackingInfo=0
AllowOptimize=1
```
Ensure that it is copied to the output directory.

#### Test project

To create a test project use the "Class Library" template from Visual Studio.
Add the following content to you local customization property file `Narvalo.XXX.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Tests.props" />
</Project>
```
This has two consequences:
- Test projects use a dummy assembly version.
- Test projects use custom FxCop rules.

Reference the shared project `tests\TestCommon`.

#### Sample project

Add the following content to you local customization property file `Narvalo.XXX.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Samples.props" />
</Project>
```
This has ony one effect:
- Sample projects use a dummy assembly version.
- Sample projects use custom FxCop rules.

--------------------------------------------------------------------------------

Localization
------------

English is the default language. Localized resources are to be found in
`Properties\Strings.resx`. Sometimes we provide resources in French too.

**WARNING** If a project references two libraries with the same root namespace,
eg Narvalo.XXX and Narvalo.YYY, and have access to their internals (a Xunit
project is the typical example where this might happen), to avoid any conflict,
it is better to choose a different name for each resource; for instance,
`Properties\Strings_XXX.resx` for Narvalo.XXX and
`Properties\Strings_YYY.resx` for Narvalo.YYY.

--------------------------------------------------------------------------------

Versioning
----------

Reminder:
- `AssemblyVersion`, version used by the runtime.
- `AssemblyFileVersion`, version as seen in the file explorer,
  also used to uniquely identify a build.
- `AssemblyInformationalVersion`, the product version.

We use the following:
- `AssemblyVersion = MAJOR.MINOR.0.0`
- `AssemblyFileVersion = MAJOR.MINOR.BUILD.REVISION`
- `AssemblyInformationalVersion = MAJOR.MINOR.PATCH(-PreRelaseLabel)(+BuildMetadata)`

`MAJOR`, `MINOR`, `PATCH` and `PreRelaseLabel` (`alpha`, `beta`...) are set manually.

`BUILD` and `REVISION` are generated automatically:
- Inside Visual Studio, we don't mind if the versions do not change between builds.
- Continuous build or publicly released build should increment them.

We do not change the `AssemblyVersion` attribute when `PATCH` is incremented.

### How to customize the Assembly Version

Remarks:
- This is only mandatory for NuGet-enabled projects.
- Test and sample projects do not have a version property file.

In `src\Versioning`, create a version property file: `Narvalo.XXX.Version.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildThisFileDirectory)DefaultVersion.props" />
</Project>
```

NB: For MVP-related projects use: `DefaultVersion.Mvp.props`.

If you do not want to use the default version properties:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <MajorVersion>1</MajorVersion>
    <MinorVersion>2</MinorVersion>
    <PatchVersion>0</PatchVersion>
    <PreReleaseLabel>alpha</PreReleaseLabel>
  </PropertyGroup>
</Project>
```

### Version updates & NuGet packages

NuGet packages use `AssemblyInformationalVersion` without build metadata
attached: `PackageVersion = MAJOR.MINOR.PATCH(-PreRelaseLabel)`.

**IMPORTANT** Only update the version number immediately before a new release
to the _official_ NuGet repository. Otherwise, versions found in the repository
must **match** the ones found in the NuGet registry.

If two projects use a shared version `DefaultVersion.props`, we need to be very
careful. Let's see how things work with NuGet when Narvalo.YYY depends on
Narvalo.XXX:
- Patch upgrade: `X.Y.0` -> `X.Y.1`
  * If we publish Narvalo.XXX but not Narvalo.YYY, binding redirect works.
  * If we publish Narvalo.YYY but not Narvalo.XXX, even if Narvalo.YYY
    references Narvalo.XXX `X.Y.1`, obviously unknown outside, it doesn't
    matter for the CLR: `AssemblyVersion` _did not actually change_,
    it's still `X.Y.0.0`.
- Major or Minor upgrade: `1.1.0` -> `1.2.0` (or `1.1.0` -> `2.1.0`)
  * If we publish Narvalo.XXX but not Narvalo.YYY, binding redirect works.
  * If we publish Narvalo.YYY but not Narvalo.XXX, we get a **runtime error**
    since Narvalo.YYY references an assembly version unknown outside.
    The solution is obvious: do not change the shared version and configure
    Narvalo.YYY to use a custom version.

Unstable packages (published to [myget](http://www.myget.org)) use the official
version with the PATCH number increased by one and a unique PreReleaseLabel -
all this is done automatically. Their dependencies on other Narvalo packages
are always set to the highest unstable version.

--------------------------------------------------------------------------------

Packaging
---------

Before going further, be sure to read [Versioning](#versioning).

Checklist:
- Update the shared versions: `src\Versioning\DefaultVersion.props` and
  `src\Versioning\DefaultVersion.Mvp.props`.
- Update individual versions in `src\Versioning`, if they differ from the shared
  ones.
- Update the individual READMEs w/ version info.
- Individual package descriptions in `src\Packaging`.
- Check that the property `frameworkAssemblies` in nuspec's do not need any update.
- Check that the `TargetFrameworkVersion` property in the project file and
  the one in the nuspec's must match.
- Check that there is no new resource, eg a satellite assembly for french or
  a Code Contracts assembly.

To release a new version to the _official_ NuGet repository:
1. Create the packages: `make.ps1 -r pack`.
2. Publish them: `publish.ps1 -r`
3. Tag the repository. For instance, for version 1.1.0
```
git tag -a release-1.1.0 -m 'Core Version 1.1.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin core-1.1.0
```

We also publish unstable packages to [myget](https://www.myget.org/gallery/narvalo-edge).
The procedure is almost identical to the one described above with the following
differences:
- Do not update the versions.
- Do not include the command-line option `-r`:
```
make.ps1 pack
publish.ps1
```
**IMPORTANT** Due to the custom behaviour of unstable packages regarding the
dependency on other Narvalo pacakges, always publish all packages together.

--------------------------------------------------------------------------------

Developer Operations
--------------------

### Cleanup the repository

Common cases:
- Remove untracked files and directories: `git clean -nd`
- Remove ignored files and directories: `git clean -ndx -e nuget.exe -e *.user -e .vs -e packages`;
  This command should only delete `bin`, `obj` and `work`.
- Hard cleanup: `git clean -ndx`. ** WARNING:** It will also remove your local
  customizations.

When you are ready, change `-n` to `-f`, otherwise nothing will happen.

Cleanup unnecessary files and optimize the local repository:
`git clean` and every so often `git clean --aggressive`.

### Scripts

- `restore.ps1` to restore solution-level packages.
- `make.ps1` to build, test or package the projects.
- `cover.ps1` to perform test coverage.
- `publish.ps1` to publish the packages.

- `format-code.cmd` format the code w/ [CodeFormatter](https://github.com/dotnet/codeformatter).
- `docs\make.ps1` build the documentation w/ [DocFX](https://dotnet.github.io/docfx/).

### NuGet Updates

Simply use the solution `Narvalo.sln`. Unfortunately, for solution-level packages
(`etc\packages.config`), this must be done manually.

**WARNING:** If the NuGet core framework is updated, do not forget to also
update `tools\nuget.exe` (I believe this is done automatically whenever we use
it to install/update packages):
```
tools\nuget.exe update -Self
```

### Visual Studio and Framework Updates

After upgrading Visual Studio or MSBuild, do not forget to update the
`VisualStudioVersion` property in `Make.Shared.props`.

Other places to look at:
- `ToolsVersion` attribute in all MSBuild and project files.
- `publish.ps1` (for F# updates).
- `TargetFrameworkVersion` in `Narvalo.Common.props`.
- For NuGet packaging check the target lib in the NuGet projects (`src\Packaging`).

### Copyright Year Update

A copyright year appears in three places:
- `etc\AssemblyInfo.Common.cs`
- `LICENSE.txt`
- `tools\Make.CustomAfter.props` (this one is always up-to-date)

--------------------------------------------------------------------------------
