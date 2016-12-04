ChangeLog (XXXX-XX-XX)
======================

Objectives
----------
The plan is to improve the overall quality & usability of Narvalo.Cerbere
and Narvalo.Finance.

Documentation & Tests (Narvalo.Cerbere & Narvalo.Finance):
- Fully documented API.
- Usage guidelines.
- Good test coverage.

Library Enhancements & Improvements:
- Narvalo.Finance (still no big money for accurate computations, next time...)
  * localization.
  * BBAN implementation.
  * strong/weak validation of Bic, Iban & Bban (check digits).
  * provide an alternate live Currency provider (see SnvCurrencyXmlReader).
  * currency info with implementation from BCL.
  * money & currency formatting.
- Narvalo.Mvp and Narvalo.Mvp.Web.
  * add localized messages in french for Narvalo.Mvp and Narvalo.Mvp.Web.
  * Application Controller and Navigator.

Other tasks:
- Automatic code formatting with CodeFormatter.
- Put the private key in the repository.
  See [here](https://msdn.microsoft.com/en-us/library/wd40t7ad(v=vs.110).aspx)

Highlights
----------

Bugfixes
--------

Breaking Changes
----------------
### Narvalo.Core
- `Enforce.IsWhiteSpace()` now throws an `ArgumentOutOfRangeException`
  if the input is an empty string.

### Narvalo.Web.Configuration
- `AssetSection.DefaultProvider` setter now throws an `ArgumentException`
  if the input contains only whitespaces.

API Changes
-----------
### Narvalo.Core
- `BooleanStyles.EmptyIsFalse` is declared obsolete;
  use `BooleanStyles.EmptyOrWhiteSpaceIsFalse` instead. This is to better
  emphasize that whitespace-only strings are treated as empty strings by
  `ParseTo.Boolean()`.

Enhancements
------------
- Full test coverage for Narvalo.Cerbere.
