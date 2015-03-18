// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Internal;

    public static class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a single element.</returns>
        public static IEnumerable<TSource> Single<TSource>(TSource value)
        {
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            // Enumerable.Repeat(value, 1) would work too, but is less readable.
            yield return value;
        }

        #region Anamorphims

        public static IEnumerable<TResult> Create<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            return Create(iter, seed, resultSelector, Stubs<TSource>.AlwaysTrue);
        }

        public static IEnumerable<TResult> Create<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            TSource next = seed;

            while (predicate.Invoke(next))
            {
                yield return resultSelector.Invoke(next);

                next = iter.Invoke(next);
            }
        }

        #endregion

        /// <summary>
        /// Returns an empty sequence that has the specified type argument.
        /// </summary>
        /// <remarks>
        /// <para>Workaround for the fact that <see cref="Enumerable.Empty{T}"/> does not have any contract attached.</para>
        /// <para>The result is immutable.</para>
        /// </remarks>
        /// <typeparam name="TSource">The type to assign to the type parameter of the returned 
        /// generic <see cref="IEnumerable{T}"/>.</typeparam>
        /// <returns>An empty <see cref="IEnumerable{T}"/> whose type argument is TResult.</returns>
        internal static IEnumerable<TSource> Empty<TSource>()
        {
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            // We could use "yield break", but Enumerable.Empty<T> is more readable 
            // with the additional benefit of returning a singleton.
            return Enumerable.Empty<TSource>().AssumeNotNull();
        }
    }
}
