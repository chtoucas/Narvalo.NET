// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="System.Nullable{T}"/>.
    /// </summary>
    public static partial class NullableExtensions
    {
        //// ValueOrThrow

        public static T ValueOrThrow<T>(this T? @this, Exception exception)
            where T : struct
        {
            Require.NotNull(exception, "exception");

            return @this.ValueOrThrow(() => exception);
        }

        public static T ValueOrThrow<T>(this T? @this, Func<Exception> exceptionFactory)
            where T : struct
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (!@this.HasValue) {
                throw exceptionFactory.Invoke();
            }

            return @this.Value;
        }

        //// Match

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> selector,
            TResult defaultValue)
            where TSource : struct
            where TResult : struct
        {
            return @this.Map(selector) ?? defaultValue;
        }

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> selector,
            Func<TResult> defaultValueFactory)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.Match(selector, defaultValueFactory.Invoke());
        }

        //// Then

        public static TResult? Then<TSource, TResult>(this TSource? @this, TResult? other)
            where TSource : struct
            where TResult : struct
        {
            return @this.Bind(_ => other);
        }

        //// OnSome

        public static T? OnValue<T>(this T? @this, Action<T> action)
            where T : struct
        {
            Require.NotNull(action, "action");

            if (@this.HasValue) {
                action.Invoke(@this.Value);
            }

            return @this;
        }

        //// OnNone

        public static T? OnNothing<T>(this T? @this, Action action)
            where T : struct
        {
            Require.NotNull(action, "action");

            if (@this.HasValue) {
                action.Invoke();
            }

            return @this;
        }
    }
}
