// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static methods for producing sequences.
    /// </summary>
    public static class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TResult">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{TResult}"/> that contains a single element.</returns>
        public static IEnumerable<TResult> Return<TResult>(TResult value)
        {
            // Enumerable.Repeat(value, 1) works too.
            yield return value;
        }

        /// <summary>
        /// Generates an infinite sequence of one repeated value.
        /// </summary>
        /// <typeparam name="TResult">The type of the value to be used in the
        /// result sequence.</typeparam>
        /// <param name="value">The value to be repeated.</param>
        public static IEnumerable<TResult> Repeat<TResult>(TResult value)
        {
            while (true)
            {
                yield return value;
            }
        }

        public static IEnumerable<TResult> Generate<TResult>(
            TResult seed,
            Func<TResult, TResult> generator)
        {
            Require.NotNull(generator, nameof(generator));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TResult current = seed;

                while (true)
                {
                    yield return current;

                    current = generator(current);
                }
            }
        }

        public static IEnumerable<TResult> Generate<TResult>(
            TResult seed,
            Func<TResult, TResult> generator,
            Func<TResult, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TResult current = seed;

                while (predicate(current))
                {
                    yield return current;

                    current = generator(current);
                }
            }
        }

        public static IEnumerable<TResult> Unfold<TState, TResult>(
            TState seed,
            Func<TState, (TState, TResult)> generator)
        {
            Require.NotNull(generator, nameof(generator));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TState state = seed;
                TResult result;

                while (true)
                {
                    (state, result) = generator(state);

                    yield return result;
                }
            }
        }

        public static IEnumerable<TResult> Unfold<TState, TResult>(
            TState seed,
            Func<TState, (TState, TResult)> generator,
            Func<TState, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TState state = seed;
                TResult result;

                while (predicate(state))
                {
                    (state, result) = generator(state);

                    yield return result;
                }
            }
        }

        /// <remarks>
        /// This method can be derived from:
        /// <code>
        /// Sequence.Unfold(seed, state => (generator(state), resultSelector(state)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Unfold<TState, TResult>(
            TState seed,
            Func<TState, TState> generator,
            Func<TState, TResult> resultSelector)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TState state = seed;

                while (true)
                {
                    yield return resultSelector(state);

                    state = generator(state);
                }
            }
        }

        /// <remarks>
        /// This method can be derived from:
        /// <code>
        /// Sequence.Unfold(seed, state => (generator(state), resultSelector(state)), predicate);
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Unfold<TState, TResult>(
            TState seed,
            Func<TState, TState> generator,
            Func<TState, TResult> resultSelector,
            Func<TState, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TState state = seed;

                while (predicate(state))
                {
                    yield return resultSelector(state);

                    state = generator(state);
                }
            }
        }
    }
}
