// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Maybe{S}"/>.
    /// </summary>
    public static partial class Maybe
    {
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;
    }

    // Provides extension methods for Maybe<T>.
    public static partial class Maybe
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

        //// https://fsharpforfunandprofit.com/posts/computation-expressions-builder-part6/
        //// https://en.wikibooks.org/wiki/F_Sharp_Programming/Computation_Expressions
        //public static Maybe<TResult> TryWith<TSource, TResult>(
        //    this Maybe<TSource> @this,
        //    Func<TSource, Maybe<TResult>> selector,
        //    Action<Exception> onException)
        //{
        //    Require.NotNull(selector, nameof(selector));
        //    Require.NotNull(onException, nameof(onException));

        //    return @this.Bind(_ =>
        //    {
        //        try
        //        {
        //            return selector.Invoke(_);
        //        }
        //        catch (Exception ex)
        //        {
        //            onException.Invoke(ex);

        //            return Maybe<TResult>.None;
        //        }
        //    });
        //}

        //public static Maybe<TResult> TryFinally<TSource, TResult>(
        //    this Maybe<TSource> @this,
        //    Func<TSource, Maybe<TResult>> selector,
        //    Action action)
        //{
        //    Require.NotNull(selector, nameof(selector));
        //    Require.NotNull(action, nameof(action));

        //    return @this.Bind(_ =>
        //    {
        //        try
        //        {
        //            return selector.Invoke(_);
        //        }
        //        finally
        //        {
        //            action.Invoke();
        //        }
        //    });
        //}

        #region Extension methods when T is a struct

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
            => @this.ValueOrDefault();

        public static T ExtractOrDefault<T>(this Maybe<T?> @this) where T : struct
            => ExtractOrElse(@this, default(T));

        public static T ExtractOrElse<T>(this Maybe<T?> @this, T defaultValue) where T : struct
            => ExtractOrElse(@this, () => defaultValue);

        public static T ExtractOrElse<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.NotNull(defaultValueFactory, nameof(defaultValueFactory));

            return @this.ValueOrDefault() ?? defaultValueFactory.Invoke();
        }

        public static T ExtractOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
            => ExtractOrThrow(@this, () => exception);

        public static T ExtractOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            T? m = @this.ValueOrDefault();

            if (!m.HasValue)
            {
                throw exceptionFactory.Invoke();
            }

            return m.Value;
        }

        #endregion
    }

    // Provides extension methods for IEnumerable<Maybe<T>>.
    public static partial class Maybe
    {
        // Named <c>catMaybes</c> in Haskell parlance.
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }
}
