// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T, Boolean}"/> and <see cref="Predicate{T}"/>.
    /// </summary>
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidSpeculativeGeneralityRule",
        Justification = "[Intentionally] Delegation is an unavoidable annoyance of fluent interfaces on delegates.")]
    public static class PredicateExtensions
    {
        public static Predicate<TSource> Negate<TSource>(this Predicate<TSource> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Func<TSource, bool>>() != null);

            return _ => !@this.Invoke(_);
        }

        public static Func<TSource, bool> Negate<TSource>(this Func<TSource, bool> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Func<TSource, bool>>() != null);

            return _ => !@this.Invoke(_);
        }
    }
}
