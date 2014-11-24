Overview
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


Project Layout
--------------

- `.nuget`
- `docs`
- `etc`
- `packages`, local repository of NuGet packages.
- `samples`
- `src`
- `tests`
- `tools`

Temporary directories:
- `_build`


Solutions
---------

There are six solutions.

### Narvalo.sln

This solution contains all projects. This is not used for daily work but rather
for deep refactoring and installing NuGet packages updates.

### Narvalo (Core).sln

This solution contains **all** libraries built upon Narvalo.Core:
- Narvalo.Core itself, a PCL (Profile259) for .NET 4.5, Windows 8, Windows
  Phone 8.1 and Windows Phone Silverlight 8.
- Narvalo.Common complements Narvalo.Core with non portable classes.
- Narvalo.Web complements Narvalo.Common with Web centric classes.
- Narvalo.Extras a repository of sample classes depending on external packages.
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
- Narvalo.Ghostscript
- Narvalo.Reliability
- Narvalo.StyleCop.CSharp

### Narvalo (NuGet).sln


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

