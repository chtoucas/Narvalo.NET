// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class Recursion
    {
        public static Func<TSource, TResult> Fix<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> g = null;

            return generator.Invoke(_ => g.Invoke(_));
        }
    }
}
