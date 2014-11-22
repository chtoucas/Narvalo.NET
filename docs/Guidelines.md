Guidelines
==========


Prerequisites
-------------

- [Visual Studio Community 2013](http://msdn.microsoft.com/en-us/visual-studio-community-vs.aspx)
- Microsoft .NET 4.5.1 Developer Pack (?)
- F# Compiler (?)

Optional components:
- [StyleCop](http://stylecop.codeplex.com) for source analysis integration
  inside Visual Studio.
- [xUnit.net runner for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/463c5987-f82b-46c8-a97e-b1cde42b9099)
- Code Contracts extension for Visual Studio


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
- Narvalo.DocuMaker
- Narvalo.Edu
- Narvalo.Junk
- Playground
- Playground.Benchmarks


Coding Style
------------

- Max line-width 100 characters.
- Passes all StyleCop rules.
- Remove and sort usings.
- Use a minimal set of references.


FxCop
-----


StyleCop
--------

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


Code Contracts
--------------

We define the conditional compilation symbol CONTRACTS_CODEANALYSIS 
when CODE_ANALYSIS and CONTRACTS_FULL are both defined.

### Object Invariants

Wrap any object invariants method with a compiler conditional clause :
```csharp
#if CONTRACTS_CODEANALYSIS
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
