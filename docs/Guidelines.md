Guidelines
==========

How to initiale a new project
-----------------------------

Principle:
- Debug Build is optimized for development.
- Release Build is optimized for safety.

### Add relevant files as linked files to the "Properties" folder

- Shared assembly informations: `etc\AssemblyInfo.Common.cs`
- Library specific informations:
  * for a core library: `etc\AssemblyInfo.Core.cs`
  * for a MVP library: `etc\AssemblyInfo.Mvp.cs`
- Code Analysis dictionary: `etc\CodeAnalysisDictionary.xml`
  with build action _CodeAnalysisDictionary_
- Strong Name Key: `etc\Narvalo.snk`
- Cleanup AssemblyInfo.cs

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

Add the project to Narvalo.proj.

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
- AssemblyFileVersion, version seen in the file explorer.
- AssemblyInformationalVersion, version used by NuGet.

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
