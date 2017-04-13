// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public static class Recursion
    {
        // Y Combinator.
        public static Func<TSource, TResult> Fix<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            // Initialize.
            Func<TSource, TResult> g = null;
            // Define.
            g = generator(arg => g(arg));

            return g;
        }
    }
}
