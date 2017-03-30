Narvalo.Fx
==========

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Fx.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Fx)

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), return types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`), simple disjoint union (`Either<T1, T2>`),
sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be marked stable.

### Content
- [Overview](#overview)
- [Maybe monad](#maybe-monad)
- [Error monads](#error-monads)
- [LINQ extensions](#linq-extensions)
- [Infinite sequences](#infinite-sequences)
- [Changelog](#changelog)

Overview
--------

This assembly encourages an applicative-style of programming (or functional if
you prefer):
- Types are **immutable**.
- Methods are **pure** - they are free of side-effects.

The main namespace is `Narvalo.Applicative`:
- `Unit`
- `Maybe<T>`, the Maybe monad.
- `Outcome`, an Error monad with lightweight error reporting for methods without
  a return type.
- `Outcome<T>`, an Error monad with lightweight error reporting.
- `Fallible`, an Error monad with exception capture for methods without
  a return type.
- `Fallible<T>`, an Error monad with exception capture.
- `Result<T, TError>`, the Error monad.
- `Either<T1, T2>`, the Either monad.
- `Sequence` which provides various ways to generate infinite sequences.
- Stubs for some commonly used delegates.

Our implementation of monads follows very closely the Haskell API. We also
enable the Query Expression Pattern on all monadic types.

In the context of C#, I am yet to be convinced of the usefulness and practicability
of the other common monads - for skeleton definitions of `IO`, `Reader` and `State`,
see [here](https://github.com/chtoucas/Brouillons/tree/master/src/play/Functional/Monadic).

The other namespace is `Narvalo.Linq` which contains LINQ extensions:
- [Projecting] `SelectAny` (deferred)
- [Filtering] `WhereAny` (deferred)
- [Set] `Append` (deferred), `Prepend` (deferred)
- [Element] `FirstOrNone`, `LastOrNone`, `SingleOrNone`, `ElementAtOrNone`
- [Aggregation] `Aggregate` (deferred)
- [Quantifiers] `IsEmpty`
- [Generation] `EmptyIfNull`
- We have also operators accepting arguments in the Kleisli "category":
  * `SelectWith` (deferred)
  * `ZipWith` (deferred)
  * `WhereBy` (deferred)
  * `Fold`
  * `Reduce`.

Maybe monad
-----------

The `Maybe<T>` class is like `Nullable<T>` class but without restriction
on the underlying type: *it provides a way to tell the absence or the presence
of a value*. For value types, most of the time `T?` offers a much better
alternative. This class is sometimes referred to as the Option type.

### Haskell ###
- `catMaybes`   -> `Maybe.CollectAny()`
- `isJust`      -> `Maybe<T>.IsSome`
- `isNothing`   -> `Maybe<T>.IsNone`
- `fromMaybe`
- `fromJust`
- `maybeToList` -> `Maybe<T>.ToEnumerable()`
- `maybe`       -> `Maybe<T>.Match()`
- `listToMaybe` -> `Qperators.FirstOrNone()`
- `mapMaybe`    -> `Qperators.SelectAny()`

Error monads
------------

Typical use cases:
- To encapsulate the result of a computation with lightweight error reporting
  to the caller in the form of a string: `Outcome` and `Outcome<T>`.
- To encapsulate the result of a computation with exception capture
  (`ExceptionDispatchInfo`): `Fallible` and `Fallible<T>`.
- In all other cases: `Result<T, TError>`.

Of course, we could have gone away with one single type, but at the expense
of complicated signatures. The correspondence is as follows:

Type             | Alternatives
-----------------|-------------
`Outcome`        | `Result<Unit, string>` or `Outcome<Unit>`
`Outcome<T>`     | `Result<T, string>`
`Fallible`       | `Result<Unit, ExceptionDispatchInfo>` or `Fallible<Unit>`
`Fallible<T>`    | `Result<T, ExceptionDispatchInfo>`

Remarks:
- All these types are value types, their primary usage is as a return type.
  For long-lived objects prefer `Either<T, TError>`.
- `Result<T, Exception>` should be used only in very rare situations; this is
  **not** a replacement for the standard exception mechanism in .NET.
  In any cases, `Fallible` and `Fallible<T>` offer better alternatives.
- You can see `Outcome` and `Outcome<T>` as verbose versions of `Maybe<Unit>`
  and `Maybe<T>`.
- **Always** prefer `Outcome` over `Maybe<TError>`.
  With `Maybe<TError>` it is not obvious that the underlying type (`TError`)
  represents an error and not the "normal" return type.

LINQ Extensions
---------------

_TODO_

Infinite Sequences
------------------

_TODO_

Changelog
---------
