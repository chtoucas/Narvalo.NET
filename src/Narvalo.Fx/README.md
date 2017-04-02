Narvalo.Fx
==========

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), error types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`), simple disjoint union (`Either<T1, T2>`),
sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be declared stable. Some breaking
  changes are still in the work in the area of LINQ.
- Support the .NET Standard v1.0 and the PCL profile Profile259.
- Test coverage is starting to look good (75%). The number of functional tests
  is progressing too.
- C# documentation is largely missing.

### Content
- [Overview](#overview)
- [Unit Type](#maybe-type)
- [Maybe Type](#maybe-type)
- [Error Types](#error-types)
- [Either Type](#either-type)
- [Query Operators](#query-operators)
- [Utilities](#utilities)
- [Monadic API Tour](#monadic-api-tour)
- [Haskell to C# Walk-Through](#haskell-to-C-walk-through)
- [Typologia](#typologia)
- [Design Notes](#design-notes)
- [Changelog](#changelog)

**WARNING:** _I am currently in the process of rewriting this document._

--------------------------------------------------------------------------------

Overview
--------

This assembly encourages an applicative-style of programming, or functional-style
if you prefer: types are **immutable** and methods are **pure** - they are free
of side-effects.

The main namespace (`Narvalo.Applicative`) includes:
- `Unit`, a unit type.
- `Maybe<T>`, similar to `Nullable<T>` but without any constraint
  attached to the underlying type.
- Enhanced return types for methods which normally return `void`:
  * `Outcome` for lightweight error reporting.
  * `Fallible` for full exception capture.
- Enhanced return types for methods which normally return an object of type `T`:
  * `Outcome<T>` for lightweight error reporting.
  * `Fallible<T>` for full exception capture.
- `Result<T, TError>`, the most general Error type.
- `Either<T1, T2>`, an Either type.

The other namespace (`Narvalo.Linq`) is dedicated to the definition of new
query operators.

### Remarks
The types `Maybe<T>`, `Outcome<T>`, `Fallible<T>`, `Result<T, TError>` and
`Either<T1, T2>` are examples of **monads**, a concept popularized by the
[Haskell](https://www.haskell.org/) language. If you know nothing about monads
or Haskell, don't worry, no previous knowledge is required - in fact, we won't
explain what we mean by a monad until the [end](#typologia) of this document.

We will often say that something is _monadic_, which will simply mean that it is
applicable/available to any of the aforementioned monads. For instance, it is
well-known that the C# type system is not rich enough to make true monadic
constructions, but we can still create monad-like types as we do.
Our implementation of monads follows the Haskell API closely but, of course,
adapted to make it more palatable and hopefully feel natural to C#-developers
(see [below](#monadic-api-tour) for more details on this). In particular, we make
sure that all monads also support the query expression syntax
[[Query expression pattern](https://github.com/dotnet/csharplang/blob/master/spec/expressions.md#the-query-expression-pattern)].

The astute reader will have noticed that some of the common monads are missing,
they were not included on purpose. In the context of C#, I am yet to be
convinced of their usefulness and practicability (for skeleton definitions of
`IO`, `Reader` and `State`,
see [here](https://github.com/chtoucas/Brouillons/tree/master/src/play/Functional/Monadic)).

--------------------------------------------------------------------------------

Unit Type
---------

The [unit type](https://en.wikipedia.org/wiki/Unit_type) is not the same as
the [void type](https://en.wikipedia.org/wiki/Void_type).

The CLR includes a [void type](https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Void.cs)

--------------------------------------------------------------------------------

Maybe Type
----------

The `Maybe<T>` struct is a lot like the `Nullable<T>` class but without any
restriction on the underlying type: _it provides a way to tell the absence or
the presence of a value_ - this class is sometimes referred to as the _Option type_.
For value types, `T?` offers a much better alternative.

There is a [proposal](https://github.com/dotnet/csharplang/blob/master/proposals/nullable-reference-types.md)
to add nullable reference types to C#, nevertheless it won't render this type
obsolete; even if they might look similar, they carry different semantics.

#### Construction / Deconstruction
A `Maybe<T>` object exists in two states, it either contains a value or it does
not:
```csharp
var some = Maybe.Of("value");
var none = Maybe<string>.None;
```
You can check afterwards its status by querying the property `IsSome`, which is
true in the first case and false in the second one - the property `IsNone` is
the negation of `IsSome`. If it is easy to wrap a value into a maybe, but you
will soon wonder why it is not possible to recover it later on - there is a
property named `Value` exactly for that but it is only accessible internally.
As we will learn, this is not the recommended way of doing things but, if
you really insist, the type supports deconstruction:
```csharp
(bool isSome, T value) = maybe;
```
The deconstruction is not a "safe" operation. Before accessing `value`, you
should always check if `isSome` is true - when it is not, `value` is set to
`default(T)`.

--------------------------------------------------------------------------------

Error Types
-----------

Typical use cases:
- To encapsulate the result of a computation with lightweight error reporting
  to the caller in the form of a string: `Outcome` and `Outcome<T>`.
- To encapsulate the result of a computation with full exception capture
  (`ExceptionDispatchInfo`): `Fallible` and `Fallible<T>`.
- In all other cases: `Result<T, TError>`.

Of course, we could have gone away with one single type, but at the expense
of complicated type signatures. The correspondence is as follows:

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

### `Outcome`

#### Construction / Deconstruction
```csharp
var success = Outcome.Ok;
var failure = Outcome.FromError("My error message.");
```

```csharp
(bool succeed, string error) = outcome;
```

### `Outcome<T>`

#### Construction / Deconstruction
```csharp
var success = Outcome.Of(1);
var failure = Outcome<int>.FromError("My error message.");
```

```csharp
(bool succeed, T value, string error) = outcome;
```

### `Fallible`

#### Construction
If `edi` is an object of type `ExceptionDispatchInfo`:
```csharp
var success = Fallible.Ok;
var failure = Fallible.FromError(edi);
```

```csharp
(bool succeed, ExceptionDispatchInfo exceptionInfo) = fallible;
```

### `Fallible<T>`

#### Construction / Deconstruction
If `edi` is an object of type `ExceptionDispatchInfo`:
```csharp
var success = Fallible.Of(1);
var failure = Fallible<int>.FromError(edi);
```

```csharp
(bool succeed, T value, ExceptionDispatchInfo exceptionInfo) = fallible;
```

### `Result<T, TError>`

#### Construction / Deconstruction
```csharp
var success = Result<int, Error>.Of(1);
var failure = Result<int, Error>.FromError(new Error());
```

```csharp
(bool succeed, T value, TError error) = result;
```

--------------------------------------------------------------------------------

Either Type
-----------

#### Construction / Deconstruction
```csharp
var left = Either<int, long>.OfLeft(1);
var right = Either<int, long>.OfRight(1L);
```

```csharp
(bool isLeft, TLeft left, TRight right) = either;
```

--------------------------------------------------------------------------------

Query Operators
---------------

- [LINQ Extensions](#linq-extensions)
- [`Collect` and `CollectAny`](#collect-and-collectany)
- [Generalized Operators](#generalized-operators)
- [Specialized Operators](#specialized-operators)

For each new query operator, we define its behaviour regarding deferred or
immediate execution - to quote the [C# documentation](https://docs.microsoft.com/en-us/dotnet/articles/csharp/programming-guide/concepts/linq/classification-of-standard-query-operators-by-manner-of-execution),
_deferred execution_ means that the operation is not performed at the point
in the code where the query is declared. Additionally,
query operators that use deferred execution can be classified as _streaming_,
they do not have to read all the source data before they yield elements,
or _not streaming_, they must read all the source data before they can yield
a result element.

### LINQ Extensions

To enable any of them, you must first import the namespace `Narvalo.Linq`.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | :------:
Set            | `Append`             | `IEnumerable<T>`           | Streaming
|              | `Prepend`            | `IEnumerable<T>`           | Streaming
Element        | `FirstOrNone`        | `Maybe<T>`                 | -
|              | `LastOrNone`         | `Maybe<T>`                 | -
|              | `ElementAtOrNone`    | `Maybe<T>`                 | -
|              | `SingleOrNone`       | `Maybe<T>`                 | -
Aggregation    | `Aggregate` (reduce) | `T`                        | -
|              | `Aggregate` (fold)   | `TResult` or `TAccumulate` | -
Quantification | `IsEmpty`            | `bool`                     | -
Generation     | `EmptyIfNull`        | `IEnumerable<T>`           | -

All these operators are defined as extension methods (in `Qperators`) and expect
an `IEnumerable<T>` as input:
- `Append` (resp. `Prepend`) appends (resp. prepends) a new element to a sequence.
  **NB:** A much better [implementation](https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/AppendPrepend.cs)
  appears in later versions of `System.Linq`; it optimizes multiple calls to
  `Append` and `Prepend`.
- `FirstOrNone()` returns the first element of a sequence, or `Maybe<T>.None`
  if the sequence contains no elements.
- `FirstOrNone(predicate)` returns the first element of a sequence that satisfies the
  predicate, or `Maybe<T>.None` if no such element is found.
- `LastOrNone()` returns the last element of a sequence, or `Maybe<T>.None`
  if the sequence contains no elements.
- `LastOrNone(predicate)` returns the last element of a sequence that satisfies the
  predicate, or `Maybe<T>.None` if no such element is found.
- `ElementAtOrNone(index)` returns the element at the specified index in a
  sequence or `Maybe<T>.None` if the index is out of range.
- `SingleOrNone()` returns the only element of a sequence, or `Maybe<T>.None`
  if the sequence is empty or contains more than one element. **WARNING:**
  This operator differs in behaviour from the standard query `SingleOrDefault`
  which throws an exception if there is more than one element in the sequence.
- `SingleOrNone(predicate)` returns the only element of a sequence that satisfies
  a specified predicate, or `Maybe<T>.None`
  if no such element exists or there are more than one of them. **WARNING:**
  This operator differs in behaviour from the standard query `SingleOrDefault`
  which throws an exception if more than one element satisfies the predicate.
- `IsEmpty` returns true if the sequence is empty; otherwise false.
- `EmptyIfNull` returns a new empty sequence if the sequence is empty; otherwise
  it returns the sequence.

#### Reducing

#### Folding

### `Collect` and `CollectAny`

Operators that act on an `IEnumerable<Monad<T>>`.

Category | Operator | Return Type | Deferred |
-------- | -------- | ----------- | :------: |
Restriction | `CollectAny` | `IEnumerable<T>`        | Streaming
|           | `Collect`    | `Monad<IEnumerable<T>>` | Streaming

Accidentally, for all monads considered here, `Collect` is just a `CollectAny`
wrapped into a monad, even if it is not true in general.

#### `CollectAny`
For instance, applying `CollectAny` to the sequence defined by:
```csharp
yield return Maybe<int>.None;
yield return Maybe.Of(2);
yield return Maybe<int>.None;
yield return Maybe.Of(4);
yield return Maybe.Of(5);
```
would return a sequence of type `IEnumerable<int>` with three elements `2`, `4`
and `5`; it filters out the two _none_'s

#### `Collect`

### Generalized Operators

We also provide generalized operators accepting as arguments functions
that maps a value to a nullable, a Maybe, an Error or an Either.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | :------:
Projection  | `SelectWith` | `Monad<IEnumerable<TResult>>` | Streaming
Restriction | `WhereBy`    | `Monad<IEnumerable<T>>`       | Streaming
Set         | `ZipWith`    | `Monad<IEnumerable<TResult>>` | Streaming
Aggregation | `Reduce`     | `Monad<T>`                    | -
|           | `Fold`       | `Monad<TAccumulate>`          | -
Generation  | `Repeat`     | `Monad<IEnumerable<T>>`       | Streaming

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | :------:
Projection  | `SelectAny`  | `IEnumerable<TResult>`        | Streaming
Restriction | `WhereAny`   | `IEnumerable<T>`              | Streaming

#### `SelectWith`
`SelectWith` is a standard `Select` followed by a `Collect`.

#### `WhereBy`
Accidentally for all monads considered here, `WhereBy` is just a `WhereAny`
wrapped into a monad even if it is not true in general.

#### `ZipWith`
`ZipWith` is a standard `Zip` followed by a `Collect`.

### Specialized Operators

Operators that act on an `IEnumerable<Maybe<T>>`.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | :------:
Aggregation | `Sum` (*) | `Maybe<T>` | -

### Further readings

--------------------------------------------------------------------------------

Utilities
---------

### Infinite Sequences

### Recursion

--------------------------------------------------------------------------------

Monadic API Tour
----------------

### Core API

### Derived API

--------------------------------------------------------------------------------

Haskell to C# Walk-Through
--------------------------

We will use the Maybe type as an example. Below we use:
- `obj` for an object of type `Maybe<T>`.
- `kunc` for a function of type `Func<T, Maybe<TResult>>`.
- `seq` for an object of type `IEnumerable<T>`.
- `mseq` for an object of type `IEnumerable<Maybe<T>>`

All variants that return a `Maybe<Unit>` instead of a `Maybe<T>` (those that have
a suffix `_`) are not implemented.

Haskell | C# | Return Type
--------|----|------------
`>>=`    | `obj.Bind`         | `Maybe<TResult>`
`>>`     | `obj.ContinueWith` | `Maybe<TResult>`
`return` | `Maybe.Of`         | `Maybe<T>`
`fail`   | -                  | -
`fmap`   | `obj.Select`       | `Maybe<TResult>`

We do not implement `fail` as .NET has its own way of reporting errors.

### Basic monad functions

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

### Generalisations of list functions

Below `square` is an object of type `Maybe<Maybe<T>>`.

Haskell | C# | Return Type
--------|----|------------
`join`                       | `square.Flatten`    | `Maybe<T>`
`filterM`                    | `seq.WhereBy`       | `Maybe<IEnumerable<T>>`
`mapAndUnzipM`               | -                   | -
`zipWithM` / `zipWithM_`     | `seq.ZipWith`       | `Maybe<IEnumerable<TResult>>`
`foldM` / `foldM_`           | `seq.Fold`          | `Maybe<TAccumulate>`
`replicateM` / `replicateM_` | `Maybe.Repeat`      | `Maybe<IEnumerable<T>>`

#### `SelectUnzip` (C#), `mapAndUnzipM` (F#))
`mapAndUnzipM` is easily implemented using `Select` and `SelectWith`:
```csharp
public static Maybe<(IEnumerable<T1>, IEnumerable<T2>)> SelectUnzip<T, T1, T2>(
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

### Conditional execution of monadic expressions

Haskell | C# | Return Type
--------|----|------------
`when`   | -             | -
`unless` | -             | -

### Monadic lifting operators

Haskell | C# | Return Type
--------|----|------------
`liftM`  | `Maybe.Lift` | `Func<Maybe<T>, Maybe<TResult>>`
`liftM2` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>>`
`liftM3` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>>`
`liftM4` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>>`
`liftM5` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>>`
`ap`     | `obj.Gather` | `Maybe<TResult>`

### Monad Plus

Haskell | C# | Return Type
--------|----|------------
`mzero`   | `Maybe<T>.None` | `Maybe<T>`
`mplus`   | `obj.OrElse`    | `Maybe<T>`
`msum`    | `mseq.Sum`      | `Maybe<IEnumerable<T>>`
`mfilter` | `obj.Where`     | `Maybe<T>`
`guard`   | `Maybe.Guard`   | `Maybe.Unit`

### `Maybe` Specific Functions

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

--------------------------------------------------------------------------------

Typologia
---------

Again, we will use the Maybe type as an example.

### Monoid

### Functor

A _functor_ is a type with one operation
```csharp
public struct Maybe<T> {
    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) { ... }
}
```
that satisfies the **functor laws**:
1. **Identity.**  `Select` preserves the identity function.
2. **Composition.** `Select` preserves the composition operator.

If `m` is an instance of the `Maybe<T>` class, the first law says that the result of
`m.Select(x => x)` is equal to `m`, and the second law that:
```csharp
var lhs = m.Select(x => f(g(x)));
var rhs = m.Select(g).Select(f);
```
where `f` and `g` are arbitrary (composable) functions, are equals.

Outside .NET,

### Applicative Functor

```csharp
public static class Maybe {
    public static Maybe<T> Of<T>(T value) { ... }
}

public struct Maybe<T> {
    public Maybe<TResult> Select<TResult>(Func<T, TResult> selector) { ... }

    public Maybe<TResult> Gather<TResult>(Maybe<Func<T, TResult>> applicative) { ... }
}
```

### Alternative

### Monad

A _monad_ is simply a type with at least two operations
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

#### An alternate definition
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

### Computation or Container?

### F# Computation Expressions

### .NET Framework types

Type             | Properties
---------------- | ------------------------
`IEnumerable<T>` |
`Nullable<T>`    |
`Func<T>`        |
`Lazy<T>`        |
`Task<T>`        |

### A Glimpse of Category Theory

### Further Readings
- The first public discussion of monads in the context of .NET seems to be due to
  [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx).
- A popular explanation of monads given by [Eric Lippert](http://ericlippert.com/category/monads/).
- A more abstract one by [Erik Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/).

--------------------------------------------------------------------------------

Design Notes
------------

--------------------------------------------------------------------------------

Changelog
---------

--------------------------------------------------------------------------------
