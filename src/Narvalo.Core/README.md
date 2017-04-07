Narvalo.Core
============

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Core.svg)](https://www.nuget.org/packages/Narvalo.Core/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Core.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Core)

Provides helpers on which depend the other Narvalo packages.

### Status
- **Stable**
- Full test coverage.
- Complete C# documentation.

Our versioning scheme is explained
[here](https://github.com/chtoucas/Narvalo.NET/blob/master/docs/index.md#versioning).

Changelog
---------

**vNext**
- **[Breaking]** Remove all variants of `Format.Invariant`;
  use `FormattableString.Invariant` instead.
- **[Breaking]** Remove variant of `Format.Current` w/ variable arguments;
  we do not have a real use case for it.
- **[Breaking]** Change the name of the second parameter (`arg0` to `arg` )
  in `Format.Current(string, T)`.
- Achieve good documentation and full test coverage.

**Version 1.2.0** (released 2017-03-17)
- **[Breaking]** Remove variants of `Format.Current` and `Format.Invariant`
  w/o arguments; adding these methods was a mistake.
- Provide localized messages in both French and English.