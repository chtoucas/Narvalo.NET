// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Linq;
    using Narvalo.Linq;

    public static class MaybeArray
    {
        public static TResult[] SelectAny<TSource, TResult>(
            TSource[] source,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(selectorM, "selectorM");

            Maybe<TResult>[] options = Array.ConvertAll(source, _ => selectorM.Invoke(_));
            Maybe<TResult>[] values = Array.FindAll(options, _ => _.IsSome);

            int length = values.Length;
            TResult[] list = new TResult[length];
            for (int i = 0; i < length; i++) {
                list[i] = values[i].Value;
            }

            return list;
        }

        public static Maybe<T[]> Filter<T>(T[] source, Func<T, Maybe<bool>> predicateM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicateM, "predicateM");

            T[] list = Array.FindAll(
                source,
                _ => predicateM.Invoke(_).Match(b => b, false));

            return Maybe.Create(list);
        }

        public static Maybe<TAccumulate> FoldLeft<TSource, TAccumulate>(
            TSource[] source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TAccumulate> result = Maybe.Create(seed);

            int length = source.Length;

            if (length == 0) { return result; }

            for (int i = 0; i < length; i++) {
                result = result.Bind(_ => accumulatorM.Invoke(_, source[i]));
            }

            return result;
        }

        public static Maybe<TAccumulate> FoldRight<TSource, TAccumulate>(
            TSource[] source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.NotNull(source, "source");

            return FoldLeft(source.Reverse().ToArray(), seed, accumulatorM);
        }

        public static Maybe<TSource> Reduce<TSource>(
            TSource[] source,
            Func<TSource, TSource, Maybe<TSource>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulator");

            int length = source.Length;
            if (length == 0) { return Maybe<TSource>.None; }

            Maybe<TSource> result = Maybe.Create(source[0]);

            for (int i = 1; i < length; i++) {
                result = result.Bind(_ => accumulatorM.Invoke(_, source[i]));
            }

            return result;
        }
    }
}
