// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using Narvalo.Fx;

    /// <summary>
    /// Provides limited support for the Query Expression Pattern with <see cref="Narvalo.Fx.Maybe&lt;T&gt;"/>.
    /// </summary>
    public static class MaybeExtensions
    {
        //// Restriction Operators

        public static Maybe<TSource> Where<TSource>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? @this : Maybe<TSource>.None);
        }

        //// Projection Operators

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
        {
            // NB: Added only for completeness but this is not necessary in order to support the QEP.
            return @this.Bind(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Map(m => resultSelector(_, m)));
        }

        //// Conversions Operators

        public static Maybe<TResult> Cast<TSource, TResult>(this Maybe<TSource> @this) where TSource : TResult
        {
            Require.Object(@this);

            return from _ in @this select (TResult)_;
        }
    }
}
