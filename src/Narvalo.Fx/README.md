Narvalo.Fx
==========

[![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)
[![MyGet](https://img.shields.io/myget/narvalo-edge/v/Narvalo.Fx.svg)](https://www.myget.org/feed/narvalo-edge/package/nuget/Narvalo.Fx)

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), error types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`), simple disjoint union (`Either<T1, T2>`),
sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be declared stable.
- Test coverage is starting to look good (75%).
- C# documentation is largely missing.

### Content
- [Overview](#overview)
- [Maybe type](#maybe-type)
- [Error types](#error-types)
- [Either type](#either-type)
- [LINQ extensions](#linq-extensions)
- [Infinite sequences](#infinite-sequences)
- [Derived API](#derived-api)
- [Monad Tutorial](#monad-tutorial)
- [Changelog](#changelog)

**WARNING:** _I am currently in the process of rewriting this document._

Overview
--------

This assembly encourages an applicative-style of programming, or functional-style
if you prefer: types are **immutable** and methods are **pure** - they are free
of side-effects.

### Namespace `Narvalo.Applicative`
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

We also provide generalized LINQ operators accepting as arguments functions
that maps a value to a Maybe, an Error or an Either:
  * `SelectWith` (deferred)
  * `ZipWith` (deferred)
  * `WhereBy` (deferred)
  * `Fold`
  * `Reduce`

### Namespace `Narvalo.Linq`
The other namespace is `Narvalo.Linq` which contains LINQ extensions.

- [Projection Operators] `SelectAny` (deferred)
- [Restriction Operators] `WhereAny` (deferred), `CollectAny` (deferred)
- [Set Operators] `Append` (deferred), `Prepend` (deferred)
- [Element Operators] `FirstOrNone`, `LastOrNone`, `SingleOrNone`, `ElementAtOrNone`
- [Aggregation Operators] `Aggregate` (deferred)
- [Quantification Operators] `IsEmpty`
- [Generation Operators] `EmptyIfNull`

### Remarks
Maybe, Error and Either are examples of _monads_, a concept popularized by Haskell.
If you know nothing about monads or Haskell, don't worry, no previous knowledge
is required.

The implementation of Maybe, Error and Either follows closely the Haskell API
but, of course, adapted to make it more C#-friendly (see [below](#derived-api)
for more details on this). These types also support the query expression syntax
[[Query expression pattern](https://github.com/dotnet/csharplang/blob/master/spec/expressions.md#the-query-expression-pattern)].

The astute reader will have notice that some of the common monads are missing;
they were not included on purpose. In the context of C#, I am yet to be
convinced of their usefulness and practicability (for skeleton definitions of
`IO`, `Reader` and `State`,
see [here](https://github.com/chtoucas/Brouillons/tree/master/src/play/Functional/Monadic)).

Maybe type
----------

The `Maybe<T>` class is a lot like the `Nullable<T>` class but without any
restriction on the underlying type: _it provides a way to tell the absence or
the presence of a value_. For value types, most of the time `T?` offers a much
better alternative. This class is sometimes referred to as the Option type.

Error types
-----------

Typical use cases:
- To encapsulate the result of a computation with lightweight error reporting
  to the caller in the form of a string: `Outcome` and `Outcome<T>`.
- To encapsulate the result of a computation with full exception capture
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

Either type
-----------

LINQ Extensions
---------------

Infinite Sequences
------------------

Derived API
-----------

We will use the Maybe type as an example. Below we use:
- `obj` for an object of type `Maybe<T>`.
- `kunc` for a function of type `Func<T, Maybe<TResult>>`.
- `seq` for an object of type `IEnumerable<T>`.
- `mseq` for an object of type `IEnumerable<Maybe<T>>`

All variants that return a `Maybe<Unit>` instead of a `Maybe<T>` (those that have
a suffix `_`) are not implemented.

Haskell | C# | Return Type
--------|----|------------
`>>=`          | `obj.Bind`         | `Maybe<TResult>`
`>>`           | `obj.ContinueWith` | `Maybe<TResult>`
`return`       | `Maybe.Of`         | `Maybe<T>`
`fail`         | -                  | -
`fmap`         | `obj.Select`       | `Maybe<TResult>`

We do not implement `fail` as .NET has its own way of reporting errors.

#### Basic monad functions

Haskell | C# | Return Type
--------|----|------------
`mapM` / `mapM_`         | `seq.SelectWith`   | `Maybe<IEnumerable<TResult>>`
`forM` / `forM_`         | `kunc.InvokeWith`  | `Maybe<IEnumerable<TResult>>`
`sequence` / `sequence_` | `mseq.Collect`     | `Maybe<IEnumerable<T>>`
`(=<<)`                  | `kunc.InvokeWith`  | `Maybe<TResult>`
`(>=>)`                  | `kunc.Compose`     | `Func<T, Maybe<TResult>>`
`(<=<)`                  | `kunc.ComposeBack` | `Func<T, Maybe<TResult>>`
`forever`                | -                  | -
`void`                   | `obj.Skip`         | `Maybe<Unit>`

#### Generalisations of list functions

Below `square` is an object of type `Maybe<Maybe<T>>`.

Haskell | C# | Return Type
--------|----|------------
`join`                       | `square.Flatten`    | `Maybe<T>`
`filterM`                    | `seq.WhereBy`       | `Maybe<IEnumerable<T>>`
`mapAndUnzipM`               | -                   | -
`zipWithM` / `zipWithM_`     | `seq.ZipWith`       | `Maybe<IEnumerable<TResult>>`
`foldM` / `foldM_`           | `seq.Fold`          | `Maybe<TAccumulate>`
`replicateM` / `replicateM_` | `Maybe.Repeat`      | `Maybe<IEnumerable<T>>`

`mapAndUnzipM` is easily implemented using `Select` and `SelectWith`:
```csharp
public Maybe<(IEnumerable<T1>, IEnumerable<T2>)> SelectUnzip<T, T1, T2>(
    this IEnumerable<T> source,
    Func<TSource, Maybe<(T1, T2)>> selector) {

    Maybe<IEnumerable<(T1, T2)>> seq = SelectWith(source, selector);

    return seq.Select(q => {
        IEnumerable<T1> q1 = q.Select(x => x.Item1);
        IEnumerable<T2> q2 = q.Select(x => x.Item2);

        return (q1, q2);
    });
}
```

#### Conditional execution of monadic expressions

Haskell | C# | Return Type
--------|----|------------
`when`   | -             | -
`unless` | -             | -

#### Monadic lifting operators

Haskell | C# | Return Type
--------|----|------------
`liftM`  | `Maybe.Lift` | `Func<Maybe<T>, Maybe<TResult>>`
`liftM2` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>>`
`liftM3` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>>`
`liftM4` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>>`
`liftM5` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>>`
`ap`     | `obj.Gather` | `Maybe<TResult>`

#### Extras

#### Monad Plus

Haskell | C# | Return Type
--------|----|------------
`mzero`   | `Maybe<T>.None` | `Maybe<T>`
`mplus`   | `obj.OrElse`    | `Maybe<T>`
`msum`    | `mseq.Sum`      | `Maybe<IEnumerable<T>>`
`mfilter` | `obj.Where`     | `Maybe<T>`
`guard`   | `Maybe.Guard`   | `Maybe.Unit`

#### `Maybe` specific functions

Haskell | C# | Return Type
--------|----|------------
`catMaybes`   | `mseq.CollectAny()`    | `Maybe<IEnumerable<T>>`
`isJust`      | `obj.IsSome`           | `bool`
`isNothing`   | `obj.IsNone`           | `bool`
`fromMaybe`   |                        |
`fromJust`    |                        |
`maybeToList` | `obj.ToEnumerable()`   | `IEnumerable<T>`
`maybe`       | `obj.Match()`          | `TResult`
`listToMaybe` | `seq.FirstOrNone()`    | `Maybe<T>`
`mapMaybe`    | `seq.SelectAny()`      | `IEnumerable<TResult>`

#### Further Readings
- [The Haskell 98 Report](http://www.haskell.org/onlinereport/monad.html)
- Haskell: [Data.Functor](https://hackage.haskell.org/package/base-4.9.1.0/docs/Data-Functor.html),
  [Control.Applicative](https://hackage.haskell.org/package/base-4.9.1.0/docs/Control-Applicative.html)
  and [Control.Monad](https://hackage.haskell.org/package/base-4.9.1.0/docs/Control-Monad.html)

Monad Tutorial
--------------

Again, we will use the Maybe type as an example.

### Functor

```csharp
public struct Maybe<T> {
    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) { ... }
}
```
The `Select` method must satisfy the **functor laws**:
1. **Identity.**  `Select` preserves the identity function.
2. **Composition.** `Select` preserves the composition operator.

If `m` is an instance of the `Maybe<T>` class, the first law says that the result of
`m.Select(x => x)` is equal to `m`, and the second law that:
```csharp
var lhs = m.Select(x => f(g(x)));
var rhs = m.Select(g).Select(f);
```
where `f` and `g` are two functions, are equals.

### Applicative

```csharp
public static class Maybe {
    public static Maybe<T> Of<T>(T value) { ... }
}

public struct Maybe<T> {
    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) { ... }

    public Maybe<TResult> Gather<TResult>(Maybe<Func<T, TResult>> applicative) { ... }
}
```

### Monad

A monad `Monad<T>` is simply a type with at least two operations
```csharp
public static class Maybe {
    public static Maybe<T> Of<T>(T value) { ... }
}

