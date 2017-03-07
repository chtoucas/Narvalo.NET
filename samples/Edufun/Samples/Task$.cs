// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // Adapted from https://blogs.msdn.microsoft.com/pfxteam/2013/04/03/tasks-monads-and-linq/
    // See also:
    // - https://blogs.msdn.microsoft.com/pfxteam/2012/08/15/implementing-then-with-await/
    // - https://blogs.msdn.microsoft.com/pfxteam/2010/11/21/processing-sequences-of-asynchronous-operations-with-tasks/
    public static class TaskExtensions
    {
        #region Monad

        internal static Task<T> Return<T>(T value) => Task.FromResult(value);

        public static Task<T> Flatten<T>(Task<Task<T>> square) => square.Result;

        public static async Task<TResult> Bind<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, Task<TResult>> binder)
            => await binder(await @this);

        #endregion

        #region Comonad

        public static Task<TResult> Extend<TSource, TResult>(
            this Task<TSource> @this,
            Func<Task<TSource>, TResult> func)
            => @this.ContinueWith(func);

        public static T Extract<T>(this Task<T> value) => value.Result;

        public static Task<Task<T>> Duplicate<T>(this Task<T> value) => Task.FromResult(value);

        #endregion

        #region Query Expresion Pattern

        public static async Task<TResult> Select<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, TResult> selector)
        {
            TSource val = await @this;

            return selector(val);
        }

        public static async Task<TSource> Where<TSource>(
            this Task<TSource> @this,
            Func<TSource, bool> predicate)
        {
            TSource val = await @this;

            if (!predicate(val)) { throw new OperationCanceledException(); }

            return val;
        }

        public static async Task<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Task<TSource> @this,
            Func<TSource, Task<TMiddle>> selector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            TSource val = await @this;
            TMiddle middle = await selector(val);

            return resultSelector(val, middle);
        }

        public static async Task<TResult> Join<TSource, TInner, TKey, TResult>(
            this Task<TSource> @this,
            Task<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            await Task.WhenAll(@this, inner);

            var outerKey = outerKeySelector(@this.Result);
            var innerKey = innerKeySelector(inner.Result);

            if (!EqualityComparer<TKey>.Default.Equals(outerKey, innerKey))
            {
                throw new OperationCanceledException();
            }

            return resultSelector(@this.Result, inner.Result);
        }

        public static async Task<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Task<TSource> @this,
            Task<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Task<TInner>, TResult> resultSelector)
        {
            TSource val = await @this;

            return resultSelector(val,
                inner.Where(
                    inval => EqualityComparer<TKey>.Default.Equals(outerKeySelector(val), innerKeySelector(inval))));
        }

        //public static async Task<TSource> Cast<TSource>(this Task source)
        //{
        //    await source;

        //    return (TSource)((dynamic)source).Result;
        //}

        #endregion
    }
}
