// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    delegate Func<TSource, TResult> Recursive<TSource, TResult>(Recursive<TSource, TResult> r);

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Combinator")]
    public static class YCombinator
    {
        public static Func<TSource, TResult> Y0<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Recursive<TSource, TResult> rec = r => _ => generator(r(r))(_);
            return rec(rec);
        }

        public static Func<TSource, TResult> Y1<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> g = null;
            g = _ => generator(g)(_);

            return g;

        }
    }
}
