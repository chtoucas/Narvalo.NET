 Narvalo.Fx
==========

Features implementations of some of the usual suspects from functional
programming: option type (`Maybe<T>`), error types (`Result<T, TError>`,
`Outcome<T>` and `Fallible<T>`) for Railway Oriented Programming, simple
disjoint union (`Either<T1, T2>`), sequence generators and LINQ extensions.

### Status
- The next release should be the first one to be declared stable (some breaking
  changes are still in the work in the area of LINQ).
- Support the .NET Standard v1.0 and the PCL profile Profile259.
- Test coverage is starting to look good (75%). The number of functional tests
  is progressing too.
- C# documentation is largely missing.

### Content
- [Overview](#overview)
- [Unit Type](#unit-type)
- [Nullable Type](#nullable-type)
- [Maybe Type](#maybe-type)
- [Railway Oriented Programming](#railway-oriented-programming)
- [Either Type](#either-type)
- [Query Operators and Generators](#query-operators-and-generators)
- [Recursion](#recursion)
- [Tour of the Monad Verbs](#tour-of-the-monad-verbs)
- [Haskell to C# Walk-Through](#haskell-to-C-walk-through)
- [Typologia](#typologia)
- [F# is better at functional programming!](#f-is-better-at-functional-programming)
- [Design Notes](#design-notes)
- [Changelog](#changelog)

**WARNING:** _I am currently in the process of rewriting this document._

--------------------------------------------------------------------------------

Overview
--------

This assembly encourages an applicative-style of programming, or functional-style
if you prefer: types are **immutable** and methods are **pure** - they are free
of side-effects and always return a new object.

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

The types `Maybe<T>`, `Outcome<T>`, `Fallible<T>`, `Result<T, TError>` and
`Either<T1, T2>` are examples of **monads**, a concept popularized by the
[Haskell](https://www.haskell.org/) language. If you know nothing about monads
or Haskell, don't worry, no previous knowledge is required - in fact, we won't
explain the term monad until the [end](#typologia) of this document.

We will often say that something is _monadic_, which will simply mean that it is
applicable/available to any of the aforementioned monads. For instance, "it is
well-known that the C# type system is not rich enough to make true monadic
constructions, but we can still create monad-like types as we do".
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

If you go to the [NuGet website](https://www.nuget.org), you will find many
packages that solve the same problems as we do; those that I found the most
interesting are [Optional](https://github.com/nlkl/Optional), and
[Chessie](https://github.com/fsprojects/Chessie) or
[RailwaySharp](https://github.com/gsscoder/railwaysharp) for Railway Oriented
Programming. Why write another one? This project started as an exercise to better
understand monads, it just happened that I thought it was solid enough to be
published; of course, I prefer my version too :relaxed:.

--------------------------------------------------------------------------------

Unit Type
---------

In C#, when a function does not return any value, it has a `void` return type
which, on close inspection, looks quite odd. The .NET CLR defines a
[Void](https://github.com/dotnet/coreclr/blob/master/src/mscorlib/shared/System/Void.cs)
type, but it does not allow you to use it as a normal type: it has internal
visibility and it cannot be used as a generic type parameter, e.g. `List<Void>`
is explicitly forbidden. Moreover, it breaks the composition of functions,
`Void` cannot appear in a function parameter.

In a functional-style of programming, one prefers a
[unit type](https://en.wikipedia.org/wiki/Unit_type) over a
[void type](https://en.wikipedia.org/wiki/Void_type). All functions can then be
treated equal, there is no longer a need to define custom delegates for
actions - for instance, `Func<unit, unit>` (instead of `Action`) and
`Func<T, unit>` (instead of `Action<T>`) are perfectly legal.

The unit type `Unit` (in `Narvalo.Applicative`) is both an empty struct and a
singleton, it has only one value `Unit.Default`. This type is not an original
creation, far from it, _rendons à César ce qui est à César_, it is largely
copied from
[Rx.NET](https://github.com/Reactive-Extensions/Rx.NET/blob/develop/Rx.NET/Source/src/System.Reactive/Unit.cs).
Personally, I like to make it look like a built-in type with:
```csharp
using unit = global::Narvalo.Applicative.Unit;
```
An implementation detail is that we make sure that a `Unit` instance is equal to
any _empty tuple literal_, the 0-tuples. The .NET team _"lovingly
[refers](https://github.com/dotnet/roslyn/issues/10429) to 0-tuples as nuples,
and 1-tuples as womples"_. Currently, there is no special syntax for writing
nuples or womples, but we might get `()` for nuples which would make the 0-tuple
a very natural unit type.

--------------------------------------------------------------------------------

Nullable Type
-------------

- [Querying](#nullable-querying)
- [Binding](#nullable-binding)

### <a name="nullable-querying"></a>Querying

Importing the namespace `Narvalo.Applicable` enables a subset of the
[Query expression pattern](https://github.com/dotnet/csharplang/blob/master/spec/expressions.md#the-query-expression-pattern)
for the `Nullable<T>` type, namely:

Method | C# Query Expression Syntax
------ | --------------------------
`Select`     | `select`
`Where`      | `where`
`SelectMany` | Multiple `from` clauses.
`Join`       | `join ... in ... on ... equals ...`
`GroupJoin`  | `join ... in ... on ... equals ... into ...`

These operators do not behave like those on `IEnumerable<T>`, they use
_immediate execution_.

**Caution.** It is not because something is possible, that you should use it and
abuse it. First, another programmer might not know that LINQ is not just for
`IEnumerable<T>` therefore might have problems understanding your code,
second, the result will always be less performant than hand-written code, and
lastly C# already offers nice syntactic sugars for nullables (the conditional
operators `?:` and `?.`, and the null-coalescing operator `??`). Nevertheless,
there are situations where a piece of code written in query syntax is much more
readable.

#### `Select`
`Select` allows to transform the enclosed value with a given selector:
```csharp
T? x = ...;
Func<T, TResult> selector = ...;

TResult? q = x.Select(selector);
TResult? q = from v in x select selector(v);
```
For instance, to take the square of a nullable integer:
```csharp
short? x = 1;
int? q = from i in x select i * i;
```
Joining the hard way via a subquery:
```csharp
(int, (int, int)?)? source = (1, (2, 3));
var q = from outer in source
        select (
            outer.Item1,
            (from inner in outer.Item2 select inner.Item1 + inner.Item2));
```
the result is `(1, 5)` of type `(int, int?)?`. I agree with you, this is a
rather contrived example but, wait, we will show you soon a better and simpler
solution (see `SelectMany`).

#### `Where`
`Where` allows to filter the enclosed value with a given predicate:
```csharp
T? x = ...;
Func<T, bool> predicate = ...;

T? q = x.Where(predicate);
T? q = from v in x where predicate(v) select v;
```
We can mix and match `where` and `select` clauses: take the square of a nullable
integer only if it is "even"
```csharp
int? x = 2;
int? q = from i in x where i % 2 == 0 select i * i;
```

#### `SelectMany`
Cross join,
```csharp
T1? x1 = ...;
T2? x2 = ...;
Func<T1, T2, TResult> resultSelector = ...;

TResult? q = x.SelectMany(_ => x2, resultSelector);
TResult? q = from v1 in x1
             from v2 in x2
             select resultSelector(v1, v2);
```
Outer join,
```csharp
T1? x1 = ...;
Func<T1, T2?> valueSelector = ...;
Func<T1, T2, TResult> resultSelector = ...;

TResult? q = x1.SelectMany(valueSelector, resultSelector);
TResult? q = from v1 in x1
             from v2 in valueSelector(v1)
             select resultSelector(v1, v2);
```
Let's rewrite the subquery example with `SelectMany`:
```csharp
(int, (int, int)?)? source = (1, (2, 3));
var q = from outer in source
        from inner in outer.Item2
        select (outer.Item1, inner.Item1 + inner.Item2);

```
the result is still `(1, 5)` but, this time, of type `(int, int)?`
instead of `(int, int?)?`; the operator `SelectMany` eliminates the hierarchical
structure of LINQ queries. Furthermore, the code is so much easier to read and
maintain. A good exercise is to write the same query by using only `HasValue`
and `Value`, or with pattern matching - of course, you won't come very often
across such a convoluted example.

#### `Join`
```csharp
(int, int)? x1 = (1, 2);
(int, int)? x2 = (2, 3);

var q = from t1 in x1
        join t2 in x2 on t1.Item2 equals t2.Item1
        select (t1.Item1, t2.Item2);
```
the result is still `(1, 3)` of type `(int, int)?`.

[Discuss Join vs SelectMany / In LINQ to Objects, Join is better, not here]

#### `GroupJoin`

### <a name="nullable-binding"></a>Binding
`Bind` allows to transform the enclosed value to a nullable which is then
"flattened":
```csharp
T? x = ...;
Func<T, TResult?> binder = ...;

TResult? q = x.Bind(binder);
```
`Bind` is really a special case of `SelectMany<T, TResult, TResult>`:
```csharp
TResult? q = x.SelectMany(binder, (_, v2) => v2);
TResult? q = from v1 in x
             from v2 in binder(v1)
             select v2;
```
In LINQ for Objects, `Bind` is even named `SelectMany`.

**Remark.** In fact, `Bind` is the most important method upon which
one can construct all the other operators; it is not the other way around, more
on this later.

--------------------------------------------------------------------------------

Maybe Type
----------

- [Construction / Deconstruction](#maybe-ctor)
- [Give me back the value!](#maybe-value)
- [Matching](#maybe-matching)
- [Programming for side-effects](#maybe-effects)
- [Querying](#maybe-querying)
- [Binding](#maybe-binding)
- [Design notes](#maybe-design)

We discuss `Maybe<T>` at length, the type is quite simple, nevertheless it
illustrates many principles that are applicable to the other monads, it's well
worth the time spent to study it.

The `Maybe<T>` struct is a lot like the `Nullable<T>` class but without any
restriction on the underlying type: _it provides a way to tell the absence or
the presence of a value_ - this class is usually referred to as the
_[option type](https://en.wikipedia.org/wiki/Option_type)_.

There is a [proposal](https://github.com/dotnet/csharplang/blob/master/proposals/nullable-reference-types.md)
to add nullable reference types to C#, nevertheless it should not render this
type obsolete. Even if the two types share the same objective, I think that they
will be used in different situations: `Maybe<T>` forces you to handle the
exceptional case, while a nullable value type does not - nothing prevents you
from calling the property `Value`, even if `HasValue` is false.

### <a name="maybe-ctor"></a>Construction / Deconstruction
A `Maybe<T>` object exists in two states, it either contains a value or it does
not. The constructor being private, to create a new instance, you use the static
factory method `Maybe.Of` or the static property `Maybe<T>.None`:
```csharp
var some = Maybe.Of("value");
var none = Maybe<string>.None;
```
Of course, passing `null` to `Maybe.Of<T>` returns `Maybe<T>.None`.
You can check afterwards the status of a "maybe" by querying the property `IsSome`,
which is true in the first case and false in the second one - there is also a
property `IsNone` which is the negation of `IsSome`. If it is easy to wrap a
value into a maybe, but you will soon wonder why it is not possible to get back
the value - there is a property named `Value` exactly for that but it is only
accessible internally. As we will learn, this is not the way things work with
monads but, if you really insist, the type supports deconstruction:
```csharp
(bool isSome, T value) = maybe;
```
Deconstruction is **unsafe**, before accessing `value`, you should always check
if `isSome` is true - when it is not, `value` is set to `default(T)` that is
`null` for reference types :worried:.

**Remark:** To check if a "maybe" contains a given value, rather than extracting
the enclosed value, you should use the `Contains` helper - there is also an
overload when you want to use a custom equality comparer.

When it comes to value types, there is really no reason to use `Maybe<T?>`
instead of `Maybe<T>`. For this exact reason, `Maybe.Of<T?>` returns an object
of type `Maybe<T>` **not** `Maybe<T?>`:
```csharp
int? value = 1;
Maybe<int> maybe = Maybe.Of(value);
```
Nevertheless, it is still possible to end up with an object of type `Maybe<T?>`,
e.g. `Maybe<int?>.None` (see also Binding below for a more realistic example).
Fortunately, we can always "flatten" the object:
```csharp
var maybe = Maybe<int?>.None;
Maybe<int> better = maybe.Flatten();
```

### <a name="maybe-value"></a>Give me back the value!

To repeat myself, this is not a recommended practice. Anyway,
- `ValueOrDefault()` returns the enclosed value if any; otherwise the default
  value of type `T`.
- `ValueOrElse(other)` returns the enclosed value if any; otherwise `other`.
  There is also an overload which accepts a factory as parameter. Beware,
  if `ValueOrElse(other)` never returns null, depending on the factory, the
  overload may well.
- `ValueOrThrow()` returns the enclosed value if any; otherwise throws an
  `InvalidOperationException`. There is also an overload which accepts a factory
   as parameter.

### <a name="maybe-matching"></a>Matching

### <a name="maybe-effects"></a>Programming for side-effects

### <a name="maybe-querying"></a>Querying
The `Maybe<T>` type supports a subset of the [Query expression pattern](https://github.com/dotnet/csharplang/blob/master/spec/expressions.md#the-query-expression-pattern),
namely:

Method | C# Query Expression Syntax
------ | --------------------------
`Select`     | `select`
`Where`      | `where`
`SelectMany` | Multiple `from` clauses.
`Join`       | `join ... in ... on ... equals ...`
`GroupJoin`  | `join ... in ... on ... equals ... into ...`

#### `Select`
If `maybe` is of type `Maybe<T>` and `selector` is a generic delegate
type `Func<T, TResult>`, then one can write:
```csharp
var q = maybe.Select(selector);
var q = from x in maybe select selector(x);
```
where `q` is of type `Maybe<TResult>`. The two syntaxes are strictly equivalent -
the C# compiler will transform the later into the former. Which one to use is a
matter of personal preferences, but notice how the query syntax makes it clear
that the selector applies to the value `x` enclosed in the object `maybe`, if
any. If `maybe` is "none", it does not contain any value and `q` is "none". This
is also the result when the selector returns `null`, whatever is the state of
`maybe`, "some" or "none".
```csharp
var none = Maybe<string>.None;
none.Select(x => x.Length) ≡ Maybe<int>.None;

var some = Maybe.Of("value");
some.Select(x => x.Length)     ≡ Maybe.Of(5);
some.Select(x => (string)null) ≡ Maybe<string>.None;
```
**Remark:** Of course, this is not valid C# code, but we will often use a virtual
operator `≡` to say that both sides are equal.

#### `Where`
If `maybe` is of type `Maybe<T>` and `predicate` is a generic delegate
type `Func<T, bool>`, then one can write equivalently:
```csharp
var q = maybe.Where(predicate)
var q = from x in maybe where predicate(x) select x;
```
where `q` is of type `Maybe<T>`.

#### `SelectMany`

#### `Join`

#### `GroupJoin`

### <a name="maybe-binding"></a>Binding

### <a name="maybe-design"></a>Design Notes

[Struct vs Class] [Storage]

--------------------------------------------------------------------------------

Railway Oriented Programming
----------------------------

- [`Outcome`](#rop-outcome)
- [`Outcome<T>`](#rop-outcomeT)
- [`Fallible`](#rop-fallible)
- [`Fallible<T>`](#rop-fallibleT)
- [`Result<T, TError>`](#rop-result)
- [Further readings](#rop-further)

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

### <a name="rop-outcome"></a>`Outcome`

```csharp
var success = Outcome.Ok;
var failure = Outcome.FromError("My error message.");
```

```csharp
(bool succeed, string error) = outcome;
```

### <a name="rop-outcomeT"></a>`Outcome<T>`

```csharp
var success = Outcome.Of(1);
var failure = Outcome<int>.FromError("My error message.");
```

```csharp
(bool succeed, T value, string error) = outcome;
```

### <a name="rop-fallible"></a>`Fallible`

If `edi` is an object of type `ExceptionDispatchInfo`:
```csharp
var success = Fallible.Ok;
var failure = Fallible.FromError(edi);
```

```csharp
(bool succeed, ExceptionDispatchInfo exceptionInfo) = fallible;
```

### <a name="rop-fallibleT"></a>`Fallible<T>`

If `edi` is an object of type `ExceptionDispatchInfo`:
```csharp
var success = Fallible.Of(1);
var failure = Fallible<int>.FromError(edi);
```

```csharp
(bool succeed, T value, ExceptionDispatchInfo exceptionInfo) = fallible;
```

### <a name="rop-result"></a>`Result<T, TError>`

```csharp
var success = Result<int, Error>.Of(1);
var failure = Result<int, Error>.FromError(new Error());
```

```csharp
(bool succeed, T value, TError error) = result;
```

### <a name="rop-further"></a>Further readings
- Railway Oriented Programming, [explanation](http://fsharpforfunandprofit.com/rop)
  and [sample codes](https://github.com/swlaschin/Railway-Oriented-Programming-Example)

--------------------------------------------------------------------------------

Either Type
-----------

- [Construction / Deconstruction](#either-ctor)
- [Matching](#either-matching)
- [Querying](#either-querying)
- [Binding](#either-binding)

The either type is the simplest possible
[discriminated union](https://en.wikipedia.org/wiki/Tagged_union).

### <a name="either-ctor"></a>Construction / Deconstruction
```csharp
var left = Either<int, long>.OfLeft(1);
var right = Either<int, long>.OfRight(1L);
```

```csharp
(bool isLeft, TLeft left, TRight right) = either;
```

### <a name="either-matching"></a>Matching

### <a name="either-querying"></a>Querying

### <a name="either-binding"></a>Binding

--------------------------------------------------------------------------------

Query Operators and Generators
------------------------------

- [LINQ Extensions](#linq-extensions)
- [`Collect` and `CollectAny`](#linq-collect)
- [Generalized Operators](#linq-generalized)
- [Specialized Operators](#linq-specialized)
- [Generators](#linq-generators)

For each new query operator, we define its behaviour regarding deferred or
immediate execution - to quote the [C# documentation](https://docs.microsoft.com/en-us/dotnet/articles/csharp/programming-guide/concepts/linq/classification-of-standard-query-operators-by-manner-of-execution),
_deferred execution_ means that the operation is not performed at the point
in the code where the query is declared. Additionally,
query operators that use deferred execution can be classified as _streaming_,
they do not have to read all the source data before they yield elements,
or _not streaming_, they must read all the source data before they can yield
a result element.

### <a name="linq-extensions"></a>LINQ Extensions

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
- `Append()` (resp. `Prepend()`) appends (resp. prepends) a new element to a sequence.
  **NB:** A much better [implementation](https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/AppendPrepend.cs)
  appears in later versions of `System.Linq`; it optimizes multiple consecutive
  calls to `Append` and `Prepend`.
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

### <a name="linq-collect"></a>`Collect` and `CollectAny`

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

### <a name="linq-generalized"></a>Generalized Operators

We also provide generalized operators accepting as arguments functions
that maps a value to a nullable, a Maybe, an Error or an Either.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | :------:
Projection  | `SelectWith` | `Monad<IEnumerable<TResult>>` | Streaming
Restriction | `WhereBy`    | `Monad<IEnumerable<T>>`       | Streaming
Set         | `ZipWith`    | `Monad<IEnumerable<TResult>>` | Streaming
Aggregation | `Reduce`     | `Monad<T>`                    | -
|           | `Fold`       | `Monad<TAccumulate>`          | -

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

### <a name="linq-specialized"></a>Specialized Operators

Operators that act on an `IEnumerable<Maybe<T>>`.

Category | Operator | Return Type | Deferred
-------- | -------- | ----------- | :------:
Aggregation | `Sum` (*) | `Maybe<T>` | -

### <a name="linq-generators"></a>Generators

Operator | Return Type | Deferred
-------- | ----------- | :------:
`Of`          | `IEnumerable<T>`           | Streaming
`Gather`      | `IEnumerable<T>`           | Streaming
`Unfold`      | `IEnumerable<T>`           | Streaming
`Repeat`      | `Monad<IEnumerable<T>>`    | Streaming

--------------------------------------------------------------------------------

Recursion
---------

--------------------------------------------------------------------------------

Tour of the Monad Verbs
-----------------------

### Core Verbs

### Derived Verbs

--------------------------------------------------------------------------------

Haskell to C# Walk-Through
--------------------------

- [Basic monad functions](#haskell-basic)
- [Generalisations of list functions](#haskell-list)
- [Conditional execution of monadic expressions](#haskell-exec)
- [Monadic lifting operators](#haskell-lift)
- [Monad Plus](#haskell-plus)
- [`Maybe` specific functions](#haskell-maybe)
- [Further readings](#haskell-further)

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

### <a name="haskell-basic"></a>Basic monad functions

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

### <a name="haskell-list"></a>Generalisations of list functions

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

### <a name="haskell-exec"></a>Conditional execution of monadic expressions

Haskell | C# | Return Type
--------|----|------------
`when`   | -             | -
`unless` | -             | -

### <a name="haskell-lift"></a>Monadic lifting operators

Haskell | C# | Return Type
--------|----|------------
`liftM`  | `Maybe.Lift` | `Func<Maybe<T>, Maybe<TResult>>`
`liftM2` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>>`
`liftM3` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>>`
`liftM4` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>>`
`liftM5` | `Maybe.Lift` | `Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>>`
`ap`     | `obj.Gather` | `Maybe<TResult>`

### <a name="haskell-plus"></a>Monad Plus

Haskell | C# | Return Type
--------|----|------------
`mzero`   | `Maybe<T>.None` | `Maybe<T>`
`mplus`   | `obj.OrElse`    | `Maybe<T>`
`msum`    | `mseq.Sum`      | `Maybe<IEnumerable<T>>`
`mfilter` | `obj.Where`     | `Maybe<T>`
`guard`   | `Maybe.Guard`   | `Maybe.Unit`

### <a name="haskell-maybe"></a>`Maybe` specific functions

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

### <a name="haskell-further"></a>Further readings
- [The Haskell 98 Report](http://www.haskell.org/onlinereport/monad.html)
- Haskell: [Data.Functor](https://hackage.haskell.org/package/base-4.9.1.0/docs/Data-Functor.html),
  [Control.Applicative](https://hackage.haskell.org/package/base-4.9.1.0/docs/Control-Applicative.html)
  and [Control.Monad](https://hackage.haskell.org/package/base-4.9.1.0/docs/Control-Monad.html)

--------------------------------------------------------------------------------

Typologia
---------

- Monoid
- Functor
- Applicative Functor
- Alternative Functor
- Monad
- Comonad
- Monad Plus
- .NET Framework types
- A Glimpse of Category Theory
- Further readings

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

### Alternative Functor

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

#### Computation vs Container

### Comonad

There are two equivalent ways to define a Comonad:
- `Counit`, `Cobind`
- `Counit`, `Map`, `Comultiply`

### Monad Plus

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
`Nullable<T>`    |
`IEnumerable<T>` |
`Func<T>`        |
`Lazy<T>`        |
`Task<T>`        |

#### Is `Nullable<T>` really a monad?

The `Bind` method is easy to write as an extension method:
```csharp
public static TResult? Bind<TSource, TResult>(
    this TSource? source,
    Func<TSource, TResult?> binder)
    where TSource : struct
    where TResult : struct
{
    // Argument validation omitted.
    return source is TSource value ? binder(value) : null;
}
```
- `Of` is casting `(T?)value`.
- `Zero` is `null`.
- `null` is a left zero for `Bind`; `null.Bind(binder) ≡ null`

`Nullable<Nullable<T>>` is not permitted.

#### Is `IEnumerable<T>` really a monad?

### A Glimpse of Category Theory

### Further readings
- The first public discussion of monads in the context of .NET seems to be due to
  [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx).
- A popular explanation of monads given by [Eric Lippert](http://ericlippert.com/category/monads/).
- A more abstract one by [Erik Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/).

--------------------------------------------------------------------------------

F# is better at functional programming!
---------------------------------------

I feel dumb to state the obvious, but it is interesting to see what F# has
to offer and why it is so much better. F# already has a
[unit type](https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/unit-type),
an
[option type](https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/options),
and a result type (`Result<'T,'TError>` in F# v4.1+).
More importantly, null is not permitted as a regular value. This is all very
nice, but the real big thing is Computation Expressions.

#### F# Computation Expressions

--------------------------------------------------------------------------------

Design Notes
------------

T4

Extension methods

Shadowing

--------------------------------------------------------------------------------

Changelog
---------

--------------------------------------------------------------------------------
