Narvalo.NET
===========

- [Documentation](https://github.com/chtoucas/Narvalo.NET/tree/master/docs)
- [License](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE.txt)
- [License for WebFormsMvp](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE-WebFormsMvp.txt)
  on which depend all MVP-related packages.

## General Purpose Libraries
- **Narvalo.Cerbere**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Cerbere),
  [package](https://www.nuget.org/packages/Narvalo.Cerbere/)),
  this library provides argument validation methods and Code Contracts helpers.
- **Narvalo.Fx**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Fx),
  [package](https://www.nuget.org/packages/Narvalo.Fx/)),
  this library features implementations of some of the usual suspects from functional
  programming: Option (`Maybe<T>`) and Error (`Output<T>`) monads, simple pattern matching
  (`Either<T1, T2>`, `Switch<T1, T2>`), generators and delegate extensions.
- **Narvalo.Finance**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Finance),
  [package](https://www.nuget.org/packages/Narvalo.Finance/)),
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

## Developer Tools
- **Narvalo.Build**
  ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build),
  [package](https://www.nuget.org/packages/Narvalo.Build/)),
  custom MSBuild tasks.

## Samples
- [Command-Line MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpCommandLine)
- [WebForms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWebForms)
- [Windows Forms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWindowsForms)
