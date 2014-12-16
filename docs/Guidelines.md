Guidelines
==========
                    
Prerequisites
-------------

- [Visual Studio Community 2013](http://msdn.microsoft.com/en-us/visual-studio-community-vs.aspx)

Optional components:
- [StyleCop](http://stylecop.codeplex.com) for source analysis integration
  inside Visual Studio.
- [xUnit.net runner for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/463c5987-f82b-46c8-a97e-b1cde42b9099)
  for xUnit integration inside Visual Studio.
- [Code Contracts extension for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970)
  for using Code Contracts.

Components necessary to run the build scripts:
- Code Contracts (see above).
- [Microsoft Visual Studio 2013 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=40758),
  Prerequisite for the Modeling SDK (see below).
- [Modeling SDK for Microsoft Visual Studio 2013](http://www.microsoft.com/en-us/download/details.aspx?id=40754),
  provides T4 integration in MSBuild.    
- Microsoft .NET 4.5.1 Developer Pack(?),  Microsoft Windows SDK for Windows 8.1(?) 
  or .NET Framework SDK for PEVerify.exe.


Project Layout
--------------

- `.nuget`, NuGet configurations.
- `docs`
- `etc`, directory of settings.
- `packages`, local repository of NuGet packages.
- `samples`
- `src`
- `tests`
- `tools`

Temporary directories:
- `work`


Solutions
---------

There are four solutions.

### Narvalo.sln

This solution contains all projects. This is not used for daily work but rather
for deep refactoring and installing NuGet packages updates/restores.

### Narvalo (Core).sln

This solution contains **all** libraries built upon Narvalo.Core:
- Narvalo.Core itself, a PCL (Profile259).
- Narvalo.Common complements Narvalo.Core with non portable classes.
- Narvalo.Web complements Narvalo.Common with Web centric classes.
- Narvalo.Extras.
- Narvalo.Benchmarking.
- Narvalo.Facts, the test project.
- Narvalo.Junk, a "fourre-tout" of unfinished or broken codes.

### Narvalo (Mvp).sln

This solution contains all MVP related libraries. It **does not** depend on any
of the core libraries.
- Narvalo.Mvp
- Narvalo.Mvp.Web
- Narvalo.Mvp.Windows.Forms
- Narvalo.Mvp.Extras
- Narvalo.Mvp.Facts, the test project.

### Narvalo (Miscs).sln

This solution contains anything else. It **does not** depend on any
of the core libraries.
- Narvalo.Build
- Narvalo.Externs
- Narvalo.Ghostscript
- Narvalo.Reliability
- Narvalo.StyleCop.CSharp 

             
Periodic Checklist
------------------
       
### Unfinished Work

Task List
- FIXME
- TODO
- REVIEW
- XXX

Global suppression files.

### NuGet Updates

For package updates, use the Narvalo.sln solution.

**WARNING** If the NuGet core framework is updated, do not forget to update
`tools\NuGet\nuget.exe`:
```
tools\NuGet\nuget.exe update self
```

### Visual Studio or Framework Updates

When upgrading VS, do not forget to update the default VisualStudioVersion 
property in Shared.props and in build.cmd.
We might also need to update the SDK40ToolsPath property.


Coding Style
------------
         
### General

- Directories mirror namespaces.
- One class per file.
- Internal classes MUST be in a subdirectory named "Internal".
- Optional extensions may be in a subdirectory named "Extensions".
- Max line-width 100 characters.
- Remove and sort usings.
- Projects should use a minimal set of references.
   
### StyleCop

Narvalo.Core includes a Settings.StyleCop file with actual rules that mirror
the content of "etc\Strict.SourceAnalysis".

For a detailed description of the rules, check out http://www.stylecop.com/docs/.

_Documentation rules are temporary disabled._

Disabled rules:
- SA1101:PrefixLocalCallsWithThis
- SA1121:UseBuiltInTypeAlias
- SA1126:PrefixCallsCorrectly
- SA1306:FieldNamesMustBeginWithLowerCaseLetter
- SA1309:FieldNamesMustNotBeginWithUnderscore
- SA1310:FieldNamesMustNotContainUnderscore
- SA1400:AccessModifierMustBeDeclared
- SA1500:CurlyBracketsForMultiLineStatementsMustNotShareLine
- SA1501:StatementMustNotBeOnASingleLine
- SA1502:ElementMustNotBeOnASingleLine
- SA1634:FileHeaderMustShowCopyright
    

Code Quality
------------

### FxCop
                   
The following projects use the default ruleset for Code Analysis.
- Narvalo.Junk
- DocuMaker
- Playground
    
### Code Contracts

#### Object Invariants

Wrap any object invariants method with a compiler conditional clause :
```csharp
#if CONTRACTS_FULL
    [ContractInvariantMethod]
    void ObjectInvariants()
    {
        // Contract invariants directives.
    }
#endif
```


Build Infrastructure
--------------------

Cf. http://msdn.microsoft.com/en-us/library/dd393574.aspx


How to initialize a new project
-----------------------------

Principle:
- Debug configuration is optimized for development.
- Release configuration is optimized for safety.

### Add relevant files as linked files to the "Properties" folder

- Shared assembly informations: `etc\AssemblyInfo.Common.cs`
- Code Analysis dictionary: `etc\CodeAnalysisDictionary.xml`
  with build action _CodeAnalysisDictionary_
- Strong Name Key: `etc\Narvalo.snk`

### Edit the project Properties

In Debug mode:
- "Build" panel, treat all warnings as errors.
- "Build" panel, check for arithmetic overflow/underflow.
- "Code Analysis" panel, use "Narvalo Rules".

In Release mode:
- "Build" panel, suppress compiler warnings 1591.
- "Build" panel, treat all warnings as errors.
- "Build" panel, output XML documentation file.
- "Code Analysis" panel, use "Narvalo Rules" and enable Code Analysis
  on Build.

For all modes:
- "Signing" panel, sign the assembly using the "etc\Narvalo.snk" key.
- "Build" panel, uncheck "Prefer 32-bit" if checked.
- "Code Analysis" panel, uncheck "Suppress results from generated code (managed only)".

### Edit the project file

Add the following line at the top of the project file:    
```xml
<Import Project="..\..\tools\Narvalo.Common.props" />
```
and at the bottom
```xml
<Import Project="..\..\tools\Narvalo.Common.targets" />
```

Add the project to Narvalo (Public).proj when it is ready for publication.

### Configure StyleCop for Visual Studio

Edit the local StyleCop settings and link it to "etc\Strict.SourceAnalysis".

These settings only affect StyleCop when run explicitly from within Visual Studio.
During Build, StyleCop is called from `Narvalo.Common.targets`.

### Special Cases

Desktop applications should include a .ini containing:
```
[.NET Framework Debugging Control]
GenerateTrackingInfo=0
AllowOptimize=1
```
Ensure that it is copied to the output directory.

### Global suppressions

- [FIXME]
- [GeneratedCode] to mark a suppression related to generated code.


Compilation Symbols
-------------------

- DEBUG
- TRACE
- CODE_ANALYSIS
- CONTRACTS_FULL

- NET_35
- NET_40

- BUILD_GENERATED_VERSION
- DUMMY_GENERATED_VERSION
- NO_INTERNALS_VISIBLE_TO
- SIGNED_ASSEMBLY


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
