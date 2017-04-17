Narvalo.Finance
===============

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Finance.svg)](https://www.nuget.org/packages/Narvalo.Finance/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Finance.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Finance)

BIC (ISO 9362) and IBAN types; includes support for parsing, validation and formatting.

**WARNING:** IBAN validation does not yet perform full BBAN validation.

### Status
- **Stable.** Versioning scheme explained
  [here](https://github.com/chtoucas/Narvalo.NET/blob/master/docs/content/developer.md#versioning).
- Target the **.NET Standard 1.0** and the **Profile259** PCL profile.
- Localized messages available in both French and English.
- C# documentation is largely missing.
- Full test coverage.

[What's next?](https://github.com/chtoucas/Narvalo.NET/blob/master/issues.md)

Changelog
---------

**Version 1.1.1** (released 2017-04-07)
- **[Bug Fix]** `Iban.Parse()` and `Iban.TryParse()` when using the option
  `IbanStyles.AllowInnerWhite` but not the others allowed for leading or
  trailing white spaces.
- Achieve full test coverage.

**Version 1.1.0** (released 2017-03-17)
- Provide localized messages in both French and English.
