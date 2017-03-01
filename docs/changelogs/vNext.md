ChangeLog (XXXX-XX-XX)
======================

Objectives
----------
The plan is to improve the overall quality & usability of Narvalo.Cerbere,
Narvalo.Fx and Narvalo.Finance.

Documentation & Tests (Narvalo.Cerbere & Narvalo.Finance):
- Fully documented API.
- Usage guidelines.
- Good test coverage.

Library Enhancements & Improvements:
- Narvalo.Finance
  * BBAN & IIBAN implementations.
  * validation for BIC, IBAN, IIBAN & BBAN.
  * currency info with implementation from the BCL & CLDR.
  * money rounding.
  * money & currency formatting.
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

TODO
----
- Remove Narvalo.Core and move from .NET 4.5 to .NET 4.6.1
- Remove Code Contracts -> Warrant & Expect.
Refactoring?
- Narvalo.Properties in AssemblyInfo -> Properties or Narvalo
- Narvalo.Cerbere -> Cerbere
- Narvalo.Fx -> Efix
- Narvalo.Finance -> ?

Narvalo.Finance:
- Sync `Money<TCurrency>`, CurrencyUnit<>`, `MoneyFormatter` for rounding, scale...
- Complete localization of messages.
- Ops with double, float?
- Improve Allocate() and custom allocations.
- Non-decimal moneys: Mauritania & Madagascar;
  see [here](https://en.wikipedia.org/wiki/Denomination_(currency))
- Money.Parse & Money.TryParse.
- `IConvertible` (no) and conversion between currencies.
- Rounding:
  [rounding](https://en.wikipedia.org/wiki/Rounding),
  [cash rounding](https://en.wikipedia.org/wiki/Cash_rounding)
  [sum](https://en.wikipedia.org/wiki/Kahan_summation_algorithm).
- Protect Multiply, Divide and Remainder against absurd results when rounding.
- DecimalRounding.Scale(), check for minimal value?
- Add support for minor units (EUR -> EUr).
  [Wikipedia](https://en.wikipedia.org/wiki/List_of_circulating_currencies),
  [here](http://stackoverflow.com/questions/5023754/do-minor-currency-units-have-a-iso-standard),
  and [here](http://www.fixtradingcommunity.org/pg/discussions/topicpost/167427/gbpgbpgbx).
- Check div and rem.
- Create a currency builder for user-defined currencies.
- Check boxing.
  See [here](http://stackoverflow.com/questions/13558579/are-there-other-ways-of-calling-an-interface-method-of-a-struct-without-boxing-e),
  [here](http://stackoverflow.com/questions/5757324/is-there-boxing-unboxing-when-casting-a-struct-into-a-generic-interface)
  and [here](http://stackoverflow.com/questions/3032750/structs-interfaces-and-boxing).
- Pretty sure that we can get rid off all uint and int overloads.

Narvalo.Fx:
- null-check's in generated methods.
- Async versions?
- https://blogs.msdn.microsoft.com/pfxteam/2013/04/03/tasks-monads-and-linq/
- http://tomasp.net/blog/idioms-in-linq.aspx/
- https://ruudvanasseldonk.com/2013/05/01/the-task-monad-in-csharp
- https://github.com/tomstuart/monads

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

Enhancements
------------
- Full test coverage for Narvalo.Cerbere.
- Narvalo.Facts is now free of CA warnings/errors.
- Narvalo.Finance is now a Profile259 project.
- Turned off overflow checking for Release builds.
