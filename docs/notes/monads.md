
**WARNING**: These are my reading notes on the subject, this is NOT a tutorial.

Introduction
------------

Monads have the undeserved reputation of being hard. It is certainly due to the
fact that many people try to explain them using the jargon of category theory.
Truth to be told, we don't need to understand the theory behind to make good
use of monads.

The C# type system is not rich enough to make general monadic constructions
but it gives developers access to some powerful monadic concepts in a very
friendly way. In fact, monad theory has clearly influenced the design of some
core parts of the .NET framework; LINQ and the Reactive extensions being the
most obvious proofs of that.

**A way to think about a monad is that it maps a value to a richer structure
which satisfies a set of natural rules from which we can derive a very rich vocabulary.**

Whenever possible, we illustrate the discussion with analogies from
the set of natural numbers, the boolean algebra, lists in the context of F# and
LINQ in C#. Beware, these analogies are not always accurate (most of the time they ain't),
but they should give you a feeling of what's going on.

We also provide examples from Haskell, but if you do not have any knowledge of
it, feel free to skip them.

References:

- The first public discussion of monads in the context of .NET seems to be due to
  [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx),
- A popular explanation of monads by [Eric Lippert](http://ericlippert.com/category/monads/),
- A more abstract one by [Erik Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/).

Before moving to monads, we explain two preliminary concepts: monoids and functors.
They are not necessary to understand monads but they are sufficiently simple that
I could not resist including them.

Monoid
------

In Haskell, one defines:
```haskell
-- Empty element.
mempty :: a
-- Append method.
mappend :: a -> a -> a

-- First law.
mappend mempty x = x
-- Second law.
mappend x mempty = x
-- Third law.
mappend x (mappend y z) = mappend (mappend x y) z
```

A monoid has a unit element _Empty_ and a closed binary operation _Append_ which
satisfy the **monoid laws**:

1. _Empty_ is a left identity for _Append_.
2. _Empty_ is a right identity for _Append_.
3. _Append_ is associative.

Remarks: A closed binary operation is a method that maps two values of a given type
to a value of the same type. We say that _Empty_ is a neutral element for _Append_.

For natural numbers, _Empty_ is `0` and _Append_ is the addition `+`.
For the boolean algebra, _Empty_ is `False` and _Append_ is `∨`,
the logical disjunction OR. The rules should then be familiar to anyone:

1. `0 + x = x` and `False ∨ P = P`.
2. `x + 0 = x` and `P ∨ False = P`.
3. `x + (y + z) = (x + y) + z` and `P ∨ (Q ∨ R) = (P ∨ Q) ∨ R`.

#### F\# Lists

_Empty_ is the empty list `[]` and _Append_ is `List.append`,
but the concatenation operator `@` works too.
The first law says that prepending an empty list to a list does not change the list:
```fsharp
// Result is [1; 2; 3].
let r = [] @ [1; 2; 3]
```
The second law says that appending an empty list to a list does not change the list:
```fsharp
// Result is [1; 2; 3].
let r = [1; 2; 3] @ []
```
The third law says that when concatenating several lists together, **we don't
have to worry about operator precedence**:
```fsharp
// On both sides, the result is [1; 2; 3; 4; 5; 6].
let lhs = [1; 2] @ ( [3; 4] @ [5; 6] )
let rhs = ( [1; 2] @ [3; 4] ) @ [5; 6]
```
Indeed, on the LHS, we obtain `[1; 2] @ [3; 4; 5; 6]` _i.e._ `[1; 2; 3; 4; 5; 6]`
and, on the RHS, we obtain `[1; 2; 3; 4] @ [5; 6]` _i.e._ `[1; 2; 3; 4; 5; 6]`.

#### LINQ

_Empty_ is the empty sequence `Enumerable.Empty<T>()` and _Append_
is the `Concat` method. If `q`, `r` and `s` are of type `IEnumerable<T>`, the monoid
laws say:
```csharp
var empty = Enumerable.Empty<T>();

// First law. Result is q.
var r1 = empty.Concat(q);

// Second law. Result is q.
var r2 = q.Concat(empty);

// Third law. Both sides are equal.
var lhs = q.Concat(r.Concat(s));
var rhs = q.Concat(r).Concat(s);
```

#### Pseudocode in C\#

It is just a matter of generalizing the LINQ definitions to an hypothetical `Monoid` class:
```csharp
// Skeleton definition of a monoid.
public class Monoid {
    // Empty property.
    public static Monoid Empty {
        get { throw new NotImplementedException(); }
    }

    // Append method.
    public Monoid Append(Monoid other) {
        throw new NotImplementedException();
    }
}
```
If `m`, `m1`, `m2` and `m3` are of type `Monoid`, they must follow the following rules:
```csharp
// First law. Result must be equal to m.
var r1 = Monoid.Empty.Append(m);

// Second law. Result must be equal to m.
var r2 = m.Append(Monoid.Empty);

// Third law. Both sides must be equal.
var lhs = m1.Append(m2.Append(m3));
var rhs = m1.Append(m2).Append(m3);
```

There ares sevaral instances of monoids in C\#. For instance,

Type             | _Empty_                 | _Append_
---------------- | ----------------------- | -----------------------------------
`int`            | `0`                     | `+`
`int`            | `1`                     | `*`
`bool`           | `false`                 | `||`
`bool`           | `true`                  | `&&`
`string`         | `String.Empty`          | `+`
`IEnumerable<T>` | `Enumerable.Empty<T>()` | `Enumerable.Concat`

### Standard Extension: Concat aka Reduce

Haskell also includes an operation `mconcat` which derives from `mempty` and `mappend`.
_Concat_ is a generalization of _Append_ to an arbitrary number of lists.
```haskell
-- Concat method
mconcat :: [a] -> a
mconcat = foldr mappend mempty
```

For F# lists, _Concat_ is `List.concat`. For instance,
```fsharp
// Result is [1; 2; 3; 4; 5; 6]
let r = List.concat [ [1; 2]; [3; 4]; [5; 6] ]
```

For LINQ,
```csharp
public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> @this) {
    return @this.Aggregate(Enumerable.Empty<T>(), (seq1, seq2) => seq1.Concat(seq2));
}

// Using the query expression syntax, this is even clearer.
public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> @this) {
    return from list in @this
           from item in list
           select item;
}
```

and for our hypothetical monoid class, this translates to:
```csharp
public static Monoid Reduce(this IEnumerable<Monoid> @this) {
    return @this.Aggregate(Monoid.Empty, (m1, m2) => m1.Append(m2));
}
```
_i.e._ we can **always reduce a list of monoid values to a single monoid value**.
So, if `Concat` may be a very natural name for lists, for more general monoids,
`Reduce` is a much better fit, closer to its real meaning.

### Weaker Forms: Magma and Semigroup

A **magma** only defines a closed binary operation _Append_. The set of binary
trees for a given type is an example of a (free) magma.

A **semigroup** is a magma for which _Append_ is associative, _i.e._ it is almost
a monoid but without the requirement of a neutral element.

```csharp
public class Semigroup {
    // Append method.
    public Semigroup Append(Semigroup other) {
        throw new NotImplementedException();
    }
}
```
If `a`, `b` and `b` are of type `Semigroup`, they must follow the following rule:
```csharp
// Both sides must be equal.
var lhs = a.Append(b.Append(c));
var rhs = a.Append(b).Append(c);
```
Previously, we defined a _Concat_ method whenever we had a monoid
but notice that it required the existence of a neutral element.
Even if semigroups do not have this buddy, with some restrictions, we can achieve
something similar:
```csharp
public static class Semigroup {
    // Reduce a *non-empty* sequence using Append.
    public static Semigroup Sum(this IEnumerable<Semigroup> @this) {
        using (var iter = @this.GetEnumerator()) {
            if (!iter.MoveNext()) {
                throw new InvalidOperationException("Source sequence is empty.");
            }

            Semigroup retval = iter.Current;

            while (iter.MoveNext()) {
                retval = retval.Append(iter.Current);
            }

            return retval;
        }
    }
}
```
If the semigroup is also a monoid, `Concat` with non-empty sequences is equivalent to `Sum`.

### Summary

A monoid defines an associative operation _Append_ and a neutral element
_Empty_ for _Append_.

Associativity means that we can divide a sequence of operations into smaller steps
without having to worry about operator precedence.

The existence of a neutral element means that we can always reduce a sequence of values
to a single value.

#### Further readings

- Haskell, [Data.Monoid](https://hackage.haskell.org/package/base-4.7.0.1/docs/Data-Monoid.html)
  and [Data.Semigroup](https://hackage.haskell.org/package/semigroups-0.8/docs/Data-Semigroup.html).
- F# for fun and profit, [Monoids without tears](http://fsharpforfunandprofit.com/posts/monoids-without-tears/).

Functor
-------

In Haskell, one defines:
```haskell
-- Map method.
fmap :: (a -> b) -> f a -> f b

-- First law.
fmap id == id
-- Second law.
fmap (f . g) == fmap f . fmap g
```

A functor has a _Map_ operation that satisfy the **functor laws**:

1. _Map_ preserves the identity map,
2. _Map_ preserves the composition operator.

#### F\# Lists

_Map_ is `List.map`.
The first law says that iterating over list and returning the unmodified items
is the same as iterating over the list:
```fsharp
// Result is [1; 2].
let r = [1; 2] |> List.map id
```
This is obvious, applying `id` to each element we get `[id(1); id(2)]` _i.e._ `[1; 2]`.
The second law says that iterating over a list and applying `g`
then `f` to each item is the same as iterating, applying `g`, iterating again
and applying `f`. For instance,
```fsharp
let f x = -x
let g x = x * x
let h = f << g

// On both sides, the result is [-1; -4].
let lhs = List.map h <| [1; 2]
let rhs = List.map f << List.map g <| [1; 2]
```
Indeed, on the LHS, we obtain `[h(1); h(2)]` _i.e._ `[-1; -4]`
and, on the RHS, we obtain `[g(1); g(2)]` _i.e._ `[1; 4]`
then `[f(1); f(4))]` _i.e._ `[-1; -4]`.

#### LINQ

_Map_ is the `Select` method:
```csharp
// First law. In both cases, result is q.
var r1 = q.Select(_ => _);
// Using the query expression syntax.
var r2 = from _ in q select _;

// Second law. Both sides are equal.
var lhs1 = q.Select(_ => f(g(_)));
var rhs1 = q.Select(g).Select(f);
// Using the query expression syntax.
var lhs2 = from _ in q select f(g(_));
var rhs2 = from item
               in (from _ in q select g(_))
           select f(item);
```

#### Pseudocode in C\#

```csharp
// Skeleton definition of a functor.
public class Functor<T> {
    // Map method.
    public Functor<TResult> Map<TResult>(Func<T, TResult> selector) {
        throw new NotImplementedException();
    }
}

// Functor laws.
public static class FunctorLaws {
    public static void FirstLaw<T>(Functor<T> m) {
        // First law. Result must be equal to m.
        var r = m.Map(_ => _);
    }

    public static void SecondLaw<T1, T2, T3>(Functor<T1> m, Func<T2, T3> f, Func<T1, T2> g) {
        // Second law. Both sides must be equal.
        var lhs = m.Map(_ => f(g(_)));
        var rhs = m.Map(g).Map(f);
    }
}
```

### Summary

#### Further readings

- Haskell, [Data.Functor](https://hackage.haskell.org/package/base-4.7.0.1/docs/Data-Functor.html)

Applicative Functor
-------------------

In Haskell, one defines:
```haskell
-- Pure method.
pure :: a -> f a
-- Gather method.
(<*>) :: f (a -> b) -> f a -> f b

-- First law.
pure id <*> v = v
-- Second law.
pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
-- Third law.
pure f <*> pure x = pure (f x)
-- Fourth law.
u <*> pure y = pure ($ y) <*> u
```

For F# lists,
```fsharp
// Pure method.
let pure x = [x]
```
```fsharp
let f = id
let g x = 2 * x
let h x = 3 * x
```

For LINQ,
```csharp
public static class Sequence {
    // Create a list of one element from a value.
    public static IEnumerable<T> Pure<T>(T value) {
        yield return value
    }

    // For instance, with
    //  f: x -> x
    //  g: x -> 2 * x
    //  h: x -> 3 * x
    // lifting [1; 2] with [f; g; h] gives:
    //  [1; 2] -> [f(1); f(2); g(1); g(2); h(1); h(2)]
    //              = [1; 2; 2; 4; 3; 6]
    public static IEnumerable<TResult> Gather<TSource, TResult>(
        this IEnumerable<TSource> @this,
        IEnumerable<Func<T, TResult>> funs)
    {
        foreach (var fun in funs) {
            foreach (var item in @this) {
                yield return funs(item);
            }
        }
    }
}
```

In C#,
```csharp
// Skeleton definition of an applicative functor.
public static class Applicative<T> {
    // Gather method.
    public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> funs) {
        throw new NotImplementedException();
    }
}

public static class Applicative {
    // Pure method.
    public static Applicative<T> Pure<T>(T value) {
        throw new NotImplementedException();
    }
}
```

### Further readings

- Haskell, [Control.Applicative](https://hackage.haskell.org/package/base-4.5.0.0/docs/Control-Applicative.html)

Monad
-----

References:

- [The Haskell 98 Report](http://www.haskell.org/onlinereport/monad.html)
- [Control.Monad](https://hackage.haskell.org/package/base-4.6.0.1/docs/Control-Monad.html)

In Haskell:
```haskell
-- Bind method.
(>>=) :: forall a b. m a -> (a -> m b) -> m b
-- Return method.
return :: a -> m a

-- First law: Return is a left identity for Bind.
return a >>= f == f a
-- Second law: Return is a right identity for Bind.
m >>= return == m
-- Third law: Bind is associative.
m >>= (\x -> f x >>= g) == (m >>= f) >>= g
```

A monad has a unit element _Return_ and a _Bind_ operation that satisfy the **monad laws**:

- _Return_ is the identity for _Bind_.
- _Bind_ is associative.

Since _Bind_ is not necessary a commutative operation, the first law really
means two things:

- _Return_ is a left identity for _Bind_,
- _Return_ is a right identity for _Bind_.

In arithmetic, `Return` would be `1` and `Bind` would be `*`.
For the boolean algebra, `Return` would be `True` and `Append` would be `∧`,
the logical conjunction AND. The rules should then be familiar to anyone:

Rule           | Translation
-------------- | ---------------------------------------------------------------
Left identity  | `1 * x = x` and `True ∧ P = P`
Right identity | `x * 1 = x` and `P ∧ True = P`
Associativity  | `x * (y * z) = (x * y) * z` and `P ∧ (Q ∧ R) = (P ∧ Q) ∧ R`

For LINQ,
```csharp
public static class Sequence {
    // Map each element of a list to a list, resulting in a list of lists
    // which is then flattened.
    public static IEnumerable<TResult> Bind<TSource, TResult>(
        this IEnumerable<TSource> @this,
        Func<TSource, IEnumerable<TResult>> fun)
    {
        return from _ in @this
               from item in fun(_)
               select item;
    }

    // Create a list of one element from a value.
    public static IEnumerable<T> Single<T>(T value) {
        yield return value;
    }
}

// First law: Iterating over a list of one element and creating a list by applying f
// to this unique value is the same as creating a list by applying f to the unique element.
// For instance, with f: x -> [x; 2], on the LHS, we have:
//  1 -> [1]
//    -> [f(1)] = [[1; 2]]          (apply f)
//    -> [1; 2]                     (flatten)
// and, on the RHS:
//  f(1) = [1; 2]
Sequence.Single(value).Bind(f) == f(value);
// or using the query expression syntax:
from _ in Sequence.Single(value) from item in f(_) select item
    == f(value);

// Second law: Iterating over a list and for each element returning a list containing only
// this element is the same as iterating over the list.
// For instance,
//  [1; 2; 3] -> [[1]; [2]; [3]]    (apply Single)
//            -> [1; 2; 3]          (flatten)
q.Bind(Sequence.Single) == q;
// or using the query expression syntax:
from _ in q from item in Sequence.Single(_) select item
    == q;

// Third law: Bind is associative.
// For instance, with
//  f: x -> [x; 1]
//  g: x -> [x; 2]
// we have on the LHS:
//  [3; 4] -> [f(3); f(4)]                              (apply f)
//              = [[3; 1]; [4; 1]]
//         -> [[g(3); g(1)]; [g(4); g(1)]]              (apply g)
//              = [[[3; 2]; [1; 2]]; [[4; 2]; [1; 2]]]
//         -> [[3; 2; 1; 2]; [4; 2; 1; 2]]              (flatten the inner seq)
//         -> [3; 2; 1; 2; 4; 2; 1; 2]                  (flatten the outer seq)
// and on the RHS:
//  [3; 4] -> [f(3); f(4)]                              (apply f)
//              = [[3; 1]; [4; 1]]
//         -> [3; 1; 4; 1]                              (flatten)
//         -> [g(3); g(1); g(4); g(1))]                 (apply g)
//              = [[3; 2]; [1; 2]; [4; 2]; [1; 2]]
//         -> [3; 2; 1; 2; 4; 2; 1; 2]                  (flatten)
q.Bind(_ => f(_).Bind(g)) == q.Bind(f).Bind(g);
// or using the query expression syntax:
from _ in q
from item
    in (from outer in f(_) from inner in g(outer) select inner)
select item
    == from _ in (from outer in q from inner in f(outer) select inner)
       from item in g(_)
       select item;
```

In C#:
```csharp
// Skeleton definition of a monad.
public class Monad<T> {
    // Bind method.
    public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> fun) {
        throw new NotImplementedException();
    }
}

public static class Monad {
    // Return method.
    public static Monad<T> Return<T>(T value) {
        throw new NotImplementedException();
    }
}

// Monad laws.
public static class MonadLaws {
    // First law: Return is a left identity for Bind.
    public static void FirstLaw<X, Y>(Func<X, Monad<Y>> f, X value) {
        Monad.Return(value).Bind(f) == f(value);
    }

    // Second law: Return is a right identity for Bind.
    public static void SecondLaw<X>(Monad<X> m) {
        m.Bind(Monad.Return) == m;
    }

    // Third law: Bind is associative.
    public static void ThirdLaw<X, Y, Z>(Monad<X> m, Func<X, Monad<Y>> f, Func<Y, Monad<Z>> g) {
        m.Bind(_ => f(_).Bind(g)) == m.Bind(f).Bind(g);
    }
}
```

Haskell also provides a `fail` method which is not part of the standard definition
of a monad. It is mostly used for pattern matching failure, something we do not
have in .NET.

### Extension Methods

### Query Expression Syntax

### Computation Expressions

Functions in the Kleisli Category
---------------------------------

A function in the Kleisli category is simply a function that maps a value to a monad.
Of course, functions can be invoked or composed with another function.

In Haskell:
```haskell
-- Invoke method.
(=<<) :: Monad m => (a -> m b) -> m a -> m b
f =<< x = x >>= f

-- Compose method.
(>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c
f >=> g = \x -> f x >>= g

-- ComposeBack method.
(<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c
(<=<) = flip (>=>)
```

In C#:
```csharp
public delegate Monad<TResult> Kunc<in TSource, TResult>(TSource arg);

public static partial class KuncExtensions
{
    // Invoke method.
    public static Monad<TResult> Invoke<TSource, TResult>(
        this Kunc<TSource, TResult> @this,
        Monad<TSource> monad)
    {
        return monad.Bind(@this);
    }

    // Compose method.
    public static Kunc<TSource, TResult> Compose<TSource, TMiddle, TResult>(
        this Kunc<TSource, TMiddle> @this,
        Kunc<TMiddle, TResult> kun)
    {
        return _ => @this.Invoke(_).Bind(kun);
    }

    // ComposeBack method.
    public static Kunc<TSource, TResult> ComposeBack<TSource, TMiddle, TResult>(
        this Kunc<TMiddle, TResult> @this,
        Kunc<TSource, TMiddle> kun)
    {
        return _ => kun.Invoke(_).Bind(@this);
    }
}
```

`Invoke` being just `Bind` with the arguments flipped, we can then rewrite
the monad laws in a more readable fashion.

In Haskell:
```haskell
-- First law: Return is a left identity for Compose.
return >=> g ≡ g
-- Second law: Return is a right identity for Compose.
f >=> return ≡ f
-- Third law: Compose is associative.
(f >=> g) >=> h ≡ f >=> (g >=> h)
```

In C#:
```csharp
public static class MonadLaws {
    // First law: Return is a left identity for Compose.
    public static void FirstLaw<X, Y>(Kunc<X, Y> g, X value) {
        Kunc<X, X> kReturn = Monad.Return;

        kReturn.Compose(g).Invoke(value) == g(value);
    }

    // Second law: Return is a right identity for Compose.
    public static void SecondLaw<X, Y>(Kunc<X, Y> f, X value) {
        f.Compose(Monad.Return).Invoke(value) == f(value);
    }

    // Third law: Compose is associative.
    public static void ThirdLaw<X, Y, Z, T>(Kunc<X, Y> f, Kunc<Y, Z> g, Kunc<Z, T> h, X value) {
        (f.Compose(g)).Compose(h).Invoke(value) == f.Compose(g.Compose(h)).Invoke(value);
    }
}
```

Triad: Monad Revisited
----------------------

If one wishes to stay closer to the definition of monads from category theory,
a Monad is equivalently defined by a unit element `Return` and two operations
`Map` and `Multiply` where `Map` must satisfy the functor laws.

In Haskell:
```haskell
-- Return method.
return :: a -> m a
-- Map method.
fmap :: (a -> b) -> f a -> f b
-- Multiply method.
join :: Monad m => m (m a) -> m a
```

In C#:
```csharp
public class Monad<T> {
    // Map method.
    public Monad<TResult> Map<TResult>(Func<T, TResult> selector) {
        throw new NotImplementedException();
    }
}

public static class Monad {
    // Return method.
    public static Monad<T> Return<T>(T value) {
        throw new NotImplementedException();
    }

    // Multiply method.
    public static Monad<T> Multiply<T>(Monad<Monad<T>> square) {
        throw new NotImplementedException();
    }
}
```

### From Triads to Monads

In Haskell:
```haskell
-- Bind defined via Multiply and Map.
m >>= g = join (fmap g m)
```

In C#:
```csharp
public class Monad<T> {
    // Bind defined via Multiply and Map.
    public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> kun) {
        return Monad<TResult>.Multiply(Map(kun));
    }
}
```

### From Monads to Triads

In Haskell:
```haskell
-- Map defined via Return and Bind.
fmap f x = x >>= (return . f)
-- Multiply defined via Bind.
join x = x >>= id
```

In C#:
```csharp
public class Monad<T> {
    // Map defined via Return and Bind.
    public Monad<TResult> Map<TResult>(Func<T, TResult> selector) {
        return Bind(_ => Monad<TResult>.Return(selector(_)));
    }
}

public static class Monad {
    // Multiply defined via Bind.
    public static Monad<T> Multiply(Monad<Monad<T>> square) {
        return square.Bind(_ => _);
    }
}
```

The first functor law can be deduced from the above definition of `Map`
and the second monad law:
```haskell
fmap id x = x >>= (return . id) = x >>= return = x
```

Description                             | Signature
--------------------------------------- | --------------------------------------
`Then` defined by `Bind`                | `m >> n = m >>= \_ -> n`

Description          | Signature
-------------------- | ---------------------------------------------------------
`Then` Associativity | `(m >> n) >> o = m >> (n >> o)`

Comonad
-------

There are two equivalent ways to define a Comonad:

- `Counit`, `Cobind`
- `Counit`, `Map`, `Comultiply`

Richer Monads
-------------

References:

- [MonadPlus Reform](http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal)

We follow (mostly) the proposed new terminology from the MonadPlus Reform.

### MonadPlus

### MonadPlus Reform

### MonadZero

A MonadZero is a monad with a left zero for `Bind`.

### MonadMore

A MonadMore is a monad which is also a monoid and for which `Zero`
is a zero for `Bind`. This is what Haskell calls a MonadPlus.

### MonadPlus

A MonadPlus is a monad which is also a monoid and for which `Bind`
is right distributive over `Plus`.

REVIEW: Haskell uses the term left distributive. Am I missing something?

### MonadOr

A MonadOr is a monad which is also a monoid and for which `Unit` is
a left zero for `Plus`. Here, we prefer to use `OrElse` instead of `Plus` for the
monoid composition operation.

### Summary

Name      | Haskell            | C#
--------- | ------------------ | -------------------------------------------------------------
_Empty_   | `mzero`            | `Empty`, `Zero`, `None`
_Append_  | `mappend`, `mplus` | `Append`, `Plus`, `OrElse`
_Flatten_ | `mconcat`          | `Flatten`

Description                      | Signature
-------------------------------- | ---------------------------------------------
MonadZero Left zero            | `mzero >>= f = mzero`
MonadMore Right zero           | `v >> mzero = mzero`
(TODO)                           | `m >>= (\x -> mzero) = mzero`
MonadPlus Right distributivity | `mplus a b >>= f = mplus (a >>= f) (b >>= f)`
Left distributivity        | (TODO)
MonadOr Left zero              | `morelse (return a) b ≡ return a`
Right zero                 | `morelse a (return b) ≡ return b`

In arithmetic, `Empty` would be `0` and `Append` would be `+`.
For the boolean algebra, `Empty` would be `False` and `Append` would be `∨`,
the logical disjunction OR. The rules should then be familiar to anyone:

Rule           | Translation
-------------- | ---------------------------------------------------------------
Left identity  | `0 + x = x` and `False ∨ P = P`
Right identity | `x + 0 = x` and `P ∨ False = P`
Associativity  | `x + (y + z) = (x + y) + z` and `P ∨ (Q ∨ R) = (P ∨ Q) ∨ R`

Description                      | Signature
-------------------------------- | ---------------------------------------------
MonadZero Left zero            | `0 * x = 0`
MonadMore Right zero           | `x * 0 = 0`
MonadPlus Right distributivity | `(x + y) * z = x * z + x * z`
Left distributivity        | `x * (y + z) = x * y + x * z`
MonadOr Left zero              | (TODO)
Right zero                 | (TODO)

Description                      | Signature
-------------------------------- | ---------------------------------------------
MonadZero Left zero            | `False ∧ P = False`
MonadMore Right zero           | `P ∧ False = False`
MonadPlus Right distributivity | (not available)
Left distributivity        | `P ∧ (Q ∨ R) = (P ∧ Q) ∨ (P ∧ R)`
MonadOr Left zero              | `True ∨ P = True`
Right zero                 | `P ∨ True = True`

Name      | Description
--------- | ------------------------------------------
Monoid    | (Plus, Zero) + Monoid Laws
Monad     | (Bind, Unit) + Monad Laws
Comonad   | (Cobind, Counit) + Comonad Laws
MonadZero | (Monad, Zero) + Zero = left zero for Bind
MonadMore | Monad + Monoid + Zero = zero for Bind
MonadPlus | Monad + Monoid + Right distributivity
MonadOr   | Monad + Monoid + Unit = left zero for Plus

Monads in the .NET Framework
----------------------------

Class            | Type
---------------- | ------------------------
`IEnumerable<T>` | MonadZero, MonadPlus (?)
`Nullable<T>`    | MonadMore, MonadOr (?)
`Func<T>`        | (TODO)
`Lazy<T>`        | Monad, Comonad (?)
`Task<T>`        | Monad, Comonad (?)

Sometimes we choose a more appropriate name than the default one.

We also prefer to use the name expected by the Query Expression Pattern (QEP).
The immediate benefit is that we can use the query expression syntax (from, select, where).
This is similar to the do syntaxic sugar of Haskell.

Monoid

Name               | Haskell           | Terminology used here
-------------------|-------------------|----------------------------------------
`Zero`             | `mzero`           | `Zero` or `None`, `Empty`, `Failure`,...
`Plus`             | `mplus`           | `Plus` or `OrElse`,...

Monad

Name               | Haskell           | Terminology used here
-------------------|-------------------|----------------------------------------
`Unit` (`η`)       | `return`          | `Return` or `Create`, `Success`,...
`Bind`             | `>>=`             | `Bind`
`Map`              | `fmap` or `liftM` | `Map`
`Multiply` (`μ`)   | `join`            | `Flatten`
`Then`             | `>>`              | `Then`
(TODO)             | `fail`            | (TODO)

Comonad

Name               | Haskell           | Terminology used here
-------------------|-------------------|----------------------------------------
`Counit` (`ε`)     | `extract`         | `Extract`
`Cobind`           | `extend`          | `Extend`
`Comultiply` (`δ`) | `duplicate`       | `Duplicate`

### `IEnumerable<T>`

### `Nullable<T>`

### `Func<T>`

### `Lazy<T>`

### `Task<T>`

Reference: [Stephen Toub](http://blogs.msdn.com/b/pfxteam/archive/2013/04/03/tasks-monads-and-linq.aspx)

### `Maybe<T>`

### State

### Write

### Reader

### `Error<T>`

An Haskell to C# Dictionary
---------------------------

All variants that return a `Monad<Unit>` instead of a `Monad<T>` (those that have
a postfix `_`) are not implemented; ignoring the result achieves the same effect.

### Monad

C#                | Haskell
----------------- | -----------------------------------------------
`Monad<T>.Select` | `fmap :: (a -> b) -> m a -> m b`
`Monad<T>.Bind`   | `(>>=) :: forall a b. m a -> (a -> m b) -> m b`
`Monad<T>.Then`   | `(>>) :: forall a b. m a -> m b -> m b`
`Monad.Return`    | `return :: a -> m a`
(1)               | `fail :: String -> m a`

_1._ We do not implement `fail` as .NET has its own way of reporting errors.

### MonadPlus

C#                | Haskell
----------------- | ----------------------------
`Monad<T>.Zero`   | `mzero :: m a`
`Monad<T>.Plus`   | `mplus :: m a -> m a -> m a`

### Basic Monad functions

C#                             | Haskell
------------------------------ | -------------------------------------------------------------------
`Func.Map`                     | `mapM :: Monad m => (a -> m b) -> [a] -> m [b]`
_Ignore_                       | `mapM_ :: Monad m => (a -> m b) -> [a] -> m ()`
`Enumerable<T>.ForEach`        | `forM :: Monad m => [a] -> (a -> m b) -> m [b]`
_Ignore_                       | `forM_ :: Monad m => [a] -> (a -> m b) -> m ()`
`Enumerable<Monad<T>>.Collect` | `sequence :: Monad m => [m a] -> m [a]`
_Ignore_                       | `sequence_ :: Monad m => [m a] -> m ()`
`Func.Invoke`                  | `(=<<) :: Monad m => (a -> m b) -> m a -> m b`
`Func.Compose`                 | `(>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c`
`Func.ComposeBack`             | `(<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c`
 (TODO)                        | `forever :: Monad m => m a -> m b`
 (TODO)                        | `void :: Functor f => f a -> f ()`

### Generalisations of list functions

C#                          | Haskell
--------------------------- | ----------------------------------------------------------------------
`Monad.Flatten`             | `join :: Monad m => m (m a) -> m a`
`Enumerable<Monad<T>>.Sum`  | `msum :: MonadPlus m => [m a] -> m a`
`Monad<T>.Filter`           | `mfilter :: MonadPlus m => (a -> Bool) -> m a -> m a`
`Enumerable<T>.Filter`      | `filterM :: Monad m => (a -> m Bool) -> [a] -> m [a]`
`Enumerable<T>.MapAndUnzip` | `mapAndUnzipM :: Monad m => (a -> m (b, c)) -> [a] -> m ([b], [c])`
`Enumerable<T>.Zip`         | `zipWithM :: Monad m => (a -> b -> m c) -> [a] -> [b] -> m [c]`
_Ignore_                    | `zipWithM_ :: Monad m => (a -> b -> m c) -> [a] -> [b] -> m ()`
`Enumerable<T>.Fold`        | `foldM :: Monad m => (a -> b -> m a) -> a -> [b] -> m a`
_Ignore_                    | `foldM_ :: Monad m => (a -> b -> m a) -> a -> [b] -> m ()`
`Monad<T>.Repeat`           | `replicateM :: Monad m => Int -> m a -> m [a]`
_Ignore_                    | `replicateM_ :: Monad m => Int -> m a -> m ()`

### Conditional execution of monadic expressions

C#                    | Haskell
--------------------- | ----------------------------------------------------------------------------
`Monad.Guard`         | `guard :: MonadPlus m => Bool -> m ()`
(1)                   | `when :: Monad m => Bool -> m () -> m ()`
(1)                   | `unless :: Monad m => Bool -> m () -> m ()`

_1._ `when` and `unless` are related to the way I/O operations are handled in Haskell.

### Monadic lifting operators

Implemented as both static methods (`Monad.Lift`) and extension methods.

C#                    | Haskell
--------------------- | ----------------------------------------------------------------------------
`Monad<T>.Map`        | `liftM :: Monad m => (a1 -> r) -> m a1 -> m r`
`Monad<T>.SelectMany` | `liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r`
`Monad<T>.Zip`        | `liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r`
`Monad<T>.Zip`        | `liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r`
`Monad<T>.Zip`        | `liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r`
(TODO                 | `ap :: Monad m => m (a -> b) -> m a -> m b`

### Extras

C#                    | Haskell (if it existed)
--------------------- | ----------------------------------------------------------------------------
`Monad<T>.Coalesce`   | `coalesce :: Monad m => (a -> Bool) -> m a -> m b -> m b -> m b`
`Monad<T>.Then`       | `then :: MonadPlus m => (a -> Bool) -> m a -> m b -> m b`
`Monad<T>.Otherwise`  | `otherwise :: MonadPlus m => (a -> Bool) -> m a -> m b -> m b`
`Monad<T>.When`       | `when :: Monad m => Bool -> m a -> () -> m a`
`Monad<T>.Unless`     | `unless :: Monad m => Bool -> m a -> () -> m a`
`Monad<T>.Invoke`     | `invoke :: MonadPlus m => m a -> (a -> m ()) -> m () -> m a`
`Monad<T>.OnZero`     | `onzero :: MonadPlus m => m a -> m () -> m ()`
`Monad<T>.Invoke`     | `invoke :: Monad m => m a -> (a -> m ()) -> m ()`

Implementations
---------------

Implementations in .NET:

- [iSynaptic.Commons](https://github.com/iSynaptic/iSynaptic.Commons) in C#
- [SharpMaLib](http://sharpmalib.codeplex.com/) in F#
