Narvalo.NET
===========

Right now, a place for me to learn and to test various ideas.

1. General Purpose Libraries
    - **Narvalo.Core**
      (_alpha_,
      [sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Core),
      [package](https://www.nuget.org/packages/Narvalo.Core/)),
      this library features implementations of some of the usual suspects from functional
      programming: Option (`Maybe<T>`) and Error (`Output<T>`) monads, simple pattern matching
      (`Either<T1, T2>`, `Switch<T1, T2>`), generators and delegate extensions. It also provides
      a few Code Contracts helpers. This is a portable class library targeting _Profile259_.
      The behaviour is 100% identical across all supported platforms; this is achieved without
      making use of the [bait and switch PCL trick](http://log.paulbetts.org/the-bait-and-switch-pcl-trick/).
      This library fully passes the FxCop, Gendarme and Code Contracts static analysis.
    - **Narvalo.Common**
      (_alpha_,
      [sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Common),
      [package](https://www.nuget.org/packages/Narvalo.Common/)),
      this library provides various utilities and extension methods: Currency (ISO 4217) and Range
      types, directory walker, Int64 encoders,  benchmark helpers, extensions for Collections,
      Configuration, SQL client and XDom.
    - **Narvalo.Web**
      (_experimental_,
      [sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Web),
      [package](https://www.nuget.org/packages/Narvalo.Common/)),
      this library provides types that might prove useful for Web development: generic HttpHandler
      type, asset providers, Razor and WebForms compile-time optimizers, preliminary support
      for OpenGraph and Schema.Org.
    - **Narvalo.Externs**
      ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Externs)),
      this project contains sample codes for Autofac, Serilog, Castle Windsor...
    - **Narvalo.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Facts))
      is the test project (still in early development).

2. MVP Framework
    - **Narvalo.Mvp**
      (_beta_,
      [sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp),
      [package](https://www.nuget.org/packages/Narvalo.Mvp/)),
      a simple MVP framework largely inspired by [WebFormsMvp](https://github.com/webformsmvp/webformsmvp).
      Contrary to WebFormsMvp, it is not restricted to the WebForms platform; nevertheless, featurewise,
      it should be on par with WebFormsMvp.
    - **Narvalo.Mvp.Web**
      (_beta_,
      [sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp.Web),
      [package](https://www.nuget.org/packages/Narvalo.Mvp.Web/)),
      enhances Narvalo.Mvp to provide support for ASP.NET WebForms similar to WebFormsMvp.
    - [WebForms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWebForms)
    - **Narvalo.Mvp.Facts** ([sources](https://github.com/chtoucas/Narvalo.NET/tree/master/tests/Narvalo.Mvp.Facts))
      is the test project (still in early development).

3. Developer Tools
    - **Narvalo.Build**
      (_experimental_,
      [sources](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build),
      [package](https://www.nuget.org/packages/Narvalo.Build/)),
      custom MSBuild tasks.

There are other projects
([Narvalo.GhostScript](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.GhostScript),
[Narvalo.PowerShell](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.PowerShell),
[Narvalo.StyleCop.CSharp](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.StyleCop.CSharp),
Prose...),
but they are either unfinished, not even started, or severely broken!

Technology footprint
--------------------

- Most developments are done in C#.
- Static and quality analysis are done with StyleCop, FxCop, Gendarme, Code Contracts
  and a tailor-made script.
- All tasks are fully automated with MSBuild, PowerShell (PSake) and F# scripts.

Extra care has been taken to completely isolate the CI environment and to keep the development
inside Visual Studio as smooth and swift as they can be.

### Requirements ###

- Visual Studio Express 2013 Community Edition
- Code Contracts Tools extension
- PowerShell v3
