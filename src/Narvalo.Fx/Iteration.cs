// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static class Iteration
    {
        public static Iteration<TResult, TSource> Create<TResult, TSource>(TResult result, TSource next)
        {
            return new Iteration<TResult, TSource>(result, next);
        }
    }
}
