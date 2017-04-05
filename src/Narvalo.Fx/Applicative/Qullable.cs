// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo;

    public static partial class Qullable
    {
        public static TResult? Bind<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult?> binder)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(binder, nameof(binder));

            return @this is TSource v ? binder(v) : null;
        }
    }

    // Query Expression Pattern for nullables.
    public static partial class Qullable
    {
        public static TSource? Where<TSource>(
            this TSource? @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(predicate, nameof(predicate));

            return @this is TSource v && predicate(v) ? @this : null;
        }

        public static TResult? Select<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, nameof(selector));

            return @this is TSource v ? (TResult?)selector(v) : null;
        }

        public static TResult? SelectMany<TSource, TMiddle, TResult>(
            this TSource? @this,
            Func<TSource, TMiddle?> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this is TSource v && valueSelector(v) is TMiddle m
                ? resultSelector(v, m)
                : (TResult?)null;
        }

        public static TResult? Join<TSource, TInner, TKey, TResult>(
            this TSource? @this,
            TInner? inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
            => Join(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static TResult? Join<TSource, TInner, TKey, TResult>(
            this TSource? @this,
            TInner? inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            var keyLookup = GetKeyLookup(
                inner, outerKeySelector, innerKeySelector, comparer);

            return @this.SelectMany(
                outer => (keyLookup(outer) is TKey key ? inner : null), resultSelector);
        }

        public static TResult? GroupJoin<TSource, TInner, TKey, TResult>(
            this TSource? @this,
            TInner? inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner?, TResult> resultSelector)
            where TSource : struct
            where TInner : struct
            where TResult : struct
            => GroupJoin(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static TResult? GroupJoin<TSource, TInner, TKey, TResult>(
            this TSource? @this,
            TInner? inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner?, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TResult : struct
        {
            throw new NotImplementedException();
        }

        private static Func<TSource, TKey?> GetKeyLookup<TSource, TInner, TKey>(
            TInner? inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
        {
            Debug.Assert(outerKeySelector != null);
            Debug.Assert(innerKeySelector != null);
            Debug.Assert(comparer != null);

            return arg =>
            {
                TKey? innerKey = from v in inner select innerKeySelector(v);
                return from key in innerKey
                       where comparer.Equals(key, outerKeySelector(arg))
                       select key;
            };
        }
    }
}
