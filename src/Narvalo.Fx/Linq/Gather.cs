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
        /// <typeparam name="TResult">The type of the value to be used in the
        /// result sequence.</typeparam>
        /// <param name="value">The value to be repeated.</param>
        public static IEnumerable<TResult> Gather<TResult>(TResult value)
        {
            while (true)
            {
                yield return value;
            }
        }

        public static IEnumerable<TResult> Gather<TResult>(
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

        public static IEnumerable<TResult> Gather<TResult>(
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

        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, state => (resultSelector(state), generator(state)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TState, TResult>(
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
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, state => (resultSelector(state), generator(state)), predicate);
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TState, TResult>(
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
