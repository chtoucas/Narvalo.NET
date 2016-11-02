Narvalo.NET
===========

## Documentation
- [Project Overview](Overview.md)
- [Project Changelog](Changelog.md)
- [Issues and Roadmap](Issues.md)
- [Developer Guidelines](Guidelines.md)
- [DevOps](BuildAndRelease.md)
- [License](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE.txt)
- [License for WebFormsMvp](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE-WebFormsMvp.txt)
  on which depend all MVP-related packages.

## General Purpose Libraries
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
- **Narvalo.Core**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Core),
  [package](https://www.nuget.org/packages/Narvalo.Core/)),
  this library provides various utilities and extension methods: Range type,
  Int64 encoders, extension methods for Collections and XDom.
- **Narvalo.Common**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Common),
  [package](https://www.nuget.org/packages/Narvalo.Common/)),
  this library provides various utilities and extension methods: directory walker,
  extension methods for Configuration and SQL client.
- **Narvalo.Web**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Web),
  [package](https://www.nuget.org/packages/Narvalo.Web/)),
  this library provides types that might prove useful for Web development: generic HttpHandler
  type, asset providers, Razor and WebForms compile-time optimizers, preliminary support
  for OpenGraph and Schema.Org.
- **Narvalo.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Facts))
  is the test project.

## MVP Framework
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

## Developer Tools
- **Narvalo.Build**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build),
  [package](https://www.nuget.org/packages/Narvalo.Build/)),
  custom MSBuild tasks.

## Other projects
- **Narvalo.T4**, custom T4 templates (for internal use only).
- **Narvalo.StyleCop**, custom StyleCop rules for C# (for internal use only).
- **Narvalo.Mvp.Windows.Forms** (_incomplete & unusable_).
- **Narvalo.Ghostscript**, a .NET wrapper for GhostScript (_incomplete & broken_).
- **Narvalo.Reliability** features reliability patterns (_incomplete & broken_).

## Status for the NuGet packages

Library             | Status | PCL        | CA | GA | CC | SA  | TC
--------------------|--------|------------|----|----|----|-----|-----
Narvalo.Cerbere (*) | Beta   | Profile259 | OK | OK | OK | OK+ | 100%
Narvalo.Fx      (*) | Beta   | Profile259 | OK | OK | OK | OK  |
Narvalo.Finance (*) |        | Profile111 | OK | !  | OK | OK  |
Narvalo.Core        | Alpha  | Profile259 | !  | OK | OK | OK  |
Narvalo.Common      | Alpha  |            | !  | !  | OK | OK  |
Narvalo.Web         |        |            |    |    |    | OK  |
Narvalo.Mvp         | Beta   |            | !  |    |    | OK  |
Narvalo.Mvp.Web     | Beta   |            | !  |    |    | OK  |
Narvalo.Build       | Stable |            | !  |    |    | OK+ |

(*) Not yet published.

Explanations:
- CA: Static Analysis with FxCop
- GA: Static Analysis with Gendarme
- CC: Static Analysis with Code Contracts
- SA: Source Analysis with StyleCop. OK+ means that the assembly is fully documented.
- TC: Code Coverage. OK means > 90%.

### Retired NuGet packages

- [Narvalo.Web.Extras](https://www.nuget.org/packages/Narvalo.Web.Extras/),
  replaced by [Narvalo.Web](https://www.nuget.org/packages/Narvalo.Web/).
