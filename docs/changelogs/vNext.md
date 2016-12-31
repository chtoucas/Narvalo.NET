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
  * BBAN & IIBAN implementations.
  * strong/weak validation of Bic, Iban & Bban (check digits).
  * currency info with implementation from the BCL & CLDR.
  * handle rounding and native scale.
  * money & currency formatting + rounding before formatting.
  * exchange rate.
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
Narvalo.Finance           | 0.26.0 (1.0.0?)
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
- `Enforce.IsWhiteSpace()` has been replaced by `Check.IsWhiteSpace()` and
  no longer throws when the input is `null`, but rather
  returns `false`. More importantly, the method returns `false` instead of
  `true` for an empty string.
- New class constraint added to `Require.NotNull<T>()` (idem with `Demand`
  and `Expect`). See below for unconstrained alternatives.

### Narvalo.Common
- Renamed `PathUtility` to `PathHelpers`.

### Narvalo.Core
- All classes in Narvalo.Collections moved to the package Narvalo.Common.
- `BooleanStyles.EmptyIsFalse` has been replaced by
  `BooleanStyles.EmptyOrWhiteSpaceIsFalse`. This is to better emphasize that
  white-space only strings are treated as empty strings by `ParseTo.Boolean()`.
- `Range<T>.Includes(T)` has been replaced by `Range<T>.Contains(T)`

### Narvalo.Finance
- `Bic.CheckSwiftFormat()` and `Bic.CheckFormat()` have been removed;
  it is no longer possible to create an invalid Bic.
- `Iban.CheckDigit` becomes `Iban.CheckDigits`.
- `Currency.Of()` now returns a different object at each call.
- Removed `CurrencyFactory`, `CurrencyProvider` and `DefaultCurrencyFactory`.
- For built-in currencies, renamed the singleton method, e.g 'EUR.Currency'
  becomes `EUR.UniqInstance`.
- `Bic.IsConnected` becomes `Bic.IsSwiftConnected`.

### Narvalo.Web
- `AssetSection.DefaultProvider` setter now throws an `ArgumentException`
  if the input consists of only white spaces.

API Changes
-----------
### Narvalo.Cerbere
Methods marked as obsolete and their replacements:
- `Require.Object<T>()` -> `Require.NotNullUnconstrained()`
- `Require.Property<T>()` -> `Require.NotNullUnconstrained()`
- `Require.Property(bool)` -> `Require.True()`
- `Require.PropertyNotEmpty(bool)` -> `Require.NotNullOrEmpty()`
- `Demand.Object<T>()` -> `Demand.NotNullUnconstrained()`
- `Demand.Property<T>()` -> `Demand.NotNullUnconstrained()`
- `Demand.Property(bool)` -> `Demand.True()`
- `Demand.PropertyNotEmpty(bool)` -> `Demand.NotNullOrEmpty()`
- `Expect.Object<T>()` -> `Expect.NotNullUnconstrained()`
- `Expect.Property<T>()` -> `Expect.NotNullUnconstrained()`
- `Expect.Property(bool)` -> `Expect.True()`
- `Expect.PropertyNotEmpty(bool)` -> `Expect.NotNullOrEmpty()`
- `Enforce.NotNullOrWhiteSpace()` -> use `Require.NotNullOrEmpty()` and
  `Enforce.NotWhiteSpace()`
- `Enforce.PropertyNotWhiteSpace()` -> use `Require.NotNullOrEmpty()` and
  `Enforce.NotWhiteSpace()`
- `Enforce.IsWhiteSpace()` -> `Check.IsWhiteSpace()`

New classes and new methods:
- `Require.NotNullUnconstrained<T>()` complements `Require.NotNull<T>()`
  by not requiring any constraint on the generic parameter.
- `Demand.NotNullUnconstrained<T>()` complements `Demand.NotNull<T>()`.
- `Expect.NotNullUnconstrained<T>()` complements `Expect.NotNull<T>()`.
- `Enforce.State()`
- `Enforce.True()`
- `Enforce.Range()`
- `Enforce.Range<T>()`
- `Check.IsEmptyOrWhiteSpace()`
- `Check.IsFlagsEnum()`
- `Warrant` a new helper class for postconditions.

### Narvalo.Common
New classes and new methods:
- `DictionaryExtensions` previously available in Narvalo.Core.
- `EnumeratorExtensions` previously available in Narvalo.Core.
- `Appender<TSource, T>`
- `Setter<TSource, T>`

### Narvalo.Core
New classes and new methods:
- `Range.IsValid<T>()`
- `Range<T>.IsDegenerate`

### Narvalo.Finance
New classes and new methods:
- `Currency.OfCurrentRegion()`
- `Currency.OfCulture()`
- `Currency.OfCurrentCulture()`
- `Currency.RegisterCurrency()`
- `Bic.IsSwiftTest`
- `Iban` implemens `IFormattable`.
- `BicFormat`
- `BicStyles`
- `IbanFormat`
- `IbanStyle`
- `AsciiHelpers`

- `Iban.Parse()` and `Iban.TryParse()` allows the presence of display chars.

Enhancements
------------
- Full test coverage for Narvalo.Cerbere.
- Narvalo.Facts is now free of CA warnings/errors.
- Narvalo.Finance is now a Profile259 project.
- Turned off overflow checking for Release builds.
