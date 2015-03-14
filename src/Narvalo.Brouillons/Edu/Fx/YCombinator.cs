﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System;

    internal delegate Func<TSource, TResult> Recursive<TSource, TResult>(Recursive<TSource, TResult> r);

    public static class YCombinator
    {
        public static Func<TSource, TResult> Y0<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Recursive<TSource, TResult> rec = r => _ => generator.Invoke(r.Invoke(r)).Invoke(_);

            return rec.Invoke(rec);
        }

        public static Func<TSource, TResult> Y1<TSource, TResult>(Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> g = null;
            g = _ => generator.Invoke(g).Invoke(_);

            return g;
        }

        public static Func<TSource, TResult> Y2<TSource, TResult>(
            Func<Func<TSource, TResult>, Func<TSource, TResult>> generator)
        {
            Func<TSource, TResult> g = null;

            g = generator.Invoke(_ => g.Invoke(_));

            return g;
        }
    }
}