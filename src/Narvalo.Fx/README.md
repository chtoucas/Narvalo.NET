Narvalo.Fx
==========

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Fx.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Fx)

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), error types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`) for Railway Oriented Programming, simple
disjoint union (`Either<T1, T2>`), sequence generators and LINQ extensions.

### Status
- **Unstable.** Versioning scheme explained
  [here](https://github.com/chtoucas/Narvalo.NET/blob/master/docs/content/developer.md#versioning).
- Target the **.NET Standard 1.0** and the **Profile259** PCL profile.
- Localized messages available in both **French** and **English**.
- **[User guide](https://github.com/chtoucas/narvalo.org/blob/master/content/doc/narvalo-fx.md)**
- C# documentation is largely missing.
- Test coverage is starting to look good (70%). The number of functional tests
  is progressing too.

Tentative release date for a stable package: end of april 2017?
_Pending:_ API stability, good documentation and thorough testing.
_Open question:_ should the error types be put in a separate assembly?

[What's next?](https://github.com/chtoucas/Narvalo.NET/blob/master/issues.md)
