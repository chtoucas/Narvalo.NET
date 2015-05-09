On Monads
=========

Monads have the undeserved reputation of being hard. It is certainly due to the
fact that many people try to explain them using the jargon of category theory.
Truth to be told, you shouldn't be afraid of the monad! We don't need to
understand the theory behind to make good use of it.

The .NET type system is not rich enough to make general monadic constructions
but it gives developers access to some powerful monadic concepts in a very
friendly way. In fact, monad theory has clearly influenced the design of some
core parts of the .NET framework; LINQ and the Reactive extensions being the
most obvious proofs of that.

A way to think about a monad is that it maps a value into a richer structure
which follows a set of simple rules from which we can derive a very rich vocabulary.

Whenever it is possible, we illustrate the discussion with analogies from
arithmetic, boolean algebra and LINQ. Beware, they are not always accurate,
but they give a fairly easy way to get a sense of the rules.
The first analogy comes from the _Arithmetic_,
- `Bind` is `*`,
- `Plus` is `+`,
- `Unit` is `1`,
- `Zero` is `0`,
(`Bind`, `Plus`, `Unit` and `Zero` will be explained below)
and the second one is the _Boolean Algebra_,
- `Bind` is `∧`, the logical conjunction AND,
- `Plus` is `∨`, the logical disjunction OR,
- `Unit` is `True`,
- `Zero` is `False`.

Before moving to monads, we explain two preliminary concepts: monoids and functors.

Monoid
------

A Monoid has an `Empty` element and an `Append` operation that satisfy the monoid laws:
- `Empty` is the identity for `Append`.
- `Append` is associative.

In the context of monads, we use different names: `Plus` or `OrElse` instead of `Append`
and `Zero` instead of `Empty`.

In arithmetic, `Empty` would be `0` and `Append` would be `+`.

Rule           | Arithmetic
-------------- | ---------------------------------------------------------------
Left identity  | `0 + x = x`
Right identity | `x + 0 = x`
Associativity  | `x + (y + z) = (x + y) + z`

For the boolean algebra, `Empty` would be `False` and `Append` would be `∧`,
the logical conjunction AND.

Rule           | Boolean Algebra
-------------- | ---------------------------------------------------------------
Left identity  | `False ∨ P = P`
Right identity | `P ∨ False = P`
Associativity  | `P ∨ (Q ∨ R) = (P ∨ Q) ∨ R`

For LINQ, `Empty` is the empty sequence `Enumerable.Empty<T>()` and `Append`
is the concatenation of two sequences `IEnumerable<T>.Concat()`.
```csharp
// First law: Empty is a left identity for Append.
empty.Concat(q) == q;
// Second law: Empty is a right identity for Append.
q.Concat(empty) == q;
// Third law: Empty is associative.
x.Concat(y.Concat(z)) == x.Concat(y).Concat(z);
```

In Haskell:
```haskell
-- Empty method.
mempty :: a
-- Append method.
mappend :: a -> a -> a

-- First law: Empty is a left identity for Append.
mappend mempty x = x
-- Second law: Empty is a right identity for Append.
mappend x mempty = x
-- Third law: Empty is associative.
mappend x (mappend y z) = mappend (mappend x y) z
```

In C#,
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
    public static bool ThirdLaw<T>(Monoid<T> a, Monoid<T> b, Monoid<T> c) {
        return a.Append(b.Append(c)) == (a.Append(b)).Append(c);
    }
}
```

Haskell also includes a `Concat` operation which derives from `Empty` and `Append`:
```haskell
-- Concat method
mconcat :: [a] -> a
mconcat = foldr mappend mempty
```

Reference: [Data.Monoid](https://hackage.haskell.org/package/base-4.7.0.1/docs/Data-Monoid.html)

Functor
-------

A Functor has a `Map` operation that satisfy the functor laws:
- The identity map is a fixed point for `Map`.
- `Map` preserves the composition operator.

In Haskell:
```haskell
-- First law: The identity map is a fixed point for `Map`.
fmap id == id
-- Second law: `Map` preserves the composition operator.
fmap (f . g) == fmap f . fmap g
```

In C#:
```csharp
public class Functor<T> {
    // Map method.
    public Functor<TResult> Map<TResult>(Func<T, TResult> selector) {
        throw new NotImplementedException();
    }
}

public static class FunctorLaws {
    // First law: The identity map is a fixed point for `Map`.
    public static void FirstLaw<X>(Functor<X> m) {
        Func<Functor<X>, Functor<X>> id = _ => _;

        m.Map(_ => _) == id.Invoke(m);
    }

