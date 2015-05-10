On Monads
=========

Monads have the undeserved reputation of being hard. It is certainly due to the
fact that many people try to explain them using the jargon of category theory.
Truth to be told, you shouldn't be afraid of the monad! We don't need to
understand the theory behind to make good use of it.

The C# type system is not rich enough to make general monadic constructions
but it gives developers access to some powerful monadic concepts in a very
friendly way. In fact, monad theory has clearly influenced the design of some
core parts of the .NET framework; LINQ and the Reactive extensions being the
most obvious proofs of that.

**A way to think about a monad is that it maps a value to a richer structure
which follows a set of simple rules from which we can derive a very rich vocabulary.**

Whenever it is possible, we illustrate the discussion with analogies from
arithmetic, the boolean algebra and LINQ. Beware, these analogies are not always
accurate (most of the time they ain't), but they should give you a feeling
of what's going on.

We also provide examples from Haskell, but if you do not have any knowledge of
it, feel free to skip them.

References:
- The first public discussion of monads in the context of .NET seems to be due to
  [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx)
- A popular explanation of monads by [Eric Lippert](http://ericlippert.com/category/monads/)
- A more abstract one by [Erik Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/)

### Outline

```
Monoid
Functor
    Applicative Functor
Monad
Functions in the Kleisli Category
Triad: Monad Revisited
    From Triads to Monads
    From Monads to Triads
Comonad
Richer Monads
    MonadPlus
    Monad Reform
        MonadZero
        MonadMore
        MonadPlus
        MonadOr
Summary of what we have seen so far
Monads in the .NET Framework
    `IEnumerable<T>`
    `Nullable<T>`
    `Func<T>`
    `Lazy<T>`
    `Task<T>`
LINQ and the Query Expression Syntax
Monad Vocabulary
    Terminology
    An Haskell to C# Dictionary
Maybe Monad
State Monad
Write Monad
Reader Monad
Error Monad
```

Before moving to monads, we explain two preliminary concepts: monoids and functors.
They are not necessary to understand monads but they are sufficiently simple that
I could not resist including them. Better to start slowly.

Monoid
------

Reference: [Data.Monoid](https://hackage.haskell.org/package/base-4.7.0.1/docs/Data-Monoid.html)

A monoid has an `Empty` element and an `Append` operation that satisfy the **monoid laws**:
- `Empty` is an identity for `Append`,
- `Append` is associative.

Since `Append` is not necessary a commutative operation, the first law really
means two things: "`Empty` is a left identity for `Append`" and "`Empty` is a
right identity for `Append`".

In arithmetic, `Empty` would be `0` and `Append` would be `+`.
For the boolean algebra, `Empty` would be `False` and `Append` would be `∨`,
the logical disjunction OR. The rules should then be familiar to anyone:

Rule           | Translation
-------------- | ---------------------------------------------------------------
Left identity  | `0 + x = x` and `False ∨ P = P`
Right identity | `x + 0 = x` and `P ∨ False = P`
Associativity  | `x + (y + z) = (x + y) + z` and `P ∨ (Q ∨ R) = (P ∨ Q) ∨ R`

For LINQ, `Empty` is the empty sequence `Enumerable.Empty<T>()` and `Append`
is the concatenation operation on sequences: `IEnumerable<T>.Concat()`:
```csharp
// First law: Appending a list q to an empty list returns the list q.
// For instance, [] + [1, 2, 3] = [1, 2, 3]
empty.Concat(q) == q;

// Second law: Appending an empty list to a list q returns the list q.
// For instance, [1, 2, 3] + [] = [1, 2, 3]
q.Concat(empty) == q;

// Third law: Appending a list s to a list r then appending the result
// to a third list q is the same as appending the list r to the list q
// then appending the list s to the result.
// For instance, on the LHS we have:
//  [1, 2] + ([3, 4] + [5, 6])
//      -> [1, 2] + [3, 4, 5, 6]
//      -> [1, 2, 3, 4, 5, 6]
// and on the RHS:
//  ([1, 2] + [3, 4]) + [5, 6]
//      -> [1, 2, 3, 4] + [5, 6]
//      -> [1, 2, 3, 4, 5, 6]
q.Concat(r.Concat(s)) == q.Concat(r).Concat(s);
```

In Haskell, things read as follows:
```haskell
-- Empty method.
mempty :: a
-- Append method.
mappend :: a -> a -> a

-- First law: Empty is a left identity for Append.
mappend mempty x = x
-- Second law: Empty is a right identity for Append.
mappend x mempty = x
-- Third law: Append is associative.
mappend x (mappend y z) = mappend (mappend x y) z
```

In C#, it is just a matter of generalizing the LINQ definitions to an
hypothetical `Monoid<T>` class:
```csharp
// Skeleton definition of a monoid.
public class Monoid<T> {
    // Empty property.
    public static Monoid<T> Empty {
        get { throw new NotImplementedException(); }
    }

    // Append method.
    public Monoid<T> Append(Monoid<T> other) {
        throw new NotImplementedException();
    }
}

// Monoid laws.
public static class MonoidLaws {
    // First law: Empty is a left identity for Append.
    public static void FirstLaw<T>(Monoid<T> m) {
        Monoid<T>.Empty.Append(m) == m;
    }

    // Second law: Empty is a right identity for Append.
    public static void SecondLaw<T>(Monoid<T> m) {
        m.Append(Monoid<T>.Empty) == m;
    }

    // Third law: Append is associative.
    public static void ThirdLaw<T>(Monoid<T> a, Monoid<T> b, Monoid<T> c) {
        a.Append(b.Append(c)) == (a.Append(b)).Append(c);
    }
}
```

Haskell also includes a `Concat` operation which derives from `Empty` and `Append`:
```haskell
-- Concat method
mconcat :: [a] -> a
mconcat = foldr mappend mempty
```

Beware this `Concat` is NOT the one from LINQ, it is more a way of flattening a list of lists:
```csharp
// Concat for LINQ.
public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> @this)
{
    Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>> accumulator = (seq1, seq2) => seq1.Concat(seq2);

    return @this.Aggregate(Enumerable.Empty<T>(), accumulator);
}

// Using the query expression syntax, this is even clearer.
public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> @this)
{
    // For instance, [[1, 2], [3, 4]] = [1, 2, 3, 4]
    return from _ in @this
           from item in _
           select item;
}
```

For our hypothetical monoid class, this translates to:
```csharp
public static Monoid<T> Flatten<T>(this IEnumerable<Monoid<T>> @this)
{
    Func<Monoid<T>, Monoid<T>, Monoid<T>> accumulator = (m1, m2) => m1.Append(m2);

    return @this.Aggregate(Monoid<T>.Empty, accumulator);
}
```

Functor
-------

Reference: [Data.Functor](https://hackage.haskell.org/package/base-4.7.0.1/docs/Data-Functor.html)

A functor has a `Map` operation that satisfy the **functor laws**:
- The identity map is a fixed point for `Map`,
- `Map` preserves the composition operator.

For LINQ, `Map` is the select method `IEnumerable<T>.Select()`:
```csharp
// First law: Iterating over list and returning the unmodified items
// is the same as iterating over the list.
// For instance, with id: x -> x,
//  [1, 2] -> [id(1), id(2)] = [1, 2]
q.Select(_ => _) == q;
// or using the query expression syntax:
from _ in q select _ == q;

// Second law: Iterating over a list and returning the result of applying g
// then f to each item is the same as iterating over the list while applying g
// to each item followed by another iteration that returns the results of applying f.
// For instance, with
//  f: x -> -x
//  g: x -> x * x
//  h = f . g: x -> -1 * x * x
// we have on the LHS:
//  [1, 2] -> [h(1), h(2)] = [-1, -4]
// and on the RHS:
//  [1, 2] -> [g(1), g(2)] = [1, 4]
//         -> [f(1), f(4)] = [-1, -4]
q.Select(_ => f(g(_))) == q.Select(g).Select(f);
// or using the query expression syntax:
from _ in q select f(g(_))
    == from item
           in (from _ in q select g(_))
       select f(item);
```

In Haskell, we have:
```haskell
-- Map method.
fmap :: (a -> b) -> f a -> f b

-- First law: The identity map is a fixed point for Map.
fmap id == id
-- Second law: Map preserves the composition operator.
fmap (f . g) == fmap f . fmap g
```

and in C#,
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
    // First law: The identity map is a fixed point for Map.
    public static void FirstLaw<X>(Functor<X> m) {
        m.Map(_ => _) == m;
    }

    // Second law: Map preserves the composition operator.
    public static void SecondLaw<X, Y, Z>(Functor<X> m, Func<Y, Z> f, Func<X, Y> g) {
        m.Map(_ => f(g(_))) == m.Map(g).Map(f);
    }
}
```

### Applicative Functor

Reference: [Control.Applicative](https://hackage.haskell.org/package/base-4.5.0.0/docs/Control-Applicative.html)

In Haskell,
```haskell
-- Pure method.
pure :: a -> f a
-- Gather method.
(<*>) :: f (a -> b) -> f a -> f b

-- First law:
pure id <*> v = v
-- Second law:
pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
-- Third law:
pure f <*> pure x = pure (f x)
-- Fourth law:
u <*> pure y = pure ($ y) <*> u
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
    // we have lifting [1, 2] with [f, g, h] gives:
    //  [1, 2] -> [f(1), f(2), g(1), g(2), h(1), h(2)]
    //              = [1, 2, 2, 4, 3, 6]
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

Monad
-----

References:
- [Haskell](http://www.haskell.org/onlinereport/monad.html)
- [Control.Monad](https://hackage.haskell.org/package/base-4.6.0.1/docs/Control-Monad.html)

A monad has a unit element `Return` and a `Bind` operation that satisfy the **monad laws**:
- `Return` is the identity for `Bind`.
- `Bind` is associative.

Since `Bind` is not necessary a commutative operation, the first law really
means two things: "`Return` is a left identity for `Bind`" and "`Return` is a
right identity for `Bind`".

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
// For instance, with f: x -> [x, 2], on the LHS we have:
//  1 -> [1]
//    -> [f(1)] = [[1, 2]]          (apply f)
//    -> [1, 2]                     (flatten)
// and on the RHS:
//  f(1) = [1, 2]
Sequence.Single(value).Bind(f) == f(value);
// or using the query expression syntax:
from _ in Sequence.Single(value) from item in f(_) select item
    == f(value);

// Second law: Iterating over a list and for each element returning a list containing only
// this element is the same as iterating over the list.
// For instance,
//  [1, 2, 3] -> [[1], [2], [3]]    (apply Single)
//            -> [1, 2, 3]          (flatten)
q.Bind(Sequence.Single) == q;
// or using the query expression syntax:
from _ in q from item in Sequence.Single(_) select item
    == q;

// Third law: Bind is associative.
// For instance, with
//  f: x -> [x, 1]
//  g: x -> [x, 2]
// we have on the LHS:
//  [3, 4] -> [f(3), f(4)]                              (apply f)
//              = [[3, 1], [4, 1]]
//         -> [[g(3), g(1)], [g(4), g(1)]]              (apply g)
//              = [[[3, 2], [1, 2]], [[4, 2], [1, 2]]]
//         -> [[3, 2, 1, 2], [4, 2, 1, 2]]              (flatten the inner seq)
//         -> [3, 2, 1, 2, 4, 2, 1, 2]                  (flatten the outer seq)
// and on the RHS:
//  [3, 4] -> [f(3), f(4)]                              (apply f)
//              = [[3, 1], [4, 1]]
//         -> [3, 1, 4, 1]                              (flatten)
//         -> [g(3), g(1), g(4), g(1))]                 (apply g)
//              = [[3, 2], [1, 2], [4, 2], [1, 2]]
//         -> [3, 2, 1, 2, 4, 2, 1, 2]                  (flatten)
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

        return kReturn.Compose(g).Invoke(value) == g(value);
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
------------------------

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
- [MonadPlus](http://www.haskell.org/haskellwiki/MonadPlus)
- [MonadPlus Reform](http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal)

### MonadPlus

### MonadPlus Reform

We follow (mostly) the proposed new terminology from the MonadPlus Reform.

#### MonadZero

_MonadZero_, a MonadZero is a Monad with a left zero for `Bind`.

#### MonadMore

_MonadMore_, a MonadMore is a Monad which is also a Monoid and for which `Zero`
 is a zero for `Bind`. This is what Haskell calls a MonadPlus.

#### MonadPlus

_MonadPlus_, a MonadPlus is a Monad which is also a Monoid and for which Bind
is right distributive over Plus.

REVIEW: Haskell uses the term left distributive. Am I missing something?

#### MonadOr

_MonadOr_, a MonadOr is a Monad which is also a Monoid and for which Unit is
a left zero for Plus. Here, we prefer to use OrElse instead of Plus for the
Monoid composition operation.

We write the definitions and rules using the Haskell syntax.

### Haskell

Description                      | Signature
-------------------------------- | ---------------------------------------------
[MonadZero] Left zero            | `mzero >>= f = mzero`
[MonadMore] Right zero           | `v >> mzero = mzero`
                                 | `m >>= (\x -> mzero) = mzero`
[MonadPlus] Right distributivity | `mplus a b >>= f = mplus (a >>= f) (b >>= f)`
[...] Left distributivity        |
[MonadOr] Left zero              | `morelse (return a) b ≡ return a`
[...] Right zero                 | `morelse a (return b) ≡ return b`

### Arithmetic

Description                      | Signature
-------------------------------- | ---------------------------------------------
[MonadZero] Left zero            | `0 * x = 0`
[MonadMore] Right zero           | `x * 0 = 0`
[MonadPlus] Right distributivity | `(x + y) * z = x * z + x * z`
[...] Left distributivity        | `x * (y + z) = x * y + x * z`
[MonadOr] Left zero              | (not available)
[...] Right zero                 | (not available)

### Boolean Algebra

Description                      | Signature
-------------------------------- | ---------------------------------------------
[MonadZero] Left zero            | `False ∧ P = False`
[MonadMore] Right zero           | `P ∧ False = False`
[MonadPlus] Right distributivity |
[...] Left distributivity        | `P ∧ (Q ∨ R) = (P ∧ Q) ∨ (P ∧ R)`
[MonadOr] Left zero              | `True ∨ P = True`
[...] Right zero                 | `P ∨ True = True`

Summary
-------

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
`Func<T>`        |
`Lazy<T>`        | Monad, Comonad (?)
`Task<T>`        | Monad, Comonad (?)

### `IEnumerable<T>`

### `Nullable<T>`

### `Func<T>`

### `Lazy<T>`

### `Task<T>`

Reference: [Stephen Toub](http://blogs.msdn.com/b/pfxteam/archive/2013/04/03/tasks-monads-and-linq.aspx)

Monad Vocabulary
----------------

### Terminology

Sometimes we choose a more appropriate name than the default one.

We also prefer to use the name expected by the Query Expression Pattern (QEP).
The immediate benefit is that we can use the query expression syntax (from, select, where).
This is similar to the do syntaxic sugar of Haskell.

Name               | Haskell           | Terminology used here
-------------------|-------------------|----------------------------------------
_Monoid_           |                   |
`Zero`             | `mzero`           | `Zero` or `None`, `Empty`, `Failure`,...
`Plus`             | `mplus`           | `Plus` or `OrElse`,...
_Monad_            |                   |
`Unit` (`η`)       | `return`          | `Return` or `Create`, `Success`,...
`Bind`             | `>>=`             | `Bind`
`Map`              | `fmap` or `liftM` | `Map`
`Multiply` (`μ`)   | `join`            | `Flatten`
`Then`             | `>>`              | `Then`
                   | `fail`            |
_Comonad_          |                   |
`Counit` (`ε`)     | `extract`         | `Extract`
`Cobind`           | `extend`          | `Extend`
`Comultiply` (`δ`) | `duplicate`       | `Duplicate`

### An Haskell to C# Dictionary

All variants that return a `Monad<Unit>` instead of a `Monad<T>` (those that have
a postfix `_`) are not implemented; ignoring the result achieves the same effect.

#### Monad

C#                | Haskell
----------------- | -----------------------------------------------
`Monad<T>.Select` | `fmap :: (a -> b) -> m a -> m b`
`Monad<T>.Bind`   | `(>>=) :: forall a b. m a -> (a -> m b) -> m b`
`Monad<T>.Then`   | `(>>) :: forall a b. m a -> m b -> m b`
`Monad.Return`    | `return :: a -> m a`
(1)               | `fail :: String -> m a`

(1) We do not implement `fail` as .NET has its own way of reporting errors.

#### MonadPlus

C#                | Haskell
----------------- | ----------------------------
`Monad<T>.Zero`   | `mzero :: m a`
`Monad<T>.Plus`   | `mplus :: m a -> m a -> m a`

#### Basic Monad functions

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
                               | `forever :: Monad m => m a -> m b`
                               | `void :: Functor f => f a -> f ()`

#### Generalisations of list functions

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

(1) `when` and `unless` are related to the way I/O operations are handled in Haskell.

#### Monadic lifting operators

Implemented as both static methods (`Monad.Lift`) and extension methods.

C#                    | Haskell
--------------------- | ----------------------------------------------------------------------------
`Monad<T>.Map`        | `liftM :: Monad m => (a1 -> r) -> m a1 -> m r`
`Monad<T>.SelectMany` | `liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r`
`Monad<T>.Zip`        | `liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r`
`Monad<T>.Zip`        | `liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r`
`Monad<T>.Zip`        | `liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r`
                      | `ap :: Monad m => m (a -> b) -> m a -> m b`

#### Extras

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