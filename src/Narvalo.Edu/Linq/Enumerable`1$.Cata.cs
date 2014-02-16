// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using Narvalo.Edu.Fx;

    static partial class EnumerableExtensions
    {
        public static TAccumulate Cata<TSource, TAccumulate>(
            Func<TSource> next,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.NotNull(next, "next");
            Require.NotNull(accumulator, "accumulator");

            TSource value;
            TAccumulate result = seed;

            while ((value = next.Invoke()) != null) {
                result = accumulator.Invoke(result, value);
            }

            return result;
        }

        public static Monad<TAccumulate> Cata<TSource, TAccumulate>(
            Func<TSource> next,
            TAccumulate seed,
            Kunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Require.NotNull(next, "next");
            Require.NotNull(accumulatorM, "accumulatorM");

            TSource value;
            Monad<TAccumulate> result = Monad.Return(seed);

            while ((value = next.Invoke()) != null) {
                result = result.Bind(_ => accumulatorM.Invoke(_, value));
            }

            return result;
        }
    }
}
