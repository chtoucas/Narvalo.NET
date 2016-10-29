Overview
========

Technology footprint
--------------------

- Most developments are done in C#.
- Static and quality analysis are done with StyleCop, FxCop, Gendarme, Code Contracts
  and a tailor-made script.
- All tasks are fully automated with MSBuild, PowerShell (PSake) and F# scripts.

Extra care has been taken to completely isolate the CI environment and to keep the development
inside Visual Studio as smooth and swift as they can be.

Prerequisites
-------------

- [Visual Studio Community 2015](http://msdn.microsoft.com/en-us/visual-studio-community-vs.aspx)
- PowerShell

Optional components:
- [Code Contracts extension for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970)
- [.NET Portability Analyzer](https://visualstudiogallery.msdn.microsoft.com/1177943e-cfb7-4822-a8a6-e56c7905292b)
  to help evaluate portability across .NET platforms.
- .NET Framework 3.5 required. The MSBuild file for Code Contracts requires MSBuild v3.5.

Components necessary to run the build scripts:
- Code Contracts (see above).
- Visual Extensibility Tools provides T4 integration in MSBuild.

Prerequisites (Old)
-------------------

- [Visual Studio Community 2013](http://msdn.microsoft.com/en-us/visual-studio-community-vs.aspx)
- PowerShell v3

Optional components:
- [StyleCop](http://stylecop.codeplex.com) for source analysis integration inside Visual Studio.
- [Code Contracts extension for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970)
- [.NET Portability Analyzer](https://visualstudiogallery.msdn.microsoft.com/1177943e-cfb7-4822-a8a6-e56c7905292b)
  to help evaluate portability across .NET platforms.

Components necessary to run the build scripts:
- Code Contracts (see above).
- [Microsoft Visual Studio 2013 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=40758),
  prerequisite for the Modeling SDK (see below).
- [Modeling SDK for Microsoft Visual Studio 2013](http://www.microsoft.com/en-us/download/details.aspx?id=40754),
  provides T4 integration in MSBuild.
- Microsoft .NET 4.5.1 Developer Pack(?),  Microsoft Windows SDK for Windows 8.1(?)
  or .NET Framework SDK for PEVerify.exe.

Project Layout
--------------

- `.nuget`, NuGet configuration.
- `docs`, documentation.
- `etc`, shared configurations.
- `packages`, local repository of NuGet packages.
- `samples`, sample projects.
- `src`, source directory.
- `src\NuGet`, NuGet projects.
- `tests`, test projects.
- `tools`, build and maintenance scripts.
- `work`, temporary directory created during builds.

Solutions
---------

There are two solutions.
- `Narvalo.sln` contains all projects.
- `tools\Narvalo Maintenance.sln` contains documentation, settings, maintenance scripts and projects:
  * MyGet, private NuGet server.
  * NuGetAgent, a NuGet publishing tool.
  * Prose, a literal programming tool.

How to initialize a new project
-------------------------------

The following procedure enables us to centralize all settings into a single place.
Except for Code Contracts, there should be no need to edit the project properties anymore.
For more the details about this shared configuration, see the section "Global settings" below.

Create a project and add it to the solution `Narvalo.sln`.

Edit the project file:
- Add the following line at the bottom of the project file, just BEFORE the Microsoft targets:
```xml
<Import Project="..\..\tools\Narvalo.Common.props" />
```
- Remove all sections about Debug, Release and CodeContracts.
- Remove all properties configured globally.

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

### Add Versioning

This is mandatory only for NuGet projects.
Test and sample projects do not need a version property file.

Create a version property file: `{ProjectName}.Version.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Narvalo.CurrentVersion.props" />
</Project>
```

When you want to override the default version properties:
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

### Assembly Information

Clean up the assembly information file:
```csharp
// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("The assembly title.")]
[assembly: AssemblyDescription("The assembly description.")]
```

Optionally, give access to internals to the test project:
```csharp
using System.Runtime.CompilerServices;

#if !NO_INTERNALS_VISIBLE_TO
[assembly: InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
```

### Overriding the global settings

Create a property file `{ProjectName}.props` with the following content (this is just a sample):
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <SourceAnalysisOverrideSettingsFile>$(RepositorySettingsDir)Custom.SourceAnalysis</SourceAnalysisOverrideSettingsFile>
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

#### Portable Class Libraries
We target at least .NET 4.5, Windows 8 and Windows Phone 8.1:
- **Profile111** (.NET Framework 4.5, Windows 8, Windows Phone 8.1):
  * For MSBuild: `TargetFrameworkVersion=v4.5`.
  * Use this profile if you need `System.Collections.Concurrent.ConcurrentDictionary`.
- **Profile151** (.NET Framework 4.5.1, Windows 8.1, Windows Phone 8.1):
  * For MSBuild: `TargetFrameworkVersion=v4.6`.
  * Use this profile if you need `System.Threading.Timer`.
- **Profile259** (.NET Framework 4.5, Windows 8, Windows Phone 8.1, Windows Phone Silverlight 8):
  * For MSBuild: `TargetFrameworkVersion=v4.5`.

See
- Profiles used by the project: `etc/FrameworkProfiles.props`.
- Locally available profiles: `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\`.
- Stephen Cleary's [blog post](http://blog.stephencleary.com/2012/05/framework-profiles-in-net.html),
  the list of [Portable Class Library profiles](http://embed.plnkr.co/03ck2dCtnJogBKHJ9EjY/preview),
  the [tool](https://github.com/StephenCleary/PortableLibraryProfiles).

#### Desktop applications
Desktop applications should include a .ini containing:
```
[.NET Framework Debugging Control]
GenerateTrackingInfo=0
AllowOptimize=1
```
Ensure that it is copied to the output directory.

#### Test projects
To create a test project use the "Unit Test Project" template from Visual Studio.
Add the following content to you local customization property file `{ProjectName}.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Tests.props" />
</Project>
```
This has two consequences:
- Test projects use a dummy assembly version.
- Test projects use custom FxCop rules.

#### Sample projects
Add the following content to you local customization property file `{ProjectName}.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Samples.props" />
</Project>
```
This has ony one effect:
- Sample projects use a dummy assembly version.
- Sample projects use custom FxCop & StyleCop rules.

### StyleCop

Unless specified otherwise, a project inherits its StyleCop settings from a common settings file:
- for libraries, tests and tools, `StyleCop.Settings` which link back to `etc\Loosy.SourceAnalysis`.
- for samples, `samples\StyleCop.Settings` which link back to `etc\Empty.SourceAnalysis`.
- Narvalo.Brouillons links back to `etc\Empty.SourceAnalysis`.

This settings mirror what is done in the shared (MSBuild) property file.

There are three ways to run StyleCop:
- Inside VS from the menu, using the inherited settings from `StyleCop.Settings`.
- Inside VS, for Release builds, using the settings defined in the shared property file.
- From CI builds when `SourceAnalysisEnabled` is `true`, using the settings defined
  in the shared property file.

Remarks:
- `etc\Loosy.SourceAnalysis` includes all rules except the documentation ones.
- `etc\Empty.SourceAnalysis` disables all rules.
- `etc\Strict.SourceAnalysis` enables all rules.
- When documentation is completed, override the project properties as follows:
```xml
<PropertyGroup>
    <SourceAnalysisOverrideSettingsFile>$(RepositorySettingsDir)Strict.SourceAnalysis</SourceAnalysisOverrideSettingsFile>
</PropertyGroup>
```
  but we do not override the settings file `Settings.StyleCop` which implies that it
  does not affect StyleCop when called from the menu.

### Code Contracts

When a project is ready for Code Contracts, add the following lines to the
local property file `{ProjectName}.props`:
```xml
<PropertyGroup Condition=" '$(BuildingInsideVisualStudio)' != 'true' ">
  <CodeContractsReferenceAssembly>Build</CodeContractsReferenceAssembly>
</PropertyGroup>
```

Update external dependencies
----------------------------

### NuGet Updates

For package updates, use the Narvalo.sln solution.

**WARNING:** If the NuGet core framework is updated, do not forget to also update `tools\nuget.exe`:
```
tools\nuget.exe update self
```

### Visual Studio or Framework Updates

After upgrading Visual Studio or MSBuild, do not forget to update the `VisualStudioVersion` property
in both Make.Shared.props, PSakefile.ps1 and MSBuild.cmd. We might also need to update the
`SDK40ToolsPath` property.

Appendices
----------

### Assembly Versioning

- AssemblyVersion, version used by the runtime.
  MAJOR.MINOR.0.0
- AssemblyFileVersion, version as seen in the file explorer,
  also used to uniquely identify a build.
  MAJOR.MINOR.BUILD.REVISION
- AssemblyInformationalVersion, the product version. In most cases
  this is the version I shall use for NuGet package versioning.
  This attribute follows semantic versioning rules.
  MAJOR.MINOR.PATCH(-PreRelaseLabel)

MAJOR, MINOR, PATCH and PreRelaseLabel (alpha, beta) are manually set.

BUILD and REVISION are generated automatically:
- Inside Visual Studio, I don't mind if the versions do not change between builds.
- Continuous build or publicly released build should increment it.

I do not change the AssemblyVersion when PATCH is incremented.

All core Narvalo projects use the same version, let's see if things work with NuGet:
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

### Strong Name Key

- Create a key pair: `sn -k Narvalo.snk`.
- Extract the public key: `sn -p Narvalo.snk Narvalo.pk`.
- Extract the public key token: `sn -tp Narvalo.pk > Narvalo.txt`.

References:
- [Strong Name Tool](http://msdn.microsoft.com/en-us/library/k5b5tt23.aspx)