    // Second law: `Map` preserves the composition operator.
    public static void SecondLaw<X, Y, Z>(Functor<X> m, Func<Y, Z> f, Func<X, Y> g) {
        m.Map(_ => f(g(_))) == m.Map(g).Map(f);
    }
}
```

Monad
-----

A Monad has a unit element `Return` and a `Bind` operation that satisfy the Monad laws:
- `Return` is the identity for `Bind`.
- `Bind` is associative.

Haskell also provides a `fail` method which is not part of the standard definition
of a monad. It is mostly used for pattern matching failure, something we do not
have in .NET.

Rule           | Arithmetic
-------------- | ---------------------------------------------------------------
Return         | `1`
Bind           | `*`
Left identity  | `1 * x = x`
Right identity | `x * 1 = x`
Associativity  | `x * (y * z) = (x * y) * z`

Rule           | Boolean Algebra
-------------- | ---------------------------------------------------------------
Return         | `∨`, the logical disjunction OR,
Bind           | `True`
Left identity  | `True ∧ P = P`
Right identity | `P ∧ True = P`
Associativity  | `P ∧ (Q ∧ R) = (P ∧ Q) ∧ R`

Rule           | Lists
-------------- | ---------------------------------------------------------------
Return         | `v -> [v]`
Bind           | `xs -> f = append (map f xs)`
Left identity  | `True ∧ P = P`
Right identity | `P ∧ True = P`
Associativity  | `P ∧ (Q ∧ R) = (P ∧ Q) ∧ R`

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
    public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> kun) {
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
    // First law: Unit is a left identity for Bind.
    public static void FirstLaw<X, Y>(Func<X, Monad<Y>> f, X value) {
        Monad.Return(value).Bind(f) == f(value);
    }

    // Second law: Unit is a right identity for Bind.
    public static void SecondLaw<X>(Monad<X> m) {
        m.Bind(Monad.Return) == m;
    }

    // Third law: Bind is associative.
    public static void ThirdLaw<X, Y, Z>(Monad<X> m, Func<X, Monad<Y>> f, Func<Y, Monad<Z>> g) {
        m.Bind(_ => f(_).Bind(g)) == m.Bind(f).Bind(g);
    }
}
```

Functions in the Kleisli category
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

Monad Revisited
---------------

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

### Going from (`Return`, `Map`, `Multiply`) to (`Return`, `Bind`):
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

### Going from (`Return`, `Bind`) to (`Return`, `Map`, `Multiply`):
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
        return Bind(_ => Monad<TResult>.Return(selector.Invoke(_)));
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

We follow (mostly) the proposed new terminology from the MonadPlus Reform.

_MonadZero_, a MonadZero is a Monad with a left zero for `Bind`.

_MonadMore_, a MonadMore is a Monad which is also a Monoid and for which `Zero`
 is a zero for `Bind`. This is what Haskell calls a MonadPlus.

_MonadPlus_, a MonadPlus is a Monad which is also a Monoid and for which Bind
 is right distributive over Plus.
 REVIEW: Haskell uses the term left distributive. Am I missing something?

_MonadOr_, a MonadOr is a Monad which is also a Monoid and for which Unit is
 a left zero for Plus. Here, we prefer to use OrElse instead of Plus for the
 Monoid composition operation.

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

Sample monads
-------------

Already found in the Framework:

Class            | Type
---------------- | ------------------------
`Nullable<T>`    | MonadMore, MonadOr (?)
`Func<T>`        |
`Lazy<T>`        | Monad, Comonad (?)
`Task<T>`        | Monad, Comonad (?)
`IEnumerable<T>` | MonadZero, MonadPlus (?)

Things I am working on:

Class                   | Type
----------------------- | ------------------
`Identity<T>`           | Monad, Comonad
`Maybe<T>`              | MonadMore, MonadOr
`Outcome<T>`            | Monad
`Either<TLeft, TRight>` | Monad

Illustration
------------

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

Naming
------

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

From Haskell to C#
------------------

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

(1) We do not implement `fail` as .NET has its own way of reporting errors.

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
                               | `forever :: Monad m => m a -> m b`
                               | `void :: Functor f => f a -> f ()`

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

(1) `when` and `unless` are related to the way I/O operations are handled in Haskell.

### Monadic lifting operators

Implemented as both static methods (`Monad.Lift`) and extension methods.

C#                    | Haskell
--------------------- | ----------------------------------------------------------------------------
`Monad<T>.Map`        | `liftM :: Monad m => (a1 -> r) -> m a1 -> m r`
`Monad<T>.SelectMany` | `liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r`
`Monad<T>.Zip`        | `liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r`
`Monad<T>.Zip`        | `liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r`
`Monad<T>.Zip`        | `liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r`
                      | `ap :: Monad m => m (a -> b) -> m a -> m b`

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

References
----------

+ [Wes Dyer](http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx)
+ [Lippert](http://ericlippert.com/category/monads/)
+ [Meijer](http://laser.inf.ethz.ch/2012/slides/Meijer/)
+ [Stephen Toub on the Task Comonad](http://blogs.msdn.com/b/pfxteam/archive/2013/04/03/tasks-monads-and-linq.aspx)
+ [Haskell](http://www.haskell.org/onlinereport/monad.html)
+ [MonadPlus](http://www.haskell.org/haskellwiki/MonadPlus)
+ [MonadPlus Reform](http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal)
+ [Control.Monad](http://hackage.haskell.org/package/base-4.6.0.1/docs/Control-Monad.html)

Implementations in .NET:

+ [iSynaptic.Commons](https://github.com/iSynaptic/iSynaptic.Commons) in C#
+ [SharpMaLib](http://sharpmalib.codeplex.com/) in F#

