// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Lazy
    {
        //public static Lazy<T> Create<T>(T value)
        //{
        //    return η(value);
        //}

        public static Lazy<T> Join<T>(Lazy<Lazy<T>> square)
        {
            return μ(square);
        }

        public static Lazy<TResult> Compose<TSource, TMiddle, TResult>(
            Func<TMiddle, Lazy<TResult>> kunB,
            Func<TSource, Lazy<TMiddle>> kunA,
            TSource value)
        {
            return kunA.Invoke(value).Bind(kunB);
        }
    }
}
