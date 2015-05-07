On Monads
=========

You shouldn't be afraid of the monad! We don't need to understand the theory
behind to make good use of it, really. In fact, I guess that the monad theory,
or more precisely category theory, has influenced the design of many parts of
the .NET framework ; LINQ and the Reactive Extensions being the most obvious
proofs of that. The .NET type system is not rich enough to make very general
monadic constructions but it gives developers access to some powerful monadic
concepts in a very friendly way.

Monoid
------

A Monoid has an `Empty` element and an `Append` operation that satisfy
the Monoid laws:

- `Empty` is the identity for `Append`
- `Append` is associative

Haskell also includes a `Concat` operation which in fact derives from `Empty`
and `Append`: `FoldR Append Empty`.

Monad
-----

A Monad has a Unit element and a Bind operation must satisfy the three
monad laws:

- `Unit` is the identity for `Bind`
- `Bind` is associative

If one wishes to stay close to the Category roots of Monads, a Monad is
equivalently defines with a `Unit` element and two operations `Map`
and `Multiply`.

NB: Haskell also provides a fail method that is not part of the standard
definition. It is mostly used for pattern matching failure, something we do not
have in .NET.

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

We provide two analogies to illustrate the rules. Beware they are not accurate,
but they give a fairly simple way to understand the rules.

From the Arithmetic,

- `Bind` is `*`
- `Plus` is `+`
- `Unit` is `1`
- `Zero` is `0`

From the Boolean Algebra,

- `Bind` is `∧`, the logical conjunction AND
- `Plus` is `∨`, the logical disjunction OR
- `Unit` is `True`
- `Zero` is `False`

We write the definitions and rules using the Haskell syntax.
For a translation to .NET, see Internal\Rules.cs

Core definitions:

Description                             | Signature
--------------------------------------- | --------------------------------------
`Bind` defined via `Multiply` and `Map` | `m >>= g = join (fmap g m)`
`Map` defined by `Bind` & `Unit`        | `fmap f x = x >>= (return . f)`
`Multiply` defined by `Bind`            | `join x = x >>= id`
`Then` defined by `Bind`                | `m >> n = m >>= \_ -> n`

Core rules:

Description          | Signature
-------------------- | ---------------------------------------------------------
[Map]                | `fmap id == id`
[Map]                | `fmap (f . g) == fmap f . fmap g`
[Then] Associativity | `(m >> n) >> o = m >> (n >> o)`

### Haskell

Description                      | Signature
-------------------------------- | ---------------------------------------------
[Monoid] Left identity           | `mplus mzero m = m`
[Monoid] Right identity          | `mplus m mzero = m`
[Monoid] Associativity           | `mplus a (mplus b c) = mplus (mplus a b) c`
[Monad] Left identity            | `return x >>= f = f x`
                                 | `return >=> g ≡ g`
[Monad] Right identity           | `m >>= return = m`
                                 | `f >=> return ≡ f`
[Monad] Associativity            | `(m >>= f) >>= g = m >>= (\x -> f x >>= g)`
                                 | `(f >=> g) >=> h ≡ f >=> (g >=> h)`
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
[Monoid] Left identity           | `0 + x = x`
[Monoid] Right identity          | `x + 0 = x`
[Monoid] Associativity           | `x + (y + z) = (x + y) + z`
[Monad] Left identity            | `1 * x = x`
[Monad] Right identity           | `x * 1 = x`
[Monad] Associativity            | `x * (y * z) = (x * y) * z`
[MonadZero] Left zero            | `0 * x = 0`
[MonadMore] Right zero           | `x * 0 = 0`
[MonadPlus] Right distributivity | `(x + y) * z = x * z + x * z`
[...] Left distributivity        | `x * (y + z) = x * y + x * z`
[MonadOr] Left zero              | (not available)
[...] Right zero                 | (not available)

### Boolean Algebra

Description                      | Signature
-------------------------------- | ---------------------------------------------
[Monoid] Left identity           | `False ∨ P = P`
[Monoid] Right identity          | `P ∨ False = P`
[Monoid] Associativity           | `P ∨ (Q ∨ R) = (P ∨ Q) ∨ R`
[Monad] Left identity            | `True ∧ P = P`
[Monad] Right identity           | `P ∧ True = P`
[Monad] Associativity            | `P ∧ (Q ∧ R) = (P ∧ Q) ∧ R`
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

All variants that return a `Monad<Unit>` instead of a `Monad<T>` are not implemented;
they obviously make little sense in the contexte of C#.

### Monad

We do not implement `fail` as .NET provides its own way of reporting errors.

C#                | Haskell
----------------- | -----------------------------------------------
`Monad<T>.Select` | `fmap :: (a -> b) -> m a -> m b`
`Monad<T>.Bind`   | `(>>=) :: forall a b. m a -> (a -> m b) -> m b`
`Monad<T>.Then`   | `(>>) :: forall a b. m a -> m b -> m b`
`Monad.Return`    | `return :: a -> m a`
_Ignore_          | `fail :: String -> m a`

### MonadPlus

C#                | Haskell
----------------- | ----------------------------
`Monad<T>.Zero`   | `mzero :: m a`
`Monad<T>.Plus`   | `mplus :: m a -> m a -> m a`

### Basic Monad functions

We do no

C#                             | Haskell
------------------------------ | -------------------------------------------------------------------
`Enumerable<T>.Map`            | `mapM :: Monad m => (a -> m b) -> [a] -> m [b]`
_Ignore_                       | `mapM_ :: Monad m => (a -> m b) -> [a] -> m ()`
_Ignore_                       | `forM :: Monad m => [a] -> (a -> m b) -> m [b]`
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
`Monad.When`          | `when :: Monad m => Bool -> m () -> m ()`
`Monad.Unless`        | `unless :: Monad m => Bool -> m () -> m ()`

### Monadic lifting operators

Implemented as static methods `Monad.Lift` and extension methods.

C#                    |
--------------------- | ----------------------------------------------------------------------------
`Monad<T>.Map`        | `liftM :: Monad m => (a1 -> r) -> m a1 -> m r`
`Monad<T>.SelectMany` | `liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r`
`Monad<T>.Zip`        | `liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r`
`Monad<T>.Zip`        | `liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r`
`Monad<T>.Zip`        | `liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r`
                      | `ap :: Monad m => m (a -> b) -> m a -> m b`

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

