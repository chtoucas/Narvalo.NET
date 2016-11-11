﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.42000
// Microsoft.VisualStudio.TextTemplating: 14.0
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo.Fx.Samples
{
    using static System.Diagnostics.Contracts.Contract;

    // Implements core Comonad methods.
    public static partial class Comonad
    {
        /// <remarks>
        /// Named <c>extract</c> in Haskell parlance.
        /// </remarks>
        public static T Extract<T>(Comonad<T> monad)
            /* T4: C# indent */
        {
            Demand.NotNull(monad);

            return Comonad<T>.ε(monad);
        }

        /// <remarks>
        /// Named <c>duplicate</c> in Haskell parlance.
        /// </remarks>
        public static Comonad<Comonad<T>> Duplicate<T>(Comonad<T> monad)
            /* T4: C# indent */
        {
            Ensures(Result<Comonad<Comonad<T>>>() != null);

            return Comonad<T>.δ(monad);
        }
    } // End of Comonad.
}

