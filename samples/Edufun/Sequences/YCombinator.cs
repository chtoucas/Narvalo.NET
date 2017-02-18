// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;

    using Narvalo;

    // See also Recursion.Fix
    public static class YCombinator
    {
        public static Func<TSource, TResult> Fix0<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            Recursive<TSource, TResult> rec = r => _ => generator.Invoke(r.Invoke(r)).Invoke(_);

            return rec.Invoke(rec);
        }

        public static Func<TSource, TResult> Fix1<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Require.NotNull(generator, nameof(generator));

            Func<TSource, TResult> g = null;

            return _ => generator.Invoke(g).Invoke(_);
        }
    }
}
