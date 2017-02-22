// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System.Runtime.CompilerServices;

    /**
     * A word of caution
     * -----------------
     * The classes found here exist for the sole purpose of documentation.
     * API and documentation are adapted from the Haskell sources.
     *
     * Overview
     * --------
     * - Functor
     * - Functor > Applicative
     * - Functor > Applicative > Alternative
     * - Functor > Applicative > Monad
     * - Functor > Applicative > Alternative + Monad > MonadPlus
     *
     * Compiler switches
     * -----------------
     * - MONAD_VIA_MAP_MULTIPLY
     *   The default behaviour is to define Monads via Bind.
     * - COMONAD_VIA_MAP_COMULTIPLY
     *   The default behaviour is to define Comonads via cobind.
     * - APPLICATIVE_USE_GHC_BASE
     */
    [CompilerGenerated]
    internal static class NamespaceDoc { }
}
