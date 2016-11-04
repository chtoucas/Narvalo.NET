Projects
========

## General Purpose Libraries
- **Narvalo.Cerbere**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Cerbere),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Cerbere.svg)](https://www.nuget.org/packages/Narvalo.Cerbere/)),
  this library provides argument validation methods and Code Contracts helpers.
- **Narvalo.Fx**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Fx),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)),
  this library features implementations of some of the usual suspects from functional
  programming: Option (`Maybe<T>`) and Error (`Output<T>`) monads, simple pattern matching
  (`Either<T1, T2>`, `Switch<T1, T2>`), generators and delegate extensions.
- **Narvalo.Finance**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Finance),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Finance.svg)](https://www.nuget.org/packages/Narvalo.Finance/)),
  this package provides various financial utilities: Currency (ISO 4217), Money types,
  BIC (ISO 9362), IBAN & BBAN.
- **Narvalo.Core**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Core),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Core.svg)](https://www.nuget.org/packages/Narvalo.Core/)),
  this library provides various utilities and extension methods: Range type,
  Int64 encoders, extension methods for Collections and XDom.
- **Narvalo.Common**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Common),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Common.svg)](https://www.nuget.org/packages/Narvalo.Common/)),
  this library provides various utilities and extension methods: directory walker,
  extension methods for Configuration and SQL client.
- **Narvalo.Web**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Web),
  [package](https://www.nuget.org/packages/Narvalo.Web/)),
  this library provides types that might prove useful for Web development: generic HttpHandler
  type, asset providers, Razor and WebForms compile-time optimizers, preliminary support
  for OpenGraph and Schema.Org.

## MVP Framework
- **Narvalo.Mvp**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Mvp.svg)](https://www.nuget.org/packages/Narvalo.Mvp/)),
  a simple MVP framework largely inspired by [WebFormsMvp](https://github.com/webformsmvp/webformsmvp).
  Contrary to WebFormsMvp, it is not restricted to the WebForms platform; nevertheless, featurewise,
  it should be on par with WebFormsMvp. Includes support for command-line applications.
- **Narvalo.Mvp.Web**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp.Web),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Mvp.Web.svg)](https://www.nuget.org/packages/Narvalo.Mvp.Web/)),
  enhances Narvalo.Mvp to provide support for ASP.NET WebForms similar to WebFormsMvp.

Samples:
- [Command-Line MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpCommandLine)
- [WebForms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWebForms)
- [Windows Forms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWindowsForms)

## Developer Tools
- **Narvalo.Build**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build),
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Build.svg)](https://www.nuget.org/packages/Narvalo.Build/)),
  custom MSBuild tasks.

## Test Projects
- **Narvalo.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Facts))
- **Narvalo.Mvp.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Mvp.Facts))
- **Narvalo.Reliability.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Reliability.Facts))

## Other projects
- **Narvalo.T4**, custom T4 templates (for internal use only).
- **Narvalo.StyleCop**, custom StyleCop rules for C# (for internal use only).
- **Narvalo.Mvp.Windows.Forms** (_incomplete & unusable_).
- **Narvalo.Ghostscript**, a .NET wrapper for GhostScript (_incomplete & broken_).
- **Narvalo.Reliability** features reliability patterns (_incomplete & broken_).

## Retired projects

- [Narvalo.Web.Extras](https://www.nuget.org/packages/Narvalo.Web.Extras/),
  replaced by [Narvalo.Web](https://www.nuget.org/packages/Narvalo.Web/).
