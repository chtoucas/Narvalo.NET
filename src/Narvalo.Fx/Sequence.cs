// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    public static class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a single element.</returns>
        public static IEnumerable<TSource> Pure<TSource>(TSource value)
        {
            Warrant.NotNull<IEnumerable<TSource>>();

            // Enumerable.Repeat(value, 1) works too, but is less readable.
            yield return value;
        }

        #region Anamorphisms

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator)
        {
            Require.NotNull(generator, nameof(generator));
            Warrant.NotNull<IEnumerable<TResult>>();

            return UnfoldIterator(seed, generator);
        }

        private static IEnumerable<TResult> UnfoldIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator)
        {
            Demand.NotNull(generator);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (true)
            {
                var iter = generator.Invoke(current);

                yield return iter.Result;

                current = iter.Next;
            }
        }

        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TResult>>();

            return UnfoldIterator(seed, generator, predicate);
        }

        private static IEnumerable<TResult> UnfoldIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator,
            Func<TSource, bool> predicate)
        {
            Demand.NotNull(generator);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                var iter = generator.Invoke(current);

                yield return iter.Result;

                current = iter.Next;
            }
        }

        #endregion

        #region List Comprehensions

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Require.NotNull(iterator, nameof(iterator));
            Warrant.NotNull<IEnumerable<TSource>>();

            return GatherIterator(seed, iterator);
        }

        private static IEnumerable<TSource> GatherIterator<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Demand.NotNull(iterator);
            Warrant.NotNull<IEnumerable<TSource>>();

            TSource current = seed;

            while (true)
            {
                yield return current;

                current = iterator.Invoke(current);
            }
        }

        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return GatherIterator(seed, iterator, predicate);
        }

        private static IEnumerable<TSource> GatherIterator<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Demand.NotNull(iterator);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TSource>>();

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                yield return current;

                current = iterator.Invoke(current);
            }
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => Iteration.Create(resultSelector.Invoke(_), iterator.Invoke(_)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<IEnumerable<TResult>>();

            return GatherIterator(seed, iterator, resultSelector);
        }

        private static IEnumerable<TResult> GatherIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Demand.NotNull(iterator);
            Demand.NotNull(resultSelector);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (true)
            {
                yield return resultSelector.Invoke(current);

                current = iterator.Invoke(current);
            }
        }

        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => Iteration.Create(resultSelector.Invoke(_), iterator.Invoke(_)), predicate);
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
            Warrant.NotNull<IEnumerable<TResult>>();

            return GatherIterator(seed, iterator, resultSelector, predicate);
        }

        private static IEnumerable<TResult> GatherIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Demand.NotNull(iterator);
            Demand.NotNull(resultSelector);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                yield return resultSelector.Invoke(current);

                current = iterator.Invoke(current);
            }
        }

        #endregion
    }
}
