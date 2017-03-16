Developer Guidelines
====================

- [Overview](#overview)
- [Coding Rules](#coding-rules)
- [Adding and Configuring a new Project](#adding-and-configuring-a-new-project)
- [Assembly Versioning](#assembly-versioning)
- [Developer Operations](#developer-operations)

--------------------------------------------------------------------------------

Overview
--------

### Prerequisites

Requirements:
- Visual Studio Community 2017.
- Modeling SDK (option of the Visual Studio installer) needed for T4 integration
  with MSBuild.
- PowerShell v4+ and F# v4+ for the build scripts.

Optional components:
- [DocFX](https://dotnet.github.io/docfx/) to build the documentation.

### Project Layout

- `data`, external data.
- `docs`, documentation.
- `etc`, shared configurations.
- `samples`, sample projects.
- `src`, source directory.
- `src\Packaging`, NuGet projects.
- `tests`, test projects.
- `tools`, build and maintenance scripts.

Temporary directories:
- `packages`, local repository of NuGet packages.
- `work`, temporary directory created during cmdline builds.

There are three solutions:
- `Narvalo.sln` the main solution.
- `samples\MvpSamples.sln` contains sample MVP projects.
- `tools\Maintenance.sln` contains settings and maintenance scripts.

--------------------------------------------------------------------------------

Coding Rules
------------

We mostly follow the guidelines produced by the .NET team:
[CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/)
and [CoreCLR](https://github.com/dotnet/coreclr/blob/master/Documentation/coding-guidelines/).

In addition:
- Consider using tasks: FIXME, HACK, TODO, REVIEW.
- For temporary strings, use `"XXX"`.
- For tests, consider using traits:
  * "Slow" for slow tests.
  * "Unsafe" for unsafe tests (`AppDomain` for instance).

### Code Analysis

All Code Analysis suppressions must be justified and tagged:
- `[Ignore]` Only use this one to tag a false positive.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases.

Every project already load the dictionary `etc\CodeAnalysisDictionary.xml`.
If needed, consider adding a local dictionary `CustomDictionary.xml` in the
directory `Properties` rather than modifying the global one.

--------------------------------------------------------------------------------

Adding and Configuring a new Project
------------------------------------

### Initialize a C# project

The following procedure enables us to centralize all settings into a single place.
Except for Code Contracts, there should be no need to edit the project properties
anymore.

Create a project and add it to `Narvalo.sln` and `Make.proj`.

Edit the project file:
- Add the following line at the bottom of the project file, BEFORE the Microsoft targets:
```xml
<Import Project="..\..\tools\Narvalo.Common.props" />
```
- Remove all sections about Debug, Release and CodeContracts.
- Remove all properties already configured globally.

A typical project file should then look like this:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <ProjectGuid>{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>Narvalo.XXX</RootNamespace>
    <AssemblyName>Narvalo.XXX</AssemblyName>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    ...
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Properties\AssemblyInfo.cs" />
  </ItemGroup>
  <Import Project="..\..\tools\Narvalo.Common.props" />
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
```

Clean up the assembly information file:
```csharp
// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("The assembly title.")]
[assembly: AssemblyDescription("The assembly description.")]
```

Optionally, give access to internals to the test project:
```csharp
#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.XXX.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
```

### Customize the Assembly Version

See also [Versioning](#assembly-versioning).
- This is mandatory only for NuGet projects.
- Test and sample projects do not need a version property file.

Create a version property file: `{AssemblyName}.Version.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Narvalo.CurrentVersion.props" />
</Project>
```

When you want to override the default version properties,
create a property file `{AssemblyName}.Version.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <MajorVersion>1</MajorVersion>
    <MinorVersion>2</MinorVersion>
    <PatchVersion>0</PatchVersion>
    <PreReleaseLabel></PreReleaseLabel>
  </PropertyGroup>
</Project>
```

### Override the global settings (sample)

Create a property file `{AssemblyName}.props` with the following content:
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

See also: `etc/FrameworkProfiles.props`.

#### Desktop application

Desktop applications should include a .ini containing:
```
[.NET Framework Debugging Control]
GenerateTrackingInfo=0
AllowOptimize=1
```
Ensure that it is copied to the output directory.

#### Test project

To create a test project use the "Class Library" template from Visual Studio.
Add the following content to you local customization property file `{AssemblyName}.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Tests.props" />
</Project>
```
This has two consequences:
- Test projects use a dummy assembly version.
- Test projects use custom FxCop rules.

Reference the shared project `TestCommon`.

#### Sample project

Add the following content to you local customization property file `{AssemblyName}.props`:
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

Assembly Versioning
-------------------

Reminder:
- AssemblyVersion, version used by the runtime.
- AssemblyFileVersion, version as seen in the file explorer,
  also used to uniquely identify a build.
- AssemblyInformationalVersion, the product version. In most cases
  this is the version I shall use for NuGet package versioning.
  This attribute follows semantic versioning rules.

We use the following:
- AssemblyVersion = MAJOR.MINOR.0.0
- AssemblyFileVersion = MAJOR.MINOR.BUILD.REVISION
- AssemblyInformationalVersion = MAJOR.MINOR.PATCH(-PreRelaseLabel)

MAJOR, MINOR, PATCH and PreRelaseLabel (alpha, beta...) are manually set.

BUILD and REVISION are generated automatically:
- Inside Visual Studio, I don't mind if the versions do not change between builds.
- Continuous build or publicly released build should increment it.

I do not change the AssemblyVersion when PATCH is incremented.

### Version updates

Let's see if things work with NuGet:
- Patch update: X.Y.0.0 -> X.Y.1.0
  * If I publish Narvalo.Core but not Narvalo.Common, binding redirect works
    for Narvalo.Core and Narvalo.Common can reference the newly published assembly.
  * If I publish Narvalo.Common but not Narvalo.Core, even if Narvalo.Common
    references Narvalo.Core X.Y.1.0, obviously unknown outside, it doesn't
    matter for the CLR: the assembly version _did not actually change_,
    it's still X.Y.0.0.
- Major or Minor upgrade: 1.1.0.0 -> 1.2.0.0 (or 1.1.0.0 -> 2.1.0.0)
  * If I publish Narvalo.Core but not Narvalo.Common, binding redirect works.
    Let's cross fingers that I did not make a mistake by not releasing
    Narvalo.Common too.
  * If I publish Narvalo.Common but not Narvalo.Core, we get a runtime error
    since Narvalo.Common references an assembly version unknown outside my
    development environment. The solution is obvious. Narvalo.Core has not
    changed so Narvalo.Common should replace the direct reference and use
    the NuGet package for Narvalo.Core. If necessary we can roll back
    at any time and next time we must publish both packages.

--------------------------------------------------------------------------------

Developer Operations
--------------------

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
`VisualStudioVersion` property in Make.Shared.props.

Other places to look at:
- fsci.cmd (for F# updates)
- `TargetFrameworkVersion` in Narvalo.Common.props
- For NuGet packaging check the target lib in the NuGet projects (src\Packaging).

### Copyright Year Update

A copyright appears in three places:
- `etc\AssemblyInfo.Common.cs`
- `LICENSE.txt`
- `tools\Make.CustomAfter.props` (this one is always up-to-date)

### Releasing a NuGet package

Before going further, be sure to read [Versioning](#assembly-versioning).

Checklist:
- The shared version: `etc\Narvalo.CurrentVersion.props`.
- Individual versions in `src\NuGet`, if they differ from the shared one.
- Individual package descriptions in `src\NuGet`.
- Check that the property "frameworkAssemblies" in nuspec's do not need any update.
- "TargetFrameworkVersion" in projects and the one in the nuspec's must match.

To release a new version of a package (for instance the core packages):
1. Build the packages: `make.ps1 -r pack`.
2. Publish the packages: `tools\publish-retail.fsx`
3. Tag the repository. For instance, for version 1.1.0
```
git tag -a release-1.1.0 -m 'Core Version 1.1.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin core-1.1.0
```
