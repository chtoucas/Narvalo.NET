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


How to initiale a new project
-----------------------------

### Add relevant files as linked files to the "Properties" folder

- Shared informations: `etc\AssemblyInfo.Common.cs`
- Library specific informations:
    * for a core library: `etc\AssemblyInfo.Library.cs`
    * for a misc library: `etc\AssemblyInfo.Library.Miscs.cs`
    * for a MVP library: `etc\AssemblyInfo.Library.Mvp.cs`
    * for a test project: `etc\AssemblyInfo.Facts.cs`
- Version informations:
    * for a core library: `etc\AssemblyInfo.Version.cs`
    * for a MVP library: `etc\AssemblyInfo.Version.Mvp.cs`
- Code Analysis dictionary: `etc\CodeAnalysisDictionary.xml`
  with build action _CodeAnalysisDictionary_
- Strong Name Key: `etc\Narvalo.snk`
- Cleanup AssemblyInfo.cs

### Edit the project Properties

In Debug mode:
- "Build" panel, treat all warnings as errors.
- "Build" panel, check for arithmetic overflow/underflow.
- "Code Analysis" panel, use "Narvalo Debug Rules".

In Release mode:
- "Build" panel, suppress compiler warnings 1591.
- "Build" panel, treat all warnings as errors.
- "Build" panel, output XML documentation file.
- "Code Analysis" panel, use "Narvalo Release Rules" and enable Code Analysis
  on Build.

For all modes:
- "Signing" panel, sign the assembly using the "etc\Narvalo.snk" key.

### Configure StyleCop

Edit the local StyleCop settings to link to "etc\Settings.SourceAnalysis".
For test projects, simply unselect all rules (for now).

###

Edit the project file and add the following line:
```xml
<Import Project="..\..\tools\Narvalo.Common.targets" />
```

Add the project to Narvalo.proj.


Coding Style
------------

- Max line-width 100 characters.
- Passes all StyleCop rules.

### StyleCop Rules

For a detailed description of the rules, check out http://www.stylecop.com/docs/.

Permanently disabled rules:
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

Temporary disabled documentation rules:
- SA1600:ElementsMustBeDocumented
- SA1601:PartialElementsMustBeDocumented
- SA1602:EnumerationItemsMustBeDocumented
- SA1604:ElementDocumentationMustHaveSummary
- SA1606:ElementDocumentationMustHaveSummaryText
- SA1611:ElementParametersMustBeDocumented
- SA1615:ElementReturnValueMustBeDocumented
- SA1618:GenericTypeParametersMustBeDocumented
- SA1623:PropertySummaryDocumentationMustMatchAccessors
- SA1633:FileMustHaveHeader
- SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText
- SA1650:ElementDocumentationMustBeSpelledCorrectly


NuGet Updates
-------------

For package updates, use the Narvalo (All).sln solution.

**WARNING** If the NuGet core framework is updated, do not forget to update
`tools\NuGet\nuget.exe`:
```
tools\NuGet\nuget.exe update self
```


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