public struct Maybe<T> {
    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> binder) { ... }
}
```
that satisfy the _monad laws_. We won't discuss them but, in plain English, they
say that `Of` is an identity for `Bind`, and that `Bind` is associative.

If one wishes to stay closer to the definition of monads from category theory,
a monad is rather defined by a unit element `Of` and two operations
`Select` and `Flatten` where `Select` must satisfy the _functor laws_.

### Triad: an alternate definition of a monad

```csharp
public static class Maybe {
    public static Maybe<T> Of<T>(T value) { ... }

    public static Maybe<T> Flatten<T>(Maybe<Maybe<T>> square) { ... }
}

public struct Maybe<T> {
    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) { ... }
}
```

From a triad to a monad: `Bind` derived from `Flatten` and `Select`,
```csharp
public struct Maybe<T> {
    public Maybe<TResult> Bind<TResult>(Func<T,Maybe<TResult>> binder) {
        return Maybe.Flatten(Select(binder));
    }
}
```
From a monad to a triad: `Select` derived from `Of` and `Bind`,
and `Flatten` derived from `Bind`.
```csharp
public struct Maybe<T> {
    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) {
        return Bind(x => Maybe.Of(selector(x)));
    }
}

public static class Maybe {
    public static Maybe<T> Flatten(Maybe<Maybe<T>> square) {
        return square.Bind(x => x);
    }
}
```

### Comonad

There are two equivalent ways to define a Comonad:
- `Counit`, `Cobind`
- `Counit`, `Map`, `Comultiply`

### Richer Monads

We follow (mostly) the proposed new terminology from the
[MonadPlus Reform](http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal).

Name | Haskell | Terminology used here
-----|---------|----------------------------------------
`Zero` | `mzero` | `Zero`, `None`, `Empty`, `Ok`...
`Plus` | `mplus` | `Plus`, `OrElse`...

#### MonadZero
A MonadZero is a monad with a left zero for `Bind`.

#### MonadMore
A MonadMore is a monad which is also a monoid and for which `Zero`
is a zero for `Bind`. This is what Haskell calls a MonadPlus.

#### MonadPlus
A MonadPlus is a monad which is also a monoid and for which `Bind`
is right distributive over `Plus`.

REVIEW: Haskell uses the term left distributive. Am I missing something?

#### MonadOr
A MonadOr is a monad which is also a monoid and for which `Unit` is
a left zero for `Plus`. Here, we prefer to use `OrElse` instead of `Plus` for the
monoid composition operation.

### .NET Framework types

Type             | Properties
---------------- | ------------------------
`IEnumerable<T>` |
`Nullable<T>`    |
`Func<T>`        |
`Lazy<T>`        |
`Task<T>`        |

### Further Readings
- The first public discussion of monads in the context of .NET seems to be due to
  [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx).
- A popular explanation of monads given by [Eric Lippert](http://ericlippert.com/category/monads/).
- A more abstract one by [Erik Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/).

Changelog
---------
