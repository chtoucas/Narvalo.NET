// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Fx
{
    using System;
    using System.Collections.Generic;

    // Linq from scratch.
    public static class Sequence
    {
        public static IEnumerable<TResult> Create<TSource, TResult>(
            Func<TSource, Iteration<TResult, TSource>> generator,
            TSource seed)
        {
            TSource next = seed;

            while (true) {
                var iter = generator.Invoke(next);

                yield return iter.Result;

                next = iter.Next;
            }
        }

        // Correspondence with Narvalo.Fx.Sequence.Create
        public static IEnumerable<TResult> Create<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Create(_ => Iteration.Create(resultSelector.Invoke(_), iter.Invoke(_)), seed);
        }

        ////public static IEnumerable<TResult> Create<TSource, TResult>(
        ////    Func<TSource, Maybe<Iteration<TResult, TSource>>> generator,
        ////    TSource seed)
        ////{
        ////    TSource next = seed;

        ////    while (true) {
        ////        var iter = generator.Invoke(next);

        ////        if (iter.IsNone) {
        ////            yield break;
        ////        }

        ////        yield return iter.Value.Result;

        ////        next = iter.Value.Next;
        ////    }
        ////}

        //// Correspondence with Narvalo.Fx.Sequence.Create
        ////public static IEnumerable<TResult> Create<TSource, TResult>(
        ////    Func<TSource, TSource> iter,
        ////    TSource seed,
        ////    Func<TSource, TResult> resultSelector,
        ////    Func<TSource, bool> predicate)
        ////{
        ////    return Create(
        ////        _ => predicate.Invoke(_)
        ////            ? Iteration.MayCreate(resultSelector.Invoke(_), iter.Invoke(_))
        ////            : Maybe<Iteration<TResult, TSource>>.None,
        ////        seed);
        ////}
    }
}
