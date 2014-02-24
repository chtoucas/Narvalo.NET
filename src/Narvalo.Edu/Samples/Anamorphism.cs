// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Collections.Recursion;
    using Narvalo.Fx;

    public static class Anamorphism
    {
        public static IEnumerable<TResult> Ana<TSource, TResult>(
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

        public static IEnumerable<TResult> Ana<TSource, TResult>(
            Func<TSource, Maybe<Iteration<TResult, TSource>>> generator,
            TSource seed)
        {
            TSource next = seed;

            while (true) {
                var iter = generator.Invoke(next);

                if (iter.IsNone) {
                    yield break;
                }

                yield return iter.Value.Result;

                next = iter.Value.Next;
            }
        }

        public static IEnumerable<TResult> Finite<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            return Anamorphism.Ana(
                _ => predicate.Invoke(_)
                    ? Iteration.MayCreate(resultSelector.Invoke(_), iter.Invoke(_))
                    : Maybe<Iteration<TResult, TSource>>.None,
                seed);
        }

        public static IEnumerable<TResult> Infinite<TSource, TResult>(
            Func<TSource, TSource> iter,
            TSource seed,
            Func<TSource, TResult> resultSelector)
        {
            return Anamorphism.Ana(_ => Iteration.Create(resultSelector.Invoke(_), iter.Invoke(_)), seed);
        }
    }
}
