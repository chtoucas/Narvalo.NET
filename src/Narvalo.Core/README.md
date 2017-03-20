Narvalo.Core
============

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Core.svg)](https://www.nuget.org/packages/Narvalo.Core/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Core.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.EDGE)

Provides helpers on which depend the other Narvalo packages.

Changelog
---------

**vNext**
- **[Breaking]** Remove all variants of `Format.Invariant`; 
  use `FormattableString.Invariant` instead.
- **[Breaking]** Remove variant of `Format.Current` w/ variable arguments;
  we do not have a real use case for it.
- **[Breaking]** Change from `arg0` to `arg` the name of the second parameter 
  in `Format.Current(string, T)`.

**Version 1.2.0** (released 2017-03-17)
- **[Breaking]** Remove variants of `Format.Current` and `Format.Invariant`
  w/o arguments; adding these methods was a mistake.
- Localized messages for French and English.