// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that depend on the <see cref="Maybe{T}"/> class.
    /// </summary>
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidSpeculativeGeneralityRule",
        Justification = "[Intentionally] Delegation is an unavoidable annoyance of fluent interfaces on delegates.")]
    public static partial class EnumerableMaybeExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            // We could use a LINQ query expression but it then won't be possible 
            // to add the Code Contracts workarounds.
            //  return from _ in @this
            //      let m = funM.Invoke(_)
            //      where m.IsSome
            //      select m.Value;
            return @this
                .Select(_ => funM.Invoke(_)).AssumeNotNull()
                .Where(_ => _.IsSome)
                .Select(_ => _.Value).AssumeNotNull();
        }
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Maybe.g.cs).
    /// </content>
    public static partial class EnumerableMaybeExtensions
    {
        // Custom version of FilterCore with Maybe<T>.
        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return @this
                .Where(_ => predicateM.Invoke(_).ValueOrElse(false))
                .AssumeNotNull();
        }
    }
}
