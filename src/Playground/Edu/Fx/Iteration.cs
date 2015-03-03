// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Fx
{
    using System;

    using Narvalo.Fx;

    public static class Iteration
    {
        public static Iteration<TResult, TSource> Create<TResult, TSource>(TResult result, TSource source)
        {
            return new Iteration<TResult, TSource>(Tuple.Create(result, source));
        }

        public static Maybe<Iteration<TResult, TSource>> MayCreate<TResult, TSource>(TResult result, TSource source)
        {
            return Maybe.Create(Iteration.Create(result, source));
        }
    }
}
