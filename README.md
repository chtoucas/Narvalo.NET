Narvalo.NET
===========

#### Documentation
- [Project Overview](https://github.com/chtoucas/Narvalo.NET/tree/master/docs/Overview.md)
- [Project Changelog](https://github.com/chtoucas/Narvalo.NET/tree/master/docs/Changelog.md)
- [Issues and Roadmap](https://github.com/chtoucas/Narvalo.NET/tree/master/docs/Issues.md)
- [Developer Guidelines](https://github.com/chtoucas/Narvalo.NET/tree/master/docs/Guidelines.md)
- [DevOps](https://github.com/chtoucas/Narvalo.NET/tree/master/docs/BuildAndRelease.md)
- [Developer Tips](https://github.com/chtoucas/Narvalo.NET/tree/master/docs/Tips.md)
- [License](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE.txt)
- [License for WebFormsMvp](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE-WebFormsMvp.txt)

#### General Purpose Libraries
- **Narvalo.Cerbere**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Cerbere)),
  this library provides argument validation methods and Code Contracts helpers.
- **Narvalo.Fx**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Fx)),
  this library features implementations of some of the usual suspects from functional
  programming: Option (`Maybe<T>`) and Error (`Output<T>`) monads, simple pattern matching
  (`Either<T1, T2>`, `Switch<T1, T2>`), generators and delegate extensions.
- **Narvalo.Finance**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Finance)),
  this package provides various financial utilities: Currency (ISO 4217), Money types,
  BIC (ISO 9362), IBAN & BBAN.
- **Narvalo.Common**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Common),
  [package](https://www.nuget.org/packages/Narvalo.Common/)),
  this library provides various utilities and extension methods: Range type, directory walker,
  Int64 encoders,  benchmark helpers, extension methods for Collections, Configuration, SQL client and XDom.
- **Narvalo.Web**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Web),
  [package](https://www.nuget.org/packages/Narvalo.Common/)),
  this library provides types that might prove useful for Web development: generic HttpHandler
  type, asset providers, Razor and WebForms compile-time optimizers, preliminary support
  for OpenGraph and Schema.Org.
- **Narvalo.Externs**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Externs)),
  this project contains sample codes for Autofac, Serilog, Castle Windsor...
- **Narvalo.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Facts))
  is the test project.

#### MVP Framework
- **Narvalo.Mvp**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp),
  [package](https://www.nuget.org/packages/Narvalo.Mvp/)),
  a simple MVP framework largely inspired by [WebFormsMvp](https://github.com/webformsmvp/webformsmvp).
  Contrary to WebFormsMvp, it is not restricted to the WebForms platform; nevertheless, featurewise,
  it should be on par with WebFormsMvp. Includes support for command-line applications.
- **Narvalo.Mvp.Web**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp.Web),
  [package](https://www.nuget.org/packages/Narvalo.Mvp.Web/)),
  enhances Narvalo.Mvp to provide support for ASP.NET WebForms similar to WebFormsMvp.
- [WebForms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWebForms)
- **Narvalo.Mvp.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Mvp.Facts))
  is the test project.

#### Developer Tools
- **Narvalo.Build**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build),
  [package](https://www.nuget.org/packages/Narvalo.Build/)),
  custom MSBuild tasks.

#### Other projects
- **Narvalo.Mvp.Windows.Forms**
- **Narvalo.Ghostscript**, a .NET wrapper for GhostScript.
- **Narvalo.Reliability** features reliability patterns.
- **Narvalo.PowerShell**, collection of PowerShell modules.
- **Narvalo.StyleCop.CSharp**, custom C# rules for StyleCop.
- **Narvalo.Brouillons**, a "fourre-tout" of unfinished or severely broken codes.
- **Narvalo.Benchmarks**, the benchmarking project.

#### Status for the NuGet packages

For Code Contracts, StyleCop & Documentation, OK does not mean finished.

Library             | Status | PCL        | Security    | CA | GA | CC | SA |
--------------------|--------|------------|-------------|----|----|----|----|
Narvalo.Cerbere     | Beta   | Profile259 | Transparent | OK | OK | OK | OK |
Narvalo.Fx          | Alpha  | Profile259 | Transparent | OK | OK | OK | OK |
Narvalo.Finance     |        | Profile111 | Transparent | OK | !  | OK | !  |
Narvalo.Common      | Alpha  |            | APTCA       | !  | !  | !  | !  |
Narvalo.Web         |        |            |             |    |    |    |    |
Narvalo.Mvp         | Beta   |            |             |    |    |    |    |
Narvalo.Mvp.Web     | Beta   |            |             |    |    |    |    |
Narvalo.Build       |        |            |             |    |    |    |    |

- CA: Code Analysis with FxCop
- GA: Code Analysis with Gendarme
- CC: Code Contracts
- SA: Source Analysis with StyleCop

#### Dead NuGet packages

- Narvalo.Core, replaced by Narvalo.Cerbere and Narvalo.Fx.
- Narvalo.Web.Extras, replaced by Narvalo.Web.