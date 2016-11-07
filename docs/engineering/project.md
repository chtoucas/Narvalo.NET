Adding or Configuring a Project
===============================

Initialize a C# project
-----------------------

The following procedure enables us to centralize all settings into a single place.
Except for Code Contracts, there should be no need to edit the project properties
anymore.

Create a project and add it to the solution `Narvalo.sln`.

**WARNING** When ready for CI, add the project to Make.Foundations.proj.

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

Assembly Versioning
-------------------

See also [Versioning](versioning.md).

This is mandatory only for NuGet projects.
Test and sample projects do not need a version property file.

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

Assembly Informations
---------------------

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
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
```

Overriding the global settings (sample)
---------------------------------------

Create a property file `{AssemblyName}.props` with the following content:
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

Special Cases
-------------

### Portable Class Library

We target at least .NET 4.5, Windows 8 and Windows Phone 8.1:
- **Profile111** (.NET Framework 4.5, Windows 8, Windows Phone 8.1):
  * For MSBuild: `TargetFrameworkVersion=v4.5`.
  * Use this profile if you need `System.Collections.Concurrent.ConcurrentDictionary`.
- **Profile151** (.NET Framework 4.5.1, Windows 8.1, Windows Phone 8.1):
  * For MSBuild: `TargetFrameworkVersion=v4.6`.
  * Use this profile if you need `System.Threading.Timer`.
- **Profile259** (.NET Framework 4.5, Windows 8, Windows Phone 8.1, Windows Phone Silverlight 8):
  * For MSBuild: `TargetFrameworkVersion=v4.5`.

When creating the project we should add it to the list of PCL projects used by SecAnnotate
(see Make.CustomAfter.targets).

See
- Profiles used by the project: `etc/FrameworkProfiles.props`.
- Locally available profiles: `C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\`.
- Stephen Cleary's [blog post](http://blog.stephencleary.com/2012/05/framework-profiles-in-net.html),
  the list of [Portable Class Library profiles](http://embed.plnkr.co/03ck2dCtnJogBKHJ9EjY/preview),
  the [tool](https://github.com/StephenCleary/PortableLibraryProfiles).

### Desktop application

Desktop applications should include a .ini containing:
```
[.NET Framework Debugging Control]
GenerateTrackingInfo=0
AllowOptimize=1
```
Ensure that it is copied to the output directory.

### Test project

To create a test project use the "Unit Test Project" template from Visual Studio.
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

When creating the project we should add it to the list of test projects used by OpenCover in the
PSake file.

### Sample project

Add the following content to you local customization property file `{AssemblyName}.props`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Project ToolsVersion="14.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(RepositorySettingsDir)Samples.props" />
</Project>
```
This has ony one effect:
- Sample projects use a dummy assembly version.
- Sample projects use custom FxCop & StyleCop rules.

StyleCop (obsolete)
-------------------

Unless specified otherwise, a project inherits its StyleCop settings from a common settings file:
- for libraries, tests and tools, `StyleCop.Settings` which link back to `etc\Loosy.SourceAnalysis`.
- for samples, `samples\StyleCop.Settings` which link back to `etc\Empty.SourceAnalysis`.

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

Code Contracts
--------------

When a project is ready for Code Contracts, add the following lines to the
local property file `{AssemblyName}.props`:
```xml
<PropertyGroup Condition=" '$(BuildingInsideVisualStudio)' != 'true' ">
  <CodeContractsReferenceAssembly>Build</CodeContractsReferenceAssembly>
  <CodeContractsEmitXMLDocs>true</CodeContractsEmitXMLDocs>
</PropertyGroup>
```
The part concerning the XML docs is optional (often it causes the build to fail
with the current version of the CC tools).

You MUST also configure VS to build the project when selecting the Code Contracts
configuration.