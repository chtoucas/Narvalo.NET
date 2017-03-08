ChangeLog (XXXX-XX-XX)
======================

Library                   | Previous | vNext
--------------------------|-------------------
Narvalo.Build             | 1.1.0    | -
Narvalo.Cerbere           | 2.0.0    | 2.1.0
Narvalo.Common            | 0.25.0   | 0.26.0
Narvalo.Core              | 0.25.0   | 1.0.0
Narvalo.Finance           | 0.25.0   | 1.0.0
Narvalo.Fx                | 0.25.0   | 0.26.0
Narvalo.Money             | -        | 1.0.0
Narvalo.Mvp               | 1.0.0    | 1.1.0
Narvalo.Mvp.Web           | 1.0.0    | 1.1.0
Narvalo.Web               | 0.25.0   | 0.26.0

Highlights
----------
- Good test coverage for Narvalo.Cerbere, Narvalo.Core & Narvalo.Finance.
- [Narvalo.Finance] IBAN & BIC types with validation helpers.
- [Narvalo.Money] Money & Currency types; formatting, rounding and allocators.
- [Narvalo.Fx] Much better implementations and APIs for representing "return" types.

Breaking Changes
----------------
### Narvalo.Cerbere
- `Enforce.IsWhiteSpace()` has been replaced by `Check.IsWhiteSpace()` and
  no longer throws when the input is `null`, but rather
  returns `false`. More importantly, the method returns `false` instead of
  `true` for an empty string.
- Class constraint added to `Require.NotNull<T>()` (idem with `Demand`
  and `Expect`). See below for unconstrained alternatives.

API Changes
-----------
### Narvalo.Cerbere
Methods marked as obsolete and their replacements:

Obsolete API                      | New API
-------------------------------------------------------------------------------------------
`Require.Object<T>()`             | `Require.NotNullUnconstrained()`
`Require.Property<T>()`           | `Require.NotNullUnconstrained()`
`Require.Property(bool)`          | `Require.True()`
`Require.PropertyNotEmpty(bool)`  | `Require.NotNullOrEmpty()`
`Demand.Object<T>()`              | `Demand.NotNullUnconstrained()`
`Demand.Property<T>()`            | `Demand.NotNullUnconstrained()`
`Demand.Property(bool)`           | `Demand.True()`
`Demand.PropertyNotEmpty(bool)`   | `Demand.NotNullOrEmpty()`
`Expect.Object<T>()`              | `Expect.NotNullUnconstrained()`
`Expect.Property<T>()`            | `Expect.NotNullUnconstrained()`
`Expect.Property(bool)`           | `Expect.True()`
`Expect.PropertyNotEmpty(bool)`   | `Expect.NotNullOrEmpty()`
`Enforce.NotNullOrWhiteSpace()`   | `Require.NotNullOrEmpty()` or `Enforce.NotWhiteSpace()`
`Enforce.PropertyNotWhiteSpace()` | `Require.NotNullOrEmpty()` or `Enforce.NotWhiteSpace()`
`Enforce.IsWhiteSpace()`          | `Check.IsWhiteSpace()`

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
- Narvalo.Finance is now a Profile259 project.
- Turned off overflow checking for Release builds.
- Move from .NET 4.5 to .NET 4.6.1
- Remove Code Contracts in all libraries except the MVP ones.
