// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Sequence
    {
        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, (TResult, TSource)> generator)
        {
            Require.NotNull(generator, nameof(generator));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TSource current = seed;

                while (true)
                {
                    var (result, next) = generator(current);

                    yield return result;

                    current = next;
                }
            }
        }

        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, (TResult, TSource)> generator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));

            return iterator();

            IEnumerable<TResult> iterator()
            {
                TSource current = seed;

                while (predicate(current))
                {
                    var (result, next) = generator(current);

                    yield return result;

                    current = next;
                }
            }
        }
    }
}
