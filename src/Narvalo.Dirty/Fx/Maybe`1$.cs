// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using Narvalo.Linq;

    public static class MaybeExtensions
    {
        //// Cast

        public static Maybe<TResult> Cast<TSource, TResult>(this Maybe<TSource> @this) where TSource : TResult
        {
            Require.Object(@this);

            return from _ in @this select (TResult)_;
        }

        public static Maybe<TResult> OfType<TSource, TResult>(this Maybe<TSource> @this)
            where TSource : class
            where TResult : class
        {
            Require.Object(@this);

            return from _ in @this
                   let result = _ as TResult
                   where result != null
                   select result;
        }

        //// TrySet

        public static bool TrySet<T>(this Maybe<T> @this, out T value)
        {
            if (@this.IsSome) {
                value = @this.Value;
                return true;
            }
            else {
                value = default(T);
                return false;
            }
        }
    }
}
