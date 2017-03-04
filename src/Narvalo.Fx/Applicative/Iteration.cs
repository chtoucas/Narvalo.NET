// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    public static class Iteration
    {
        public static Iteration<TResult, TSource> Create<TResult, TSource>(TResult result, TSource next)
            => new Iteration<TResult, TSource>(result, next);
    }
}
