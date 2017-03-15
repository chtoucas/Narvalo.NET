Narvalo.NET
===========

- [Documentation](https://github.com/chtoucas/Narvalo.NET/tree/master/docs)
- [License](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE.txt)
- [License for WebFormsMvp](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE-WebFormsMvp.txt)
  on which depend all MVP-related packages.

Unstable packages are available on [MyGet](https://www.myget.org/).

## General Purpose Libraries
- [Narvalo.Core](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Core/README.md)
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Core.svg)](https://www.nuget.org/packages/Narvalo.Core/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Core.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Core.EDGE)
- **Narvalo.Fx**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Fx.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Fx.EDGE),
  option type (`Maybe<T>`), return types (`Result<T, TError>`, `Outcome<T>` and
  `Fallible<T>`), simple disjoint union (`Either<T1, T2>`), sequence generators
  and LINQ extensions.
- **Narvalo.Money**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Money.svg)](https://www.nuget.org/packages/Narvalo.Money/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Money.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Money.EDGE),
  Currency (ISO 4217) and Money types.
- **Narvalo.Finance**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Finance.svg)](https://www.nuget.org/packages/Narvalo.Finance/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Finance.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Finance.EDGE),
  BIC (ISO 9362) and IBAN types.
- **Narvalo.Common**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Common.svg)](https://www.nuget.org/packages/Narvalo.Common/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Common.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Common.EDGE),
  simple parsers, Int64 encoders, extension methods for Collections, XDom and SQL client.
- **Narvalo.Web**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Web.svg)](https://www.nuget.org/packages/Narvalo.Web/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Web.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Web.EDGE),
  generic HttpHandler type, asset providers, Razor and WebForms compile-time optimizers.

## MVP Framework
- **Narvalo.Mvp**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Mvp.svg)](https://www.nuget.org/packages/Narvalo.Mvp/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Mvp.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Mvp.EDGE),
  a simple MVP framework largely inspired by [WebFormsMvp](https://github.com/webformsmvp/webformsmvp).
  Contrary to WebFormsMvp, it is not restricted to the WebForms platform; nevertheless, featurewise,
  it should be on par with WebFormsMvp. Includes support for command-line applications.
- **Narvalo.Mvp.Web**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Mvp.Web.svg)](https://www.nuget.org/packages/Narvalo.Mvp.Web/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Mvp.Web.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Mvp.Web.EDGE),
  enhances Narvalo.Mvp to provide support for ASP.NET WebForms similar to WebFormsMvp.
- Samples:
  * [Command-Line MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpCommandLine)
  * [WebForms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWebForms)

## Developer Tools
- **Narvalo.Build**
  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Build.svg)](https://www.nuget.org/packages/Narvalo.Build/)
  [![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Build.EDGE.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Build.EDGE),
  custom MSBuild tasks.
