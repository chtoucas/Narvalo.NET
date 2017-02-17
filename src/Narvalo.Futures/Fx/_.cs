// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Exploring
    {
        public static VoidOr<TException> TryWith<TException>(Action action) where TException : Exception
            => TryCatch<TException>.With(action);

        public static VoidOr<TException> TryWith<TException>(
            Func<VoidOr<TException>> factory,
            Action<TException> handler)
            where TException : Exception
        {
            Require.NotNull(factory, nameof(factory));
            Require.NotNull(handler, nameof(handler));

            try
            {
                return factory.Invoke();
            }
            catch (TException ex)
            {
                handler.Invoke(ex);

                return VoidOr<TException>.Void;
            }
        }

        public static Maybe<TResult> TryWith<TResult, TException>(
            Func<Maybe<TResult>> factory,
            Func<TException, Maybe<TResult>> handler)
            where TException : Exception
        {
            Require.NotNull(factory, nameof(factory));
            Require.NotNull(handler, nameof(handler));

            try
            {
                return factory.Invoke();
            }
            catch (TException ex)
            {
                return handler.Invoke(ex);
            }
        }

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

        public static Maybe<TResult> TryWith<TSource, TResult, TException>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TResult>> selector,
            Action<TException> onException)
            where TException : Exception
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(onException, nameof(onException));

            try
            {
                return @this.Bind(selector);
            }
            catch (TException ex)
            {
                onException.Invoke(ex);

                return Maybe<TResult>.None;
            }
        }

        public static Maybe<TResult> TryWith<TSource, TResult, TException>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TResult>> selector,
            Func<TException, Maybe<TResult>> onException)
            where TException : Exception
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(onException, nameof(onException));

            try
            {
                return @this.Bind(selector);
            }
            catch (TException ex)
            {
                return onException.Invoke(ex);
            }
        }

        public static Maybe<TResult> TryFinally<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TResult>> selector,
            Action action)
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(action, nameof(action));

            try
            {
                return @this.Bind(selector);
            }
            finally
            {
                action.Invoke();
            }
        }
    }
}
