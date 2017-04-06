// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Sequence
    {
        /// <summary>
        /// Generates an infinite sequence containing one repeated value.
        /// </summary>
        /// <typeparam name="TSource">The type of the value to be used in the
        /// result sequence.</typeparam>
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
            Func<TSource, TSource> generator)
        {
            Require.NotNull(generator, nameof(generator));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                TSource current = seed;

                while (true)
                {
                    yield return current;

                    current = generator(current);
                }
            }
        }

        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> generator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TSource> iterator()
            {
                TSource current = seed;

                while (predicate(current))
                {
                    yield return current;

                    current = generator(current);
                }
            }
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => (resultSelector(_), generator(_)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> generator,
            Func<TSource, TResult> resultSelector)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TSource current = seed;

                while (true)
                {
                    yield return resultSelector(current);

                    current = generator(current);
                }
            }
        }

        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => (resultSelector(_), generator(_)), predicate);
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> generator,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TSource current = seed;

                while (predicate(current))
                {
                    yield return resultSelector(current);

                    current = generator(current);
                }
            }
        }
    }
}
