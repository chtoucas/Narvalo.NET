// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Exploring
    {
        //public static Maybe<TResult> Using<TSource, TResult>(
        //    this Maybe<TSource> @this,
        //    Func<TSource, TResult> selector)
        //    where TResult : IDisposable
        //{
        //    Require.NotNull(selector, nameof(selector));

        //    return @this.Select(_ =>
        //    {
        //        using (var result = selector.Invoke(_))
        //        {
        //            return result;
        //        }
        //    });
        //}

        //public static void While<TSource>(this Maybe<TSource> @this, Func<bool> predicate, Action<TSource> action)
        //{
        //    Require.NotNull(predicate, nameof(predicate));
        //    Require.NotNull(action, nameof(action));

        //    if (@this.IsNone) { return; }

        //    while (predicate.Invoke())
        //    {
        //        action.Invoke(@this.Value);
        //    }
        //}
    }
}
