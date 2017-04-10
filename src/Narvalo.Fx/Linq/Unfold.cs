// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Sequence
    {
        public static IEnumerable<TResult> Unfold<TState, TResult>(
            TState seed,
            Func<TState, (TResult, TState)> generator)
        {
            Require.NotNull(generator, nameof(generator));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TState state = seed;
                TResult result;

                while (true)
                {
                    (result, state) = generator(state);

                    yield return result;
                }
            }
        }

        public static IEnumerable<TResult> Unfold<TState, TResult>(
            TState seed,
            Func<TState, (TResult, TState)> generator,
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
                    (result, state) = generator(state);

                    yield return result;
                }
            }
        }
    }
}
