// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

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

        public static IEnumerable<TSource> Generate<TSource>(
            TSource start,
            Func<TSource, TSource> iterator)
        {
            Require.NotNull(iterator, "iterator");
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            TSource current = start;

            while (true)
            {
                yield return current;

                current = iterator.Invoke(current);
            }
        }

        #region Anamorphims

        public static IEnumerable<TResult> Generate<TSource, TResult>(
            Func<TSource, TSource> iterator,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            Contract.Requires(iterator != null);
            Contract.Requires(resultSelector != null);
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            return Generate(iterator, seed, resultSelector, Stubs<TSource>.AlwaysTrue);
        }

        public static IEnumerable<TResult> Generate<TSource, TResult>(
            Func<TSource, TSource> iterator,
            TSource seed,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, "iterator");
            Require.NotNull(resultSelector, "resultSelector");
            Require.NotNull(resultSelector, "predicate");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            TSource next = seed;

            while (predicate.Invoke(next))
            {
                yield return resultSelector.Invoke(next);

                next = iterator.Invoke(next);
            }
        }

        #endregion

        /// <summary>
        /// Returns an empty sequence that has the specified type argument.
        /// </summary>
        /// <remarks>
        /// Workaround for the fact that <see cref="Enumerable.Empty{T}"/> does not have any contract attached.
        /// </remarks>
        /// <typeparam name="TSource">The type to assign to the type parameter of the returned 
        /// generic <see cref="IEnumerable{T}"/>.</typeparam>
        /// <returns>An empty <see cref="IEnumerable{T}"/> whose type argument is TResult.</returns>
        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        internal static IEnumerable<TSource> EmptyInternal<TSource>()
        {
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            // We could use "yield break", but Enumerable.Empty<T> is more readable 
            // with the additional benefit of returning a singleton.
            var retval = Enumerable.Empty<TSource>();

            Contract.Assume(retval != null);

            return retval;
        }
    }
}
