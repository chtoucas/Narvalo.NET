// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // Adapted/copied from:
    // - https://blogs.msdn.microsoft.com/pfxteam/2013/04/03/tasks-monads-and-linq/
    // - https://blogs.msdn.microsoft.com/pfxteam/2012/08/15/implementing-then-with-await/
    // See also:
    // - https://blogs.msdn.microsoft.com/pfxteam/2010/11/21/processing-sequences-of-asynchronous-operations-with-tasks/
    public static class TaskHelpers
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

        #region Monad extensions

        public static async Task<TResult> ContinueWith<TSource, TResult>(Task<TSource> source, Task<TResult> other)
        {
            // > return await source.Bind(_ => other);

            await source;
            return await other;
        }

        public static async Task<TSource> PassBy<TSource, TOther>(
            Task<TSource> source,
            Task<TOther> other)
        {
            // > return await source.Zip(other, (arg, _) => arg);

            TSource val = await source;
            await other;

            return val;
        }

        public static async Task<TResult> Zip<T1, T2, TResult>(
            Task<T1> first,
            Task<T2> second,
            Func<T1, T2, TResult> zipper)
        {
            // > return await first.Bind(
            // >     arg1 => second.Select(
            // >         arg2 => zipper(arg1, arg2)));

            T1 val1 = await first;
            T2 val2 = await second;

            return zipper(val1, val2);
        }

        #endregion

        #region Query Expresion Pattern

        public static async Task<TResult> Select<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, TResult> selector)
        {
            // > return await @this.Bind(val => Return(selector(val)));

            TSource source = await @this;

            return selector(source);
        }

        public static async Task<TSource> Where<TSource>(
            this Task<TSource> @this,
            Func<TSource, bool> predicate)
        {
            TSource source = await @this;

            if (!predicate(source)) { throw new OperationCanceledException(); }

            return source;
        }

        public static async Task<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Task<TSource> @this,
            Func<TSource, Task<TMiddle>> selector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            TSource source = await @this;
            TMiddle middle = await selector(source);

            return resultSelector(source, middle);
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
            TSource source = await @this;

            return resultSelector(source,
                inner.Where(
                    inval => EqualityComparer<TKey>.Default.Equals(outerKeySelector(source), innerKeySelector(inval))));
        }

        public static async Task<TSource> Cast<TSource>(this Task @this)
        {
            await @this;

            return (TSource)((dynamic)@this).Result;
        }

        #endregion

        #region Then()

        public static async Task Then(this Task source, Action next)
        {
            await source;
            next();
        }

        public static async Task<TResult> Then<TResult>(this Task source, Func<TResult> next)
        {
            await source;
            return next();
        }

        public static async Task Then(this Task source, Func<Task> next)
        {
            await source;
            await next();
        }

        public static async Task<TResult> Then<TResult>(this Task source, Func<Task<TResult>> next)
        {
            await source;
            return await next();
        }

        public static async Task Then<TSource>(this Task<TSource> source, Action<TSource> next)
            => next(await source);

        public static async Task<TResult> Then<TSource, TResult>(this Task<TSource> source, Func<TSource, TResult> next)
            => next(await source);

        public static async Task Then<TSource>(this Task<TSource> source, Func<TSource, Task> next)
            => await next(await source);

        public static async Task<TResult> Then<TSource, TResult>(this Task<TSource> source, Func<TSource, Task<TResult>> next)
            => await next(await source);

        #endregion
    }
}
