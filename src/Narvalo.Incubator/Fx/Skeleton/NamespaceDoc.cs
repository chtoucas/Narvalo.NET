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
     * I might use it in the future as a base for T4 templates.
     * 
     * 
     * Monoid
     * ======
     * 
     * A Monoid has a Zero and a Plus operation that satisfy the Monoid laws:
     * - Identity
     *      Zero.Plus(monad) = monad
     *      monad.Plus(Zero) = monad
     * - Associativity
     *      one.Plus(two.Plus(three)) = (one.Plus(two)).Plus(three)
     *      
     * If we look at Zero as 0 and Plus as +, 
     * - 0 + x = x
     * - x + 0 = x
     * - x + (y + z) = (x + y) + z
     * 
     * If we look at Zero as False and Plus as ∨ (OR), 
     * - False ∨ Q = Q
     * - Q ∨ False = Q
     * - P ∨ (Q ∨ R) = (P ∨ Q) ∨ z
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
     * The Unit and Bind operations must satisfy the three monad laws: left and right identity, and associativity.
     * See MonadLaws.cs for details.
     * 
     * 
     * Aliases
     * -------
     *
     * Name     | Maths | Haskell | Name we choosed to use
     * ---------+-------+---------+------------------------
     * Unit     | η     | return  | Return (NB: Sometimes we prefer to use a more appropriate name, like Create, Success,...)
     * Bind     |       | >>=     | Bind
     * Map      |       | >>      | Map    (NB: The Select of Linq)
     * Multiply | μ     | join    | Join
     *          |       | fail    | Fail
     *          
     * For MonadPlus (see below)       
     *          |       | mplus   | Plus
     *          
     *  
     * Comonad
     * =======
     * 
     * There are two equivalent ways to define a comonad:
     * - Haskell: Counit, Cobind (Extend)
     * - Category: Counit, Map, Comultiply (δ)
     *
     * 
     * Aliases
     * -------
     *
     * Name       | Maths | Haskell   | Name we choosed to use
     * -----------+-------+-----------+------------------------
     * Counit     | ε     | Extract   | Extract
     * Cobind     |       | Extend    | Cobind
     * Map        |       |           | Map
     * Comultiply | δ     | Duplicate | Duplicate
     * 
     * 
     * MonadZero
     * =========
     *      
     * A MonadZero is a Monad with a Zero which statisfies the following rule:
     * - Left Zero 
     *      Zero.Bind(kun) = Zero
     *     
     * If we look at Zero as 0 and Bind as *,
     * - 0 * x = 0
     * If we look at Zero as False and Bind as ∧ (AND),
     * - False ∧ Q = False
     *      
     * 
     * Monad + Monoid
     * ==============
     * 
     * It seems that most Monads which are Monoids are also MonadZero.
     * 
     * 
     * MonadPlus
     * =========
     * 
     * A MonadPlus is a Monad which is a Monoid with one additional rule:
     * - Left Distribution 
     *      one.Plus(second).Bind(kun) = (one.Bind(kun)).Plus(second.Bind(kun))
     *     
     * If we look at Zero as 0, Plus as + and Bind as *,
     * - (x + y) * z = (x * z) + (x * z)
     * 
     * 
     * MonadOr
     * =======
     *  
     * A MonadOr is a Monad which is a Monoid with one additional rule:
     * - Left Catch
     *      Unit(value).Plus(other) = Unit(value)
     * 
     * If we look at Zero as False, Plus as ∨ (OR) and Bind as ∧ (AND),
     * - True ∨ Q = True
     * 
     * 
     * Remarks
     * =======
     * 
     * Haskell calls a MonadPlus a Monad that is a Monoid. We follow the proposed different terminology:
     * - MonadZero has a Zero that is a Left Zero
     * - MonadOr is a Monoid that satisfies Left Catch
     * - MonadPlus is a Monoid that satisfies Left Distribution
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
