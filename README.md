Narvalo.NET
===========

Right now, a place for me to learn and to test various ideas.

**Status: Experimental**

The most stable parts are:

1. Narvalo Core Libraries
    - [Narvalo.Core](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Core),
      This library features implementations of some of the usual suspects from
      functional programming: Option (`Maybe<T>`) and Error (`Output<T>`) monads,
      simple pattern matching (`Either<T1, T2>`, `Switch<T1, T2>`), generators
      and function extensions.
      It also contains helpers to perform argument validation compatible with
      Code Contracts preconditions.
    - [Narvalo.Common](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Common),
      This library provides various utilities and extension methods: `Currency`
      (ISO 4217) and Range types, directory walker, Int64 encoder,
      benchmark helpers, extensions for Collections, Configuration, SQL client
      and XDom.
    - [Narvalo.Web](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Web),
      This library provides types that might prove useful for Web development:
      generic HttpHandler type, asset provider, Razor and WebForms compile-time
      optimizers, preliminary support for OpenGraph and Schema.Org.

2. MVP Framework
    - [Narvalo.Mvp](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp),
      a simple MVP framework largely inspired by [WebFormsMvp](https://github.com/webformsmvp/webformsmvp).
    - [Narvalo.Mvp.Web](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp.Web),
      enhances Narvalo.Mvp to provide support for ASP.NET WebForms.

3. Developer Tools
    - [Narvalo.Build](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build),
      custom MSBuild tasks.

All other projects are either unfinished or severely broken!
