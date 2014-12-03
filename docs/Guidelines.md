Guidelines
==========

How to initiale a new project
-----------------------------

Principle:
- Debug Build is optimized for development.
- Release Build is optimized for safety.

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

Add the following line at the bottom:
```xml
<Import Project="..\..\tools\Narvalo.Common.targets" />
```

Add the project to Narvalo.proj when it is stable.

### Configure StyleCop for Visual Studio

Edit the local StyleCop settings and link it to "etc\Settings.SourceAnalysis".

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

### Assembly versions

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
    matter for the CLR: the assembly version __did not actually change__,
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

### Global suppressions

- [FIXME]
- [GeneratedCode] to mark a suppression related to generated code.

### Remarks

Narvalo.Core includes a Settings.StyleCop file with actual rules that mirror
the content of "etc\Settings.SourceAnalysis".

The following projects use the default ruleset for Code Analysis.
- Narvalo.Junk
- DocuMaker
- Playground


Coding Style
------------

- Max line-width 100 characters.
- Passes all StyleCop rules.
- Remove and sort usings.
- Use a minimal set of references.


Code Contracts
--------------

### Object Invariants

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

NuGet Updates
-------------

For package updates, use the Narvalo (All).sln solution.

**WARNING** If the NuGet core framework is updated, do not forget to update
`tools\NuGet\nuget.exe`:
```
tools\NuGet\nuget.exe update self
```

Periodic checklist
------------------

Task List
- FIXME
- TODO
- REVIEW

Global suppression files.

Publishing
----------


Appendices
----------

### Strong Name Key

Create a key pair: `sn -k Application.snk`.

Extract the public key: `sn -p Application.snk Application.pk`.

Extract the public key token: `sn -tp Application.pk > Application.txt`.


References
----------

+ [Strong Name Tool](http://msdn.microsoft.com/en-us/library/k5b5tt23.aspx)
