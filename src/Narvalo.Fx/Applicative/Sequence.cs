// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a set of static methods that produce objects of type <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a single element.</returns>
        public static IEnumerable<TSource> Of<TSource>(TSource value)
        {
            // Enumerable.Repeat(value, 1) works too, but is less readable.
            yield return value;
        }

        #region Unfold (anamorphism)

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, (TResult, TSource)> accumulator)
        {
            Require.NotNull(accumulator, nameof(accumulator));

            return UnfoldIterator(seed, accumulator);
        }

        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, (TResult, TSource)> generator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));

            return UnfoldIterator(seed, generator, predicate);
        }

        private static IEnumerable<TResult> UnfoldIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, (TResult, TSource)> accumulator)
        {
            Debug.Assert(accumulator != null);

            TSource current = seed;

            while (true)
            {
                var (result, next) = accumulator(current);

                yield return result;

                current = next;
            }
        }

        private static IEnumerable<TResult> UnfoldIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, (TResult, TSource)> accumulator,
            Func<TSource, bool> predicate)
        {
            Debug.Assert(accumulator != null);
            Debug.Assert(predicate != null);

            TSource current = seed;

            while (predicate(current))
            {
                var (result, next) = accumulator(current);

                yield return result;

                current = next;
            }
        }

        #endregion

        #region Gather

        /// <summary>
        /// Generates an infinite sequence containing one repeated value.
        /// </summary>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The value to be repeated.</param>
        public static IEnumerable<TSource> Gather<TSource>(TSource value)
        {
            while (true)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Require.NotNull(iterator, nameof(iterator));

            return GatherIterator(seed, iterator);
        }

        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(predicate, nameof(predicate));

            return GatherIterator(seed, iterator, predicate);
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => (resultSelector(_), iterator(_)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return GatherIterator(seed, iterator, resultSelector);
        }

        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => (resultSelector(_), iterator(_)), predicate);
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(predicate, nameof(predicate));

            return GatherIterator(seed, iterator, resultSelector, predicate);
        }

        private static IEnumerable<TSource> GatherIterator<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Debug.Assert(iterator != null);

            TSource current = seed;

            while (true)
            {
                yield return current;

                current = iterator(current);
            }
        }

        private static IEnumerable<TSource> GatherIterator<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Debug.Assert(iterator != null);
            Debug.Assert(predicate != null);

            TSource current = seed;

            while (predicate(current))
            {
                yield return current;

                current = iterator(current);
            }
        }

        private static IEnumerable<TResult> GatherIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Debug.Assert(iterator != null);
            Debug.Assert(resultSelector != null);

            TSource current = seed;

            while (true)
            {
                yield return resultSelector(current);

                current = iterator(current);
            }
        }

        private static IEnumerable<TResult> GatherIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Debug.Assert(iterator != null);
            Debug.Assert(resultSelector != null);
            Debug.Assert(predicate != null);

            TSource current = seed;

            while (predicate(current))
            {
                yield return resultSelector(current);

                current = iterator(current);
            }
        }

        #endregion
    }
}
