// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System.Runtime.CompilerServices;

    /*!
     * Monad Skeleton
     * ==============
     *
     * A word of caution
     * -----------------
     * The classes found here exist for the sole purpose of demonstration.
     * It is NOT meant to be a generic monad implementation, anyway this is not possible in C#.
     * I did this as a learning exercise, but I might use this work as a base for writing T4 templates.
     *
     * 
     * Compiler switches
     * =================
     * 
     * See sections below for a complete explanation.
     *
     * - MONAD_VIA_MAP_MULTIPLY
     *   The default behaviour is to define Monads via Bind.
     *   
     * - MONAD_DISABLE_ZERO
     * - MONAD_DISABLE_PLUS
     *   A Monad does not necessary have a Zero and a Plus operation.
     *   NB: Disabling Zero automatically disables Plus.
     * 
     * - COMONAD_VIA_MAP_COMULTIPLY
     *   The default behaviour is to define Comonads via cobind.
     * 
     *
     * Monoid
     * ======
     *
     * A Monoid has an Empty element and an Append operation that satisfy the Monoid laws:
     * - Empty is the identity for Append
     * - Append is associative
     * Haskell also includes a Concat operation which in fact derives from Empty and Append:
     *      FoldR Append Empty.
     *
     *
     * Monad
     * =====
     *
     * A Monad has a Unit element and a Bind operation must satisfy the three monad laws:
     * - Unit is the identity for Bind
     * - Bind is associative
     *
     * If one wishes to stay close to the Category roots of Monads, a Monad is equivalently
     * defines with a Unit element and two operations Map and Multiply.
     *
     * NB: Haskell also provides a fail method that is not part of the standard definition.
     * It is mostly used for pattern matching failure, something we do not have in .NET.
     *
     *
     * Comonad
     * =======
     *
     * There are two equivalent ways to define a comonad:
     * - Counit, Cobind
     * - Counit, Map, Comultiply
     *
     *
     * Richer Monads
     * =============
     *
     * We follow (mostly) the proposed new terminology from the MonadPlus Reform.
     *
     * + MonadZero:
     *   A MonadZero is a Monad with a left zero for Bind.
     *
     * + MonadMore:
     *   A MonadMore is a Monad which is also a Monoid and for which Zero is a zero for Bind.
     *   This is what Haskell calls a MonadPlus.
     *
     * + MonadPlus:
     *   A MonadPlus is a Monad which is also a Monoid and for which Bind is right distributive over Plus.
     *   REVIEW: Haskell uses the term left distributive. Am I missing something?
     *
     * + MonadOr:
     *   A MonadOr is a Monad which is also a Monoid and for which Unit is a left zero for Plus.
     *   Here, we prefer to use OrElse instead of Plus for the Monoid composition operation.
     *
     *
     * Summary
     * =======
     *
     * - Monoid                     (Plus, Zero) + Monoid Laws
     * - Monad                      (Bind, Unit) + Monad Laws
     * - Comonad                    (Cobind, Counit) + Comonad Laws
     * - MonadZero                  (Monad, Zero) + Zero = left zero for Bind
     * - MonadMore                  Monad + Monoid + Zero = zero for Bind
     * - MonadPlus                  Monad + Monoid + Right distributivity
     * - MonadOr                    Monad + Monoid + Unit = left zero for Plus
     *
     *
     * Sample monads
     * =============
     *
     * Already found in the Framework:
     * - Nullable<T>                MonadMore, MonadOr (?)
     * - Func<T>
     * - Lazy<T>                    Monad, Comonad (?)
     * - Task<T>                    Monad, Comonad (?)
     * - IEnumerable<T>             MonadZero, MonadPlus (?)
     *
     * Things I am working on:
     * - Identity<T>                Monad, Comonad (?)
     * - Maybe<T>                   MonadMore, MonadOr
     * - Output<T>                  Monad (?)
     * - Either<TLeft, TRight>      Monad (?)
     *
     *
     * Illustration
     * ============
     *
     * We provide two analogies to illustrate the rules. Beware they are not accurate,
     * but they give a fairly simple way to understand the rules.
     *
     * From the Arithmetic,
     * - Bind is *
     * - Plus is +
     * - Unit is 1
     * - Zero is 0
     *
     * From the Boolean Algebra,
     * - Bind is ∧, the logical conjunction AND
     * - Plus is ∨, the logical disjunction OR
     * - Unit is True
     * - Zero is False
     *
     * We write the definitions and rules using the Haskell syntax.
     * For a translation to .NET, see Internal\Rules.cs
     *
     * Core definitions (see Monad`1.cs):
     * - m >>= g = join (fmap g m)                              Bind defined via Multiply and Map
     * - fmap f x = x >>= (return . f)                          Map defined by Bind & Unit
     * - join x = x >>= id                                      Multiply defined by Bind
     * - m >> n = m >>= \_ -> n                                 Then defined by Bind
     *
     * Core rules (see Internal\Rules.cs):
     * - fmap id == id                                          [Map]
     * - fmap (f . g) == fmap f . fmap g                        [Map]
     * - (m >> n) >> o = m >> (n >> o)                          [Then]      Associativity
     *
     * Haskell (see Internal\Rules.cs)
     * ------------------------------------------------
     * - mplus mzero m = m                                      [Monoid]    Left identity
     * - mplus m mzero = m                                      [Monoid]    Right identity
     * - mplus a (mplus b c) = mplus (mplus a b) c              [Monoid]    Associativity
     * - return x >>= f = f x                                   [Monad]     Left identity
     *   return >=> g ≡ g
     * - m >>= return = m                                       [Monad]     Right identity
     *   f >=> return ≡ f
     * - (m >>= f) >>= g = m >>= (\x -> f x >>= g)              [Monad]     Associativity
     *   (f >=> g) >=> h ≡ f >=> (g >=> h)
     * - mzero >>= f = mzero                                    [MonadZero] Left zero
     * - v >> mzero = mzero                                     [MonadMore] Right zero
     * - m >>= (\x -> mzero) = mzero
     * - mplus a b >>= f = mplus (a >>= f) (b >>= f)            [MonadPlus] Right distributivity
     * -                                                        [...]       Left distributivity
     * - morelse (return a) b ≡ return a                        [MonadOr]   Left zero
     * - morelse a (return b) ≡ return b                        [...]       Right zero
     *
     * Arithmetic
     * ----------
     * - 0 + x = x                                              [Monoid]    Left identity
     * - x + 0 = x                                              [Monoid]    Right identity
     * - x + (y + z) = (x + y) + z                              [Monoid]    Associativity
     * - 1 * x = x                                              [Monad]     Left identity
     * - x * 1 = x                                              [Monad]     Right identity
     * - x * (y * z) = (x * y) * z                              [Monad]     Associativity
     * - 0 * x = 0                                              [MonadZero] Left zero
     * - x * 0 = 0                                              [MonadMore] Right zero
     * - (x + y) * z = x * z + x * z                            [MonadPlus] Right distributivity
     * - x * (y + z) = x * y + x * z                            [...]       Left distributivity
     * - (not available)                                        [MonadOr]   Left zero
     * - (not available)                                        [...]       Right zero
     *
     * Boolean Algebra
     * ---------------
     * - False ∨ P = P                                          [Monoid]    Left identity
     * - P ∨ False = P                                          [Monoid]    Right identity
     * - P ∨ (Q ∨ R) = (P ∨ Q) ∨ z                            [Monoid]    Associativity
     * - True ∧ P = P                                           [Monad]     Left identity
     * - P ∧ True = P                                           [Monad]     Right identity
     * - P ∧ (Q ∧ R) = (P ∧ Q) ∧ z                            [Monad]     Associativity
     * - False ∧ P = False                                      [MonadZero] Left zero
     * - P ∧ False = False                                      [MonadMore] Right zero
     * -                                                        [MonadPlus] Right distributivity
     * - P ∧ (Q ∨ R) = (P ∧ Q) ∨ (P ∧ R)                      [...]       Left distributivity
     * - True ∨ P = True                                        [MonadOr]   Left zero
     * - P ∨ True = True                                        [...]       Right zero
     *
     *
     * Implementation
     * ==============
     * 
     * Sometimes we choose a more appropriate name than the default one.
     * 
     * We also prefer to use the name expected by the Query Expression Pattern (QEP).
     * The immediate benefit is that we can use the query expression syntax (from, select, where).
     * This is similar to the do syntaxic sugar of Haskell.
     * 
     * 
     * Name           | Haskell       | Terminology used here
     * ---------------+---------------+------------------------------------
     * ### Monoid
     * ---------------+---------------+------------------------------------
     * Zero           | mzero         | Zero        or None, Empty, Failure,...
     * Plus           | mplus         | Plus        or OrElse,...
     * ---------------+---------------+------------------------------------
     * ### Monad
     * ---------------+---------------+------------------------------------
     * Unit (η)       | return        | Return      or Create, Success,...
     * Bind           | >>=           | Bind
     * Map            | fmap / liftM  | Map
     * Multiply (μ)   | join          | Flatten
     * Then           | >>            | Then
     *                | fail          | -
     * ---------------+---------------+------------------------------------
     * ### Comonad
     * ---------------+---------------+------------------------------------
     * Counit (ε)     | extract       | Extract
     * Cobind         | extend        | Extend
     * Comultiply (δ) | duplicate     | Duplicate
     * ---------------+---------------+------------------------------------
     *              
     *
     * .NET <-> Haskell
     * ================
     *
     * We prefix with a @ the .NET methods provided as extension methods.
     * Lines starting with a ? flag method that I considered optional, either because they're too complicated
     * or they do not really make sense in .NET.
     *
     * Monad
     *      Monad<T>.Map                    fmap :: (a -> b) -> m a -> m b
     *      Monad<T>.Bind                   (>>=) :: forall a b. m a -> (a -> m b) -> m b
     *      Monad<T>.Then                   (>>) :: forall a b. m a -> m b -> m b
     *      Monad.Return                    return :: a -> m a
     *      -                               fail :: String -> m a                               NB: See discussion above.
     *
     * MonadPlus
     *      Monad<T>.Zero                   mzero :: m a
     *      Monad<T>.Plus                   mplus :: m a -> m a -> m a
     *
     * Basic Monad functions
     *      @Enumerable<T>.Map              mapM :: Monad m => (a -> m b) -> [a] -> m [b] 
     *      -                               mapM_ :: Monad m => (a -> m b) -> [a] -> m ()       NB: Same as mapM but returns Monad.Unit
     *      -                               forM :: Monad m => [a] -> (a -> m b) -> m [b]       NB: For us, same as mapM
     *      -                               forM_ :: Monad m => [a] -> (a -> m b) -> m ()       NB: Same as forM but returns Monad.Unit
     *      @Enumerable<Monad<T>>.Collect   sequence :: Monad m => [m a] -> m [a]
     *      -                               sequence_ :: Monad m => [m a] -> m ()               NB: Same as sequence but returns Monad.Unit
     *      @Kunc.Invoke                    (=<<) :: Monad m => (a -> m b) -> m a -> m b
     *      @Kunc.Compose                   (>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c
     *      @Kunc.ComposeBack               (<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c
     *      ??? Not supported               forever :: Monad m => m a -> m b
     *      ??? Not supported               void :: Functor f => f a -> f ()
     *
     * Generalisations of list functions
     *      Monad.Flatten                   join :: Monad m => m (m a) -> m a
     *      @Enumerable<Monad<T>>.Sum       msum :: MonadPlus m => [m a] -> m a
     *      @Monad<T>.Filter                mfilter :: MonadPlus m => (a -> Bool) -> m a -> m a
     *      @Enumerable<T>.Filter           filterM :: Monad m => (a -> m Bool) -> [a] -> m [a]
     * ?    @Enumerable<T>.MapAndUnzip      mapAndUnzipM :: Monad m => (a -> m (b, c)) -> [a] -> m ([b], [c])
     *      @Enumerable<T>.Zip              zipWithM :: Monad m => (a -> b -> m c) -> [a] -> [b] -> m [c]
     *      -                               zipWithM_ :: Monad m => (a -> b -> m c) -> [a] -> [b] -> m ()   NB: Same as zipWithM but returns Monad.Unit            
     *      @Enumerable<T>.Fold             foldM :: Monad m => (a -> b -> m a) -> a -> [b] -> m a
     *      -                               foldM_ :: Monad m => (a -> b -> m a) -> a -> [b] -> m ()        NB: Same as foldM but returns Monad.Unit
     *      @Monad<T>.Repeat                replicateM :: Monad m => Int -> m a -> m [a]
     *      -                               replicateM_ :: Monad m => Int -> m a -> m ()                    NB: Same as replicateM but returns Monad.Unit            
     *
     * Conditional execution of monadic expressions
     * ?    Monad.Guard                     guard :: MonadPlus m => Bool -> m ()
     * ?    Monad.When                      when :: Monad m => Bool -> m () -> m ()
     * ?    Monad.Unless                    unless :: Monad m => Bool -> m () -> m ()
     *
     * Monadic lifting operators
     *      Monad<T>.Select / Monad.Lift    liftM :: Monad m => (a1 -> r) -> m a1 -> m r
     *      @Monad<T>.Zip / Monad.Lift      liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r
     * ?    @Monad<T>.Zip / Monad.Lift      liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r
     * ?    @Monad<T>.Zip / Monad.Lift      liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r
     * ?    @Monad<T>.Zip  / Monad.Lift     liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r
     *      ??? Not supported               ap :: Monad m => m (a -> b) -> m a -> m b
     *
     *
     * References
     * ==========
     *
     * + [Haskell]: http://www.haskell.org/onlinereport/monad.html
     * + [MonadPlus]: http://www.haskell.org/haskellwiki/MonadPlus
     * + [MonadPlus Reform]: http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal
     * + [Control.Monad]: http://hackage.haskell.org/package/base-4.6.0.1/docs/Control-Monad.html
     */

    [CompilerGeneratedAttribute]
    class NamespaceDoc
    {
    }
}
