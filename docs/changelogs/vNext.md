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
  * currency info with implementation from the BCL.
  * money & currency formatting.
- Narvalo.Mvp and Narvalo.Mvp.Web.
  * add localized messages in french for Narvalo.Mvp and Narvalo.Mvp.Web.
  * Application Controller and Navigator.

Other tasks:
- Automatic code formatting with CodeFormatter.
- Put the private key in the repository.
  See [here](https://msdn.microsoft.com/en-us/library/wd40t7ad(v=vs.110).aspx)
- Remove .nuget directory.

Library                   | vNext
--------------------------|----------
Narvalo.Build             | -
Narvalo.Cerbere           | 2.1.0
Narvalo.Common            | 0.26.0
Narvalo.Core              | 0.26.0
Narvalo.Finance           | 0.26.0
Narvalo.Fx                | 0.26.0
Narvalo.Mvp               | 1.1.0
Narvalo.Mvp.Web           | 1.1.0
Narvalo.Web               | 0.26.0

Highlights
----------

Bugfixes
--------

Breaking Changes
----------------
### Narvalo.Cerbere
- `Enforce.IsWhiteSpace()` no longer throws when the input is `null`, but rather
  returns `false`. More importantly, the method returns `false` instead of
  `true` for an empty string.
- New class constraint added to `Require.NotNull<T>()` (idem with `Demand`
  and `Expect`). See below for unconstrained alternatives.

### Narvalo.Web.Configuration
- `AssetSection.DefaultProvider` setter now throws an `ArgumentException`
  if the input consists of only white spaces.

API Changes
-----------
### Narvalo.Cerbere
- Methods marked as obsolete and their replacements:
  * `Require.NotEmpty()` -> `Require.NotNullOrEmpty()`
  * `Require.Object<T>()` -> `Require.NotNullUnconstrained()`
  * `Require.Property<T>()` -> `Require.NotNullUnconstrained()`
  * `Require.Property(bool)` -> `Require.True()`
  * `Require.PropertyNotEmpty(bool)` -> `Require.NotNullOrEmpty()`
  * `Demand.NotEmpty()` -> `Demand.NotNullOrEmpty()`
  * `Expect.NotEmpty()` -> `Expect.NotNullOrEmpty()`
  * `Enforce.NotNullOrWhiteSpace()` -> `Require.NotNullOrWhiteSpace()`
  * `Enforce.PropertyNotWhiteSpace()` -> `Require.NotNullOrWhiteSpace()`
- The new methods `Require.NotNullOrWhiteSpace()` has the `ContractArgumentValidator`
  attribute and specifies a stronger code contract: "the input must not be
  `null` or empty" (not empty is what has been added).
- `Require.NotNullUnconstrained<T>()` complements `Require.NotNull<T>()`
  by not requiring any constraint on the generic parameter.
- As for `Require`, we add to `Demand` and `Expect` the methods:
  * `NotNullOrWhiteSpace()`
  * `NotNullUnconstrained()`

### Narvalo.Core
- `BooleanStyles.EmptyIsFalse` is declared obsolete;
  use `BooleanStyles.EmptyOrWhiteSpaceIsFalse` instead. This is to better
  emphasize that whitespace-only strings are treated as empty strings by
  `ParseTo.Boolean()`.

Enhancements
------------
- Full test coverage for Narvalo.Cerbere.
