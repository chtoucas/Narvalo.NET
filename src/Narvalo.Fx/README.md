Narvalo.Fx
==========

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), error types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`), simple disjoint union (`Either<T1, T2>`),
sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be declared stable.
- Test coverage is starting to look good (75%). The number of functional tests
  is progressing too.
- C# documentation is largely missing.

### Content
- [Overview](#overview)
- [Maybe Type](#maybe-type)
- [Error Types](#error-types)
- [Either Type](#either-type)
- [LINQ Extensions](#linq-extensions)
- [Infinite Sequences](#infinite-sequences)
- [Derived API](#derived-api)
- [Design Notes](#design-notes)
- [Changelog](#changelog)

**WARNING:** _I am currently in the process of rewriting this document._

--------------------------------------------------------------------------------

Overview
--------

This assembly encourages an applicative-style of programming, or functional-style
if you prefer: types are **immutable** and methods are **pure** - they are free
of side-effects.

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

The other namespace is `Narvalo.Linq` dedicated to standard LINQ extensions.

### Remarks
Maybe, Error and Either are examples of _monads_, a concept popularized by Haskell.
If you know nothing about monads or Haskell, don't worry, no previous knowledge
is required.

The implementation of Maybe, Error and Either follows closely the Haskell API
but, of course, adapted to make more palatable to C#-developers (see
[below](#derived-api) for more details on this). These types also support the
query expression syntax [[Query expression pattern](https://github.com/dotnet/csharplang/blob/master/spec/expressions.md#the-query-expression-pattern)].

The astute reader will have notice that some of the common monads are missing;
they were not included on purpose. In the context of C#, I am yet to be
convinced of their usefulness and practicability (for skeleton definitions of
`IO`, `Reader` and `State`,
see [here](https://github.com/chtoucas/Brouillons/tree/master/src/play/Functional/Monadic)).

--------------------------------------------------------------------------------

Maybe Type
----------

The `Maybe<T>` class is a lot like the `Nullable<T>` class but without any
restriction on the underlying type: _it provides a way to tell the absence or
the presence of a value_. For value types, most of the time `T?` offers a much
better alternative. This class is sometimes referred to as the Option type.

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

--------------------------------------------------------------------------------

Either Type
-----------

--------------------------------------------------------------------------------

LINQ Extensions
---------------

For each new query operator, we define its behaviour regarding deferred or
immediate execution. To quote the [C# documentation](https://docs.microsoft.com/en-us/dotnet/articles/csharp/programming-guide/concepts/linq/classification-of-standard-query-operators-by-manner-of-execution),
_deferred execution_ means that the operation is not performed at the point
in the code where the query is declared. Additionally,
query operators that use deferred execution can be classified as _streaming_,
they do not have to read all the source data before they yield elements,
or _not streaming_, they must read all the source data before they can yield
a result element.

### Query Operators

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | --------
Set            | `Append`             | `IEnumerable<T>`           | Streaming
               | `Prepend`            | `IEnumerable<T>`           | Streaming
Element        | `FirstOrNone`        | `Maybe<T>`                 | -
               | `LastOrNone`         | `Maybe<T>`                 | -
               | `SingleOrNone`       | `Maybe<T>`                 | -
               | `ElementAtOrNone`    | `Maybe<T>`                 | -
Aggregation    | `Aggregate` (reduce) | `T`                        | -
               | `Aggregate` (fold)   | `TResult` or `TAccumulate` | -
Quantification | `IsEmpty`            | `bool`                     | -
Generation     | `EmptyIfNull`        | `IEnumerable<T>`           | -

#### Set Operations
`Append` appends a new element to a sequence and `Prepend` prepends a new
element(!).

#### Element Operations
`FirstOrNone()` returns the first element of a sequence, or `Maybe<T>.None`
if the sequence contains no elements.

`FirstOrNone(predicate)` returns the first element of a sequence that satisfies the
predicate, or `Maybe<TSource>.None` if no such element is found.

`LastOrNone()` returns the last element of a sequence, or `Maybe<T>.None`
if the sequence contains no elements.

`LastOrNone(predicate)` returns the last element of a sequence that satisfies the
predicate, or `Maybe<T>.None` if no such element is found.

`SingleOrNone()` returns the only element of a sequence, or `Maybe<T>.None`
if the sequence is empty or contains more than one element. **WARNING:**
Here we differ in behaviour from the standard query `SingleOrDefault` which
throws an exception if there is more than one element in the sequence.

`SingleOrNone(predicate)` returns the only element of a sequence that satisfies
a specified predicate, or `Maybe<T>.None`
if no such element exists or there are more than one of them. **WARNING:**
Here we differ in behaviour from the standard query `SingleOrDefault` which
throws an exception if more than one element satisfies the predicate.

`ElementAtOrNone(index)` returns the element at the specified index in a
sequence or `Maybe<T>.None` if the index is out of range.

#### Aggregation Operations

#### Quantification Operations
`IsEmpty` returns true if the sequence is empty; otherwise false.

#### Generation Operations
`EmptyIfNull` returns a new empty sequence if the sequence is empty; otherwise
it returns the sequence.

### Specialized Operators

Operators that act on an `IEnumerable<Monad<T>>`.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | --------
Restriction    | `Collect`        | `Monad<IEnumerable<T>>` | Streaming
               | `CollectAny` (*) | `IEnumerable<T>`        | Streaming
Aggregation    | `Sum` (**)       | `Maybe<T>`              | -

(*) Not available for `Either<T1, T2>`
(**) Only available for `Maybe<T>`.

#### `CollectAny` and `Collect` for `IEnumerable<Maybe<T>>`
For instance, applying `CollectAny` to the sequence of type
`IEnumerable<Maybe<int>>` and defined by:
```csharp
yield return Maybe<int>.None;
yield return Maybe.Of(2);
yield return Maybe<int>.None;
yield return Maybe.Of(4);
yield return Maybe.Of(5);
```
would return a sequence with the three elements `2`, `4` and `5`; it filters out
the two _none_'s

### Generalized LINQ Operators

We also provide generalized LINQ operators accepting as arguments functions
that maps a value to a nullable, a Maybe, an Error or an Either.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | --------
Projection     | `SelectAny`  | `IEnumerable<TResult>`        | Streaming
               | `SelectWith` | `Monad<IEnumerable<TResult>>` | Streaming
Restriction    | `WhereAny`   | `IEnumerable<T>`              | Streaming
               | `WhereBy`    | `Monad<IEnumerable<T>>`       | Streaming
Set            | `ZipWith`    | `Monad<IEnumerable<TResult>>` | Streaming
Aggregation    | `Reduce`     | `Monad<T>`                    | -
               | `Fold`       | `Monad<TAccumulate>`          | -
Generation     | `Repeat`     | `Monad<IEnumerable<T>>`       | Streaming

NB: `SelectAny` and `WhereAny` are not supported by `Either<T1, T2>` and
`Result<T, TError>`.

### Further readings

--------------------------------------------------------------------------------

Infinite Sequences
------------------

--------------------------------------------------------------------------------

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

--------------------------------------------------------------------------------

Design Notes
------------

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

### An alternate specification for a monad

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

--------------------------------------------------------------------------------

Changelog
---------

--------------------------------------------------------------------------------
