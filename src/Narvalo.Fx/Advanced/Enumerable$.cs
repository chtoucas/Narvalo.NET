// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    
    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions { }

    /// <content>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that depend on the <see cref="Maybe{T}"/> class.
    /// </content>
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidSpeculativeGeneralityRule",
        Justification = "[Intentionally] Delegation is an unavoidable annoyance of fluent interfaces on delegates.")]
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            return (from _ in @this
                    let m = funM.Invoke(_)
                    where m.IsSome
                    select m.Value).EmptyIfNull();
        }

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
                .EmptyIfNull();
        }
    }

    /// <content>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that depend on the <see cref="Output{T}"/> class.
    /// </content>
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidSpeculativeGeneralityRule",
        Justification = "[Intentionally] Delegation is an unavoidable annoyance of fluent interfaces on delegates.")]
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            return (from _ in @this
                    let m = funM.Invoke(_)
                    where m.IsSuccess
                    select m.ToValue()).EmptyIfNull();
        }
    }
}
