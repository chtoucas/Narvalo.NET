// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Extensions
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T, Boolean}"/>.
    /// </summary>
    public static class PredicateExtensions
    {
        public static Func<TSource, bool> Negate<TSource>(this Func<TSource, bool> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Func<TSource, bool>>() != null);

            return _ => !@this.Invoke(_);
        }
    }
}
