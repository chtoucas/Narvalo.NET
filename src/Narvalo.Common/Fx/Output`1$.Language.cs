// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class OutputExtensions
    {
        //// Zip

        public static Output<TResult> Zip<TFirst, TSecond, TResult>(
            this Output<TFirst> @this,
            Output<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.Bind(firstValue => second.Map(secondValue => resultSelector.Invoke(firstValue, secondValue)));
        }

        //// Run

        public static Output<TSource> Run<TSource>(this Output<TSource> @this, Action<TSource> action)
        {
            return OnSuccess(@this, action);
        }

        //// Then

        public static Output<TResult> Then<TSource, TResult>(this Output<TSource> @this, Output<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }
    }
}
