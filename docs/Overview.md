Overview
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
  for building Code Contracts.

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

There are five solutions.

### Narvalo.sln

This solution contains all projects. This is not used for daily work but rather
for deep refactoring and installing NuGet packages updates/restores.

### Narvalo (Core).sln

This solution contains **all** libraries built upon Narvalo.Core:
- Narvalo.Core itself, a PCL (Profile259) for .NET 4.5, Windows 8, Windows
  Phone 8.1 and Windows Phone Silverlight 8.
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

### Narvalo (NuGet).sln


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
    
  
FxCop
-----


Code Contracts
--------------


