// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class MaybeM
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
    }
}
