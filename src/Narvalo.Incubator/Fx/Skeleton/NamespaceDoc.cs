// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
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
     * Monoid
     * ======
     *
     * A Monoid has an Empty element and an Append operation that satisfy the Monoid laws:
     * - Empty is the identity for Append
     * - Append is associative
     * Haskell also includes a Concat operation which in fact derives from Empty and Append: FoldR Append Empty.
     * 
     * Monad
     * =====
     *
     * There are two equivalent ways to define a monad:
     * - Haskell: Unit, Bind
     * - Category: Unit, Map, Multiply
     * NB: The constant MONAD_VIA_BIND allows to switch between both approachs.
     *
     * The Unit and Bind operations must satisfy the three monad laws: 
     * - Unit is the identity for Bind
     * - Bind is associative
     * 
     * NB: Haskell also defines a fail method that is not part of the standard definition.
     * Here we will provide a similar function but named Otherwise to emphasize its use
     * in pattern matching failure. Contrary to the Haskell convention, the signature of
     * the method will depend on the particular monad under consideration.
     * 
     *
     * Comonad
     * =======
     *
     * There are two equivalent ways to define a comonad:
     * - Haskell: Counit, Cobind
     * - Category: Counit, Map, Comultiply
     * NB: The constant COMONAD_VIA_COBIND allows to switch between both approachs.
     * 
     *
     * MonadZero
     * =========
     *
     * A MonadZero is a Monad with a left zero for Bind.
     *
     * 
     * Monad + Monoid
     * ==============
     * 
     * We follow (mostly) the proposed new terminology from the MonadPlus Reform.
     * 
     * MonadMore
     * ---------
     * A MonadMore is a Monad which is also a Monoid.
     *
     * 
     * MonadMost
     * ---------
     * A MonadMore for which Zero is a zero for Bind too.
     * This is what Haskell normaly calls a MonadPlus.
     * 
     * MonadPlus
     * ---------
     * A MonadPlus is a MonadMore for which Bind is right distributive over Plus.
     * REVIEW: Haskell uses the term left distributive. Am I missing something?
     * 
     * MonadOr
     * -------
     * A MonadOr is a MonadMore for which Unit is a left zero for Plus, in which case we prefer 
     * to use OrElse instead of Plus.
     *                    
     * 
     * Summary
     * =======
     *
     * - Monoid                     (Plus + Zero)
     * - Monad                      (Bind + Unit)
     * - Comonad                    (Cobind + Counit)
     * - MonadZero                  Monad + Left zero for Bind
     * - MonadMore                  Monad + Monoid
     * - MonadMost                  MonadMore + Zero for Bind 
     * - MonadPlus                  MonadMore + Right distributivity
     * - MonadOr                    MonadMore + Left zero for Plus
     * 
     *
     * Sample monads
     * =============
     * 
     * Already in the Framework:
     * - Nullable<T>                MonadOr + MonadZero
     * - Func<T>
     * - Lazy<T>                    Monad + Comonad (?)       
     * - Task<T>                    Monad + Comonad (?)       
     * - IEnumerable<T>             MonadPlus + MonadZero (?)       
     * 
     * Things I am working on:
     * - Identity<T>                Monad + Comonad
     * - Maybe<T>                   MonadOr + MonadZero
     * - Output<T>                  Monad
     * - Either<TLeft, TRight>      Monad
     * 
     * 
     * Illustration
     * ============
     * 
     * We provide two simple analogies to illustrate the rules.
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
     * .NET
     * ----
     * - Zero.Plus(m) = m                                       [Monoid] Left identity
     * - m.Plus(Zero) = m                                       [Monoid] Right identity
     * - m.Plus(p.Plus(q)) = (m.Plus(p)).Plus(q)                [Monoid] Associativity
     * - Unit(_).Bind(f) = f(_)                                 [Monad] Left identity
     * - m.Bind(Unit) == m                                      [Monad] Right identity
     * - m.Bind(f).Bind(g) == m.Bind(_ => f(_).Bind(g))         [Monad] Associativity
     * - Zero.Bind(f) = Zero                                    [MonadZero] Left zero
     * - m.Bind(_ => Zero) = Zero                               [MonodMost] Right zero
     * - m.Plus(p).Bind(kun) = m.Bind(kun).Plus(p.Bind(kun))    [MonadPlus] Right distributivity
     * -                                                        [...] Left distributivity
     * - Unit(_).Plus(other) = Unit(_)                          [MonadOr] Left zero
     * - other.Plus(Unit(_)) = Unit(_)                          [...] Right zero
     *      
     * Haskell
     * -------
     * - mplus mzero m = m                                      [Monoid] Left identity
     * - mplus m mzero = m                                      [Monoid] Right identity
     * - mplus a (mplus b c) = mplus (mplus a b) c              [Monoid] Associativity
     * - return x >>= f = f x                                   [Monad] Left identity               First Monad law
     *   return >=> g ≡ g                                                                           Kleisli version
     * - m >>= return = m                                       [Monad] Right identity              Second Monad law
     *   f >=> return ≡ f                                                                           Kleisli version
     * - (m >>= f) >>= g = m >>= (\x -> f x >>= g)              [Monad] Associativity               Third Monad law
     *   (f >=> g) >=> h ≡ f >=> (g >=> h)                                                          Kleisli version
     * - mzero >>= f = mzero                                    [MonadZero] Left zero
     * - m >>= (\x -> mzero) = mzero                            [MonadMost] Right zero
     * - mplus a b >>= f = mplus (a >>= f) (b >>= f)            [MonadPlus] Right distributivity
     * -                                                        [...] Left distributivity
     * - morelse (return a) b ≡ return a                        [MonadOr] Left zero
     * - morelse a (return b) ≡ return b                        [...] Right zero
     *      
     * Arithmetic
     * ----------
     * - 0 + x = x                                              [Monoid] Left identity
     * - x + 0 = x                                              [Monoid] Right identity
     * - x + (y + z) = (x + y) + z                              [Monoid] Associativity
     * - 1 * x = x                                              [Monad] Left identity
     * - x * 1 = x                                              [Monad] Right identity
     * - x * (y * z) = (x * y) * z                              [Monad] Associativity
     * - 0 * x = 0                                              [MonadZero] Left zero
     * - x * 0 = 0                                              [MonadMost] Right zero
     * - (x + y) * z = x * z + x * z                            [MonadPlus] Right distributivity
     * - x * (y + z) = x * y + x * z                            [...] Left distributivity
     * - (not available)                                        [MonadOr] Left zero
     * - (not available)                                        [...] Right zero
     * 
     * Boolean Algebra
     * ---------------
     * - False ∨ P = P                                          [Monoid] Left identity
     * - P ∨ False = P                                          [Monoid] Right identity
     * - P ∨ (Q ∨ R) = (P ∨ Q) ∨ z                            [Monoid] Associativity
     * - True ∧ P = P                                           [Monad] Left identity
     * - P ∧ True = P                                           [Monad] Right identity
     * - P ∧ (Q ∧ R) = (P ∧ Q) ∧ z                            [Monad] Associativity
     * - False ∧ P = False                                      [MonadZero] Left zero
     * - P ∧ False = False                                      [MonadMost] Right zero
     * -                                                        [MonadPlus] Right distributivity
     * - P ∧ (Q ∨ R) = (P ∧ Q) ∨ (P ∧ R)                      [...] Left distributivity
     * - True ∨ P = True                                        [MonadOr] Left zero
     * - P ∨ True = True                                        [...] Right zero
     *
     * 
     * Terminology
     * ===========
     * 
     * Name           | Haskell       | Terminology used here
     * ---------------+---------------+------------------------------------
     * Unit (η)       | return        | Return      or Create, Success,...
     * Bind           | >>=           | Bind
     * Map            | >>            | Map
     * Multiply (μ)   | join          | Flatten
     *                | fail          | Otherwise
     * ---------------+---------------+------------------------------------
     * Zero           | mzero         | Zero        or None, Failure,...
     * Plus           | mplus         | Plus        or OrElse,...
     * ---------------+---------------+------------------------------------
     * Counit (ε)     | extract       | Extract
     * Cobind         | extend        | Extend
     * Map            |               | Map
     * Comultiply (δ) | duplicate     | Duplicate
     * 
     * NB:
     * - Sometimes we choose a more appropriate name than the default one
     * - Map is the Select found in Linq
     * 
     * 
     * File Organization
     * =================
     * 
     * Monad
     *      Monad<T>.Bind           (>>=) :: forall a b. m a -> (a -> m b) -> m b
     *      Monad<T>.Map            (>>) :: forall a b. m a -> m b -> m b
     *      Monad.Return            return :: a -> m a
     *      Monad<T>.Otherwise      fail :: String -> m a
     * 
     * MonadPlus
     *      Monad<T>.Zero           mzero :: m a
     *      Monad<T>.Compose        mplus :: m a -> m a -> m a
     *      
     * Basic Monad functions
     *                              mapM :: Monad m => (a -> m b) -> [a] -> m [b]
     *                              mapM_ :: Monad m => (a -> m b) -> [a] -> m ()
     *                              forM :: Monad m => [a] -> (a -> m b) -> m [b]
     *                              forM_ :: Monad m => [a] -> (a -> m b) -> m ()
     *                              sequence :: Monad m => [m a] -> m [a]
     *                              sequence_ :: Monad m => [m a] -> m ()
     *                              (=<<) :: Monad m => (a -> m b) -> m a -> m b
     *      @Kunc<T>.Compose        (>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c
     *      @Kunc<T>.ComposeBack    (<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c
     *                              forever :: Monad m => m a -> m b
     *                              void :: Functor f => f a -> f ()
     *                          
     * Generalisations of list functions
     *      Monad.Flatten           join :: Monad m => m (m a) -> m a
     *                              msum :: MonadPlus m => [m a] -> m a
     *                              mfilter :: MonadPlus m => (a -> Bool) -> m a -> m a
     *                              filterM :: Monad m => (a -> m Bool) -> [a] -> m [a]
     *                              mapAndUnzipM :: Monad m => (a -> m (b, c)) -> [a] -> m ([b], [c])
     *                              zipWithM :: Monad m => (a -> b -> m c) -> [a] -> [b] -> m [c]
     *                              zipWithM_ :: Monad m => (a -> b -> m c) -> [a] -> [b] -> m ()
     *                              foldM :: Monad m => (a -> b -> m a) -> a -> [b] -> m a
     *                              foldM_ :: Monad m => (a -> b -> m a) -> a -> [b] -> m ()
     *                              replicateM :: Monad m => Int -> m a -> m [a]
     *                              replicateM_ :: Monad m => Int -> m a -> m ()
     * 
     * Conditional execution of monadic expressions
     *      @Monad<T>.Guard         guard :: MonadPlus m => Bool -> m ()
     *      @Monad<T>.When          when :: Monad m => Bool -> m () -> m ()
     *      @Monad<T>.Unless        unless :: Monad m => Bool -> m () -> m ()
     *      
     * Monadic lifting operators
     *      @Monad<T>.Zip           liftM :: Monad m => (a1 -> r) -> m a1 -> m r
     *      @Monad<T>.Zip           liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r
     *      @Monad<T>.Zip           liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r
     *      @Monad<T>.Zip           liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r
     *      @Monad<T>.Zip           liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r
     *                              ap :: Monad m => m (a -> b) -> m a -> m b
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
