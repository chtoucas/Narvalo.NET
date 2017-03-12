Adding or Configuring a Project
===============================

Initialize a C# project
-----------------------

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

Assembly Versioning
-------------------

See also [Versioning](versioning.md).

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
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.XXX.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
```

Overriding the global settings (sample)
---------------------------------------

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

Special Cases
-------------

### Portable Class Library

The long term goal is to move to .NET Standard projects.

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

### Desktop application

Desktop applications should include a .ini containing:
```
[.NET Framework Debugging Control]
GenerateTrackingInfo=0
AllowOptimize=1
```
Ensure that it is copied to the output directory.

### Test project

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
- Sample projects use custom FxCop rules.
