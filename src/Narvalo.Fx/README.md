Narvalo.Fx
==========

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Fx.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Fx)

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), error types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`), simple disjoint union (`Either<T1, T2>`),
sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be marked stable.

### Content
- [Overview](#overview)
- [Maybe type](#maybe-type)
- [Error types](#error-types)
- [LINQ extensions](#linq-extensions)
- [Infinite sequences](#infinite-sequences)
- [Monadic API](#monadic-api)
- [Design](#design)
- [Changelog](#changelog)

Overview
--------

This assembly encourages an applicative-style of programming (or functional if
you prefer):
- Types are **immutable**.
- Methods are **pure** - they are free of side-effects.

The main namespace is `Narvalo.Applicative`:
- `Unit`
- `Maybe<T>`, the Maybe type, a generalization of `Nullable<T>`.
- `Outcome`, an Error type with lightweight error reporting for methods without
  a return type.
- `Outcome<T>`, an Error type with lightweight error reporting.
- `Fallible`, an Error type with exception capture for methods without
  a return type.
- `Fallible<T>`, an Error type with exception capture.
- `Result<T, TError>`, the Error type.
- `Either<T1, T2>`, the Either type.
- `Sequence` which provides various ways to generate infinite sequences.
- Stubs for some commonly used delegates.

Remarks:
- Maybe, Error and Either are examples of monads, a concept popularized by Haskell.
  If you know nothing about monads or Haskell, don't worry, no previous knowledge
  is required.
- The implementation of Maybe, Error and Either follows closely the Haskell API
  but, of course, adapted to make it more C#-friendly (see [below](#design)
  for more details on this). These types also support query expressions
  [[Query expression pattern](https://github.com/dotnet/csharplang/blob/master/spec/expressions.md#the-query-expression-pattern)].
- The astute reader will have notice that some of the common monads are missing;
  they were not included on purpose. In the context of C#, I am yet to be
  convinced of their usefulness and practicability (for skeleton definitions of
  `IO`, `Reader` and `State`,
  see [here](https://github.com/chtoucas/Brouillons/tree/master/src/play/Functional/Monadic)).

The other namespace is `Narvalo.Linq` which contains LINQ extensions:
- [Projecting] `SelectAny` (deferred)
- [Filtering] `WhereAny` (deferred), `CollectAny` (deferred)
- [Set] `Append` (deferred), `Prepend` (deferred)
- [Element] `FirstOrNone`, `LastOrNone`, `SingleOrNone`, `ElementAtOrNone`
- [Aggregation] `Aggregate` (deferred)
- [Quantifiers] `IsEmpty`
- [Generation] `EmptyIfNull`

We also provide generalized LINQ operators accepting as arguments functions
that maps a value to a Maybe, an Error or an Either:
  * `SelectWith` (deferred)
  * `ZipWith` (deferred)
  * `WhereBy` (deferred)
  * `Fold`
  * `Reduce`.

Maybe type
----------

The `Maybe<T>` class is like the `Nullable<T>` class but without restriction
on the underlying type: *it provides a way to tell the absence or the presence
of a value*. For value types, most of the time `T?` offers a much better
alternative. This class is sometimes referred to as the Option type.

Error types
-----------

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

Monadic API
-----------

_TODO_

Design
------

Informally, a monad `Monad<T>` is simply a type with two operations
```csharp
public static class Monad {
    public static Monad<TSource> Return<TSource>(TSource value) { ... }
}

public class Monad<T> {
    Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> binder) { ... }
}
```
that satisfy the _monad laws_. We won't discuss them but, in plain English, they
say that _Return_ is an identity for _Bind_, and that _Bind_ is associative.

If one wishes to stay closer to the definition of monads from category theory,
a monad is rather defined by a unit element `Return` and two operations
`Select` and `Flatten` where `Select` must satisfy the _functor laws_.

### Maybe

Haskell | C#
--------|---
`catMaybes`   | `Maybe.CollectAny()`
`isJust`      | `Maybe<T>.IsSome`
`isNothing`   | `Maybe<T>.IsNone`
`fromMaybe`   |
`fromJust`    |
`maybeToList` | `Maybe<T>.ToEnumerable()`
`maybe`       | `Maybe<T>.Match()`
`listToMaybe` | `Qperators.FirstOrNone()`
`mapMaybe`    | `Qperators.SelectAny()`

### References
- The first public discussion of monads in the context of .NET seems to be due to
  [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx).
- A popular explanation of monads by [Eric Lippert](http://ericlippert.com/category/monads/).
- A more abstract one by [Erik Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/).


Changelog
---------
