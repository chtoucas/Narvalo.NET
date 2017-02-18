// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;

    using Narvalo;

    public static class Recursion
    {
        public static Func<TSource, TResult> Fix<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            Func<TSource, TResult> g = null;

            return generator.Invoke(_ => g.Invoke(_));
        }
    }
}
