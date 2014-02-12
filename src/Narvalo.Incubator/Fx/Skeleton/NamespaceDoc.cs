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
     *
     * The classes found here exist for the sole purpose of demonstration.
     * It is NOT meant to be a generic monad implementation.
     * In the future I might use this work as a base for writing T4 templates.
     *
     *
     * A useful picture
     * ----------------
     * 
     * In the remaining of the discussion, we shall use two simple pictures to illustrate the discussion.
     * 
     * From the Arithmetic,
     * - Bind is *
     * - Zero is 0
     * - Plus is +
     * 
     * From the Boolean Algebra,
     * - Bind is ∧, the logical conjunction AND
     * - Zero is False
     * - Unit is True
     * - Plus is ∨, the logical disjunction OR
     * 
     * 
     * Monoid
     * ======
     *
     * A Monoid has a Zero and a Plus operation that satisfy the Monoid laws:
     * - Zero is a left identity for Plus
     *      Zero.Plus(monad) = monad
     * - Zero is a right identity for Plus
     *      monad.Plus(Zero) = monad
     * - Plus is associative
     *      one.Plus(two.Plus(three)) = (one.Plus(two)).Plus(three)
     *
     * Arithmetic correspondence:
     * - 0 + x = x
     * - x + 0 = x
     * - x + (y + z) = (x + y) + z
     *
     * Boolean Algebra correspondence:
     * - False ∨ P = P
     * - P ∨ False = P
     * - P ∨ (Q ∨ R) = (P ∨ Q) ∨ z
     *
     * Terminology
     * -----------
     * 
     * Name     | Haskell | Terminology used here
     * ---------+---------+----------------------
     * Zero     | mzero   | Zero    (NB: Sometimes we prefer to use a more appropriate name, like None, Failure,...)
     * Plus     | mplus   | Compose
     *
     * Signatures
     * ----------
     * static Monoid<T> Zero
     *        Monoid<T> Compose(Monoid<T> other)
     * 
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
     * - Unit is a left and right identity for Bind
     * - Bind is associative
     * 
     * NB: Haskell also defines a fail method that is not part of the standard definition.
     * Here we will provide a similar function but named Otherwise to emphasize its use
     * in pattern matching failure. Contrary to the Haskell convention, the signature of
     * the method shall depend on the particular monad under consideration.
     * 
     * Terminology
     * -----------
     *
     * Name     | Maths | Haskell | Terminology used here
     * ---------+-------+---------+----------------------
     * Unit     | η     | return  | Return    (NB: Sometimes we prefer to use a more appropriate name, like Create, Success,...)
     * Bind     |       | >>=     | Bind
     * Map      |       | >>      | Map       (NB: The Select of Linq)
     * Multiply | μ     | join    | Flatten   (NB: We do not use Join to not conflict with the Linq Join)
     *          |       | fail    | Otherwise
     * 
     * Signatures
     * ----------
     * 
     * static Monad<T>        Return(T value)
     *        Monad<TResult>  Bind<TResult>(Func<T, Monad<TResult>> kun)
     *        Monad<TResult>  Map<TResult>(Func<T, TResult> selector)
     * static Monad<T>        Flatten(Monad<Monad<T>> square)
     * 
     *
     * Comonad
     * =======
     *
     * There are two equivalent ways to define a comonad:
     * - Haskell: Counit, Cobind
     * - Category: Counit, Map, Comultiply
     *
     * Terminology
     * -----------
     *
     * Name       | Maths | Haskell   | Terminology used here
     * -----------+-------+-----------+----------------------
     * Counit     | ε     | Extract   | Extract
     * Cobind     |       | Extend    | Cobind
     * Map        |       |           | Map
     * Comultiply | δ     | Duplicate | Duplicate
     *
     * Signatures
     * ----------
     * 
     * static T                   Extract(Comonad<T> comonad)
     *        Comonad<TResult>    Cobind<TResult>(Func<Comonad<T>, TResult> kun)
     *        Comonad<TResult>    Map<TResult>(Func<T, TResult> selector)
     * static Comonad<Comonad<T>> Duplicate(Comonad<T> comonad)
     * 
     *
     * MonadZero
     * =========
     *
     * A MonadZero is a Monad and Zero is a left zero for Bind:
     *      Zero.Bind(kun) = Zero
     *
     * Arithmetic correspondence:
     *      0 * x = 0
     * Boolean Algebra correspondence:
     *      False ∧ P = False
     *
     *
     * AdditiveMonad
     * ==============
     *
     * An Additive Monad, AdditiveMonad, is a Monad that is also a Monoid.
     * It appears that most Additive Monads are also MonadZero.
     *
     *
     * MonadPlus
     * =========
     *
     * A MonadPlus is an Additive Monad for which Bind is right distributive over Plus:
     *      one.Plus(second).Bind(kun) = one.Bind(kun).Plus(second.Bind(kun))
     *
     * Arithmetic correspondence:
     *      (x + y) * z = (x * z) + (x * z)
     *
     *
     * MonadOr
     * =======
     *
     * A MonadOr is an Additive Monad for which Unit is a left zero for Plus:
     *      Unit(value).Plus(other) = Unit(value)
     *
     * Boolean Algebra correspondence:
     *      True ∨ P = True
     *
     *
     * Summary
     * =======
     *
     * Haskell uses the term MonadPlus for what we call an Additive Monad.
     * We follow (mostly) the proposed new terminology from the MonadPlus Reform.
     * 
     * - Monoid:        Plus + Zero
     * - Monad:         Bind + Unit
     * - Comonad:       Cobind + Counit
     * - MonadZero:     Monad + Zero = Left Zero for Bind
     * - AdditiveMonad: Monad + Monoid
     * - MonadPlus:     AdditiveMonad + Right Distribution
     * - MonadOr:       AdditiveMonad + Unit = Left Zero for Plus
     *
     * Sample monads
     * -------------
     * 
     * + Nullable<T>                MonadOr + MonadZero
     * + Maybe<T>                   MonadOr + MonadZero
     * + Identity<T>                Monad + Comonad
     * + Output<T>                  Monad
     * + Either<TLeft, TRight>      Monad
     * 
     * Other monads:
     * + Func<T>
     * + Lazy<T>
     * + Task<T>
     * + IEnumerable<T>             MonadPlus + MonadZero
     *
     * 
     * References
     * ==========
     *
     * + [MonadPlus]: http://www.haskell.org/haskellwiki/MonadPlus
     * + [MonadPlus Reform]: http://www.haskell.org/haskellwiki/MonadPlus_reform_proposal
     */

    [CompilerGeneratedAttribute]
    class NamespaceDoc
    {
    }
}
