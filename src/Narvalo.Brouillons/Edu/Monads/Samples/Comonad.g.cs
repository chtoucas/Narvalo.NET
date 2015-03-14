﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.34209
// </auto-generated>
//------------------------------------------------------------------------------

using global::System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
    Justification = "This rule is disabled for files generated by a Text Template.")]
[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1403:FileMayOnlyContainASingleNamespace",
    Justification = "This rule is disabled for files generated by a Text Template.")]

[module: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess",
    Justification = "For files generated by Text Template, we favour T4 readibility over StyleCop rules.")]
[module: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1505:OpeningCurlyBracketsMustNotBeFollowedByBlankLine",
    Justification = "For files generated by Text Template, we favour T4 readibility over StyleCop rules.")]
[module: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1507:CodeMustNotContainMultipleBlankLinesInARow",
    Justification = "For files generated by Text Template, we favour T4 readibility over StyleCop rules.")]

[module: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1210:UsingDirectivesMustBeOrderedAlphabeticallyByNamespace",
    Justification = "The directives are correctly ordered in the T4 source file.")]

namespace Narvalo.Edu.Monads.Samples 
{
    using System.Diagnostics.Contracts;

    // Implements core Comonad methods.
    public static partial class Comonad
    {
        /// <remarks>
        /// Named <c>extract</c> in Haskell parlance.
        /// </remarks>
        public static T Extract<T>(Comonad<T> monad)
        {
            Contract.Requires(monad != null);

            return Comonad<T>.ε(monad);
        }

        /// <remarks>
        /// Named <c>duplicate</c> in Haskell parlance.
        /// </remarks>
        public static Comonad<Comonad<T>> Duplicate<T>(Comonad<T> monad)
        {
            return Comonad<T>.δ(monad);
        }
    } // End of the class Comonad.
}
