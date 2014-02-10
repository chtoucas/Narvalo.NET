// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Fx.Maybe{T}"/>.
    /// </summary>
    public static partial class OutputExtensions
    {
        //// Match

        public static TResult Match<TSource, TResult>(
            this Output<TSource> @this,
            Func<TSource, TResult> selector,
            TResult defaultValue)
        {
            Require.Object(@this);

            return @this.Map(selector).ValueOrElse(defaultValue);
        }

        public static TResult Match<TSource, TResult>(
            this Output<TSource> @this,
            Func<TSource, TResult> selector,
            Func<TResult> defaultValueFactory)
        {
            Require.Object(@this);

            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.Match(selector, defaultValueFactory.Invoke());
        }

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

        //// Coalescing

        public static Output<TResult> Coalesce<TSource, TResult>(
            this Output<TSource> @this,
            Output<TResult> whenSome,
            Output<TResult> whenNone)
        {
            Require.Object(@this);

            return @this.IsSuccess ? whenSome : whenNone;
        }

        public static Output<TResult> Then<TSource, TResult>(this Output<TSource> @this, Output<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }

        //// OnSuccess & OnFailure

        public static Output<TSource> OnSuccess<TSource>(this Output<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsSuccess) {
                action.Invoke(@this.Value);
            }

            return @this;
        }

        public static Output<TSource> OnFailure<TSource>(this Output<TSource> @this, Action<ExceptionDispatchInfo> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsFailure) {
                action.Invoke(@this.ExceptionInfo);
            }

            return @this;
        }
    }
}
