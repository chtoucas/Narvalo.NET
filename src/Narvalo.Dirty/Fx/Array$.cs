// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class ArrayExtensions
    {
        public static Maybe<T[]> Where<T>(this T[] @this, Func<T, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");

            T[] list = Array.FindAll(@this, _ => predicateM.Invoke(_).Match(b => b, defaultValue: false));

            return Maybe.Create(list);
        }

        public static TResult[] ConvertAny<TSource, TResult>(
            this TSource[] @this,
            Func<TSource, Maybe<TResult>> converterM)
        {
            Require.Object(@this);
            Require.NotNull(converterM, "converterM");

            // Huf pas très bien écrit tout ça...
            Maybe<TResult>[] options = Array.ConvertAll(@this, _ => converterM.Invoke(_));
            Maybe<TResult>[] values = Array.FindAll(options, _ => _.IsSome);

            int length = values.Length;
            TResult[] list = new TResult[length];
            for (int i = 0; i < length; i++) {
                list[i] = values[i].Value;
            }

            return list;
        }
    }
}
