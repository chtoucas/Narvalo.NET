﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.42000
// Microsoft.VisualStudio.TextTemplating: 14.0
// </auto-generated>
//------------------------------------------------------------------------------

using global::Narvalo;
using global::Narvalo.Fx;

namespace Edufun.Haskell.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Edufun.Haskell.Templates.Internal;
    using Edufun.Haskell.Templates.Linq;

    // Provides a set of static methods for MonadPlus<T>.
    public static partial class MonadPlus
    {
        /// <summary>
        /// The unique object of type <c>MonadPlus&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly MonadPlus<global::Narvalo.Fx.Unit> s_Unit = Of(global::Narvalo.Fx.Unit.Single);

        /// <summary>
        /// Gets the unique object of type <c>MonadPlus&lt;Unit&gt;</c>.
        /// </summary>
        public static MonadPlus<global::Narvalo.Fx.Unit> Unit => s_Unit;

        /// <summary>
        /// Gets the zero for <see cref="MonadPlus{T}.Bind"/>.
        /// </summary>
        public static MonadPlus<global::Narvalo.Fx.Unit> Zero => MonadPlus<global::Narvalo.Fx.Unit>.Zero;

        /// <summary>
        /// Obtains an instance of the <see cref="MonadPlus{T}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into an object of type <see cref="MonadPlus{T}"/>.</param>
        /// <returns>An instance of the <see cref="MonadPlus{T}"/> class for the specified value.</returns>
        public static MonadPlus<T> Of<T>(T value)
            /* T4: type constraint */
            => MonadPlus<T>.η(value);

        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        public static MonadPlus<T> Flatten<T>(MonadPlus<MonadPlus<T>> square)
            /* T4: type constraint */
            => MonadPlus<T>.μ(square);

        public static MonadPlus<Unit> Guard(bool predicate) => predicate ? Unit : Zero;

        #region Lift()

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadPlus{T}" /> values.
        /// </summary>
        public static Func<MonadPlus<T>, MonadPlus<TResult>> Lift<T, TResult>(
            Func<T, TResult> func)
            /* T4: type constraint */
            => arg =>
            {
                Require.NotNull(arg, nameof(arg));
                return arg.Select(func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadPlus{T}" /> values.
        /// </summary>
        public static Func<MonadPlus<T1>, MonadPlus<T2>, MonadPlus<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
            /* T4: type constraint */
            => (arg1, arg2) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadPlus{T}" /> values.
        /// </summary>
        public static Func<MonadPlus<T1>, MonadPlus<T2>, MonadPlus<T3>, MonadPlus<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
            /* T4: type constraint */
            => (arg1, arg2, arg3) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, arg3, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadPlus{T}" /> values.
        /// </summary>
        public static Func<MonadPlus<T1>, MonadPlus<T2>, MonadPlus<T3>, MonadPlus<T4>, MonadPlus<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func)
            /* T4: type constraint */
            => (arg1, arg2, arg3, arg4) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, arg3, arg4, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadPlus{T}" /> values.
        /// </summary>
        public static Func<MonadPlus<T1>, MonadPlus<T2>, MonadPlus<T3>, MonadPlus<T4>, MonadPlus<T5>, MonadPlus<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> func)
            /* T4: type constraint */
            => (arg1, arg2, arg3, arg4, arg5) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, arg3, arg4, arg5, func);
            };

        #endregion
    } // End of MonadPlus - T4: EmitMonadCore().

    // Provides extension methods for MonadPlus<T>.
    public static partial class MonadPlus
    {
        public static MonadPlus<TResult> Select<TSource, TResult>(
            this MonadPlus<TSource> @this,
            Func<TSource, TResult> selector)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => MonadPlus.Of(selector(val)));
        }

        public static MonadPlus<TResult> Gather<TSource, TResult>(
            this MonadPlus<TSource> @this,
            MonadPlus<Func<TSource, TResult>> applicative)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(applicative, nameof(applicative));
            return applicative.Bind(func => @this.Select(func));
        }

        public static MonadPlus<TResult> Apply<TSource, TResult>(
            this MonadPlus<Func<TSource, TResult>> @this,
            MonadPlus<TSource> value)
        {
            Require.NotNull(value, nameof(value));
            return value.Gather(@this);
        }

        public static MonadPlus<TResult> ReplaceBy<TSource, TResult>(
            this MonadPlus<TSource> @this,
            TResult value)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            return @this.Select(_ => value);
        }

        public static MonadPlus<TResult> ReplaceBy<TSource, TResult>(
            this MonadPlus<TSource> @this,
            MonadPlus<TResult> other)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            return @this.Bind(_ => other);
        }

        public static MonadPlus<TResult> If<TSource, TResult>(
            this MonadPlus<TSource> @this,
            Func<TSource, bool> predicate,
            MonadPlus<TResult> thenResult)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            return @this.Bind(val => predicate(val) ? thenResult : MonadPlus<TResult>.Zero);
        }

        public static MonadPlus<TResult> Coalesce<TSource, TResult>(
            this MonadPlus<TSource> @this,
            Func<TSource, bool> predicate,
            MonadPlus<TResult> thenResult,
            MonadPlus<TResult> elseResult)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            return @this.Bind(val => predicate(val) ? thenResult : elseResult);
        }

        public static MonadPlus<TSource> Ignore<TSource, TOther>(
            this MonadPlus<TSource> @this,
            MonadPlus<TOther> other)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Func<TSource, TOther, TSource> ignorearg2 = (arg1, _) => arg1;

            return @this.Zip(other, ignorearg2);
        }

        public static MonadPlus<global::Narvalo.Fx.Unit> Skip<TSource>(this MonadPlus<TSource> @this)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            return @this.ReplaceBy(Unit);
        }

        public static MonadPlus<TSource> Where<TSource>(
            this MonadPlus<TSource> @this,
            Func<TSource, bool> predicate)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            return @this.Bind(val => predicate(val) ? MonadPlus.Of(val) : MonadPlus<TSource>.Zero);
        }

        public static MonadPlus<IEnumerable<TSource>> Repeat<TSource>(
            this MonadPlus<TSource> @this,
            int count)
        {
            Require.NotNull(@this, nameof(@this));
            Require.Range(count >= 1, nameof(count));
            return @this.Select(val => Enumerable.Repeat(val, count));
        }

        public static MonadPlus<TResult> Using<TSource, TResult>(
            this MonadPlus<TSource> @this,
            Func<TSource, MonadPlus<TResult>> selector)
            where TSource : IDisposable
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => { using (val) { return selector(val); } });
        }

        public static MonadPlus<TResult> Using<TSource, TResult>(
            this MonadPlus<TSource> @this,
            Func<TSource, TResult> selector)
            where TSource : IDisposable
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            return @this.Select(val => { using (val) { return selector(val); } });
        }

        #region Zip()

        public static MonadPlus<Tuple<TSource, TOther>> Zip<TSource, TOther>(
            this MonadPlus<TSource> @this,
            MonadPlus<TOther> other)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            return @this.Zip(other, Tuple.Create);
        }

        /// <see cref="Lift{T1, T2, T3}" />
        public static MonadPlus<TResult> Zip<TFirst, TSecond, TResult>(
            this MonadPlus<TFirst> @this,
            MonadPlus<TSecond> second,
            Func<TFirst, TSecond, TResult> zipper)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(zipper, nameof(zipper));

            Func<TFirst, Func<TSecond, TResult>> selector
                = arg1 => arg2 => zipper(arg1, arg2);

            return second.Gather(
                @this.Select(selector));
        }

        /// <see cref="Lift{T1, T2, T3, T4}" />
        public static MonadPlus<TResult> Zip<T1, T2, T3, TResult>(
            this MonadPlus<T1> @this,
            MonadPlus<T2> second,
            MonadPlus<T3> third,
            Func<T1, T2, T3, TResult> zipper)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(third, nameof(third));
            Require.NotNull(zipper, nameof(zipper));

            Func<T1, Func<T2, Func<T3, TResult>>> selector
                = arg1 => arg2 => arg3 => zipper(arg1, arg2, arg3);

            return third.Gather(
                second.Gather(
                    @this.Select(selector)));
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5}" />
        public static MonadPlus<TResult> Zip<T1, T2, T3, T4, TResult>(
             this MonadPlus<T1> @this,
             MonadPlus<T2> second,
             MonadPlus<T3> third,
             MonadPlus<T4> fourth,
             Func<T1, T2, T3, T4, TResult> zipper)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(third, nameof(third));
            Require.NotNull(fourth, nameof(fourth));
            Require.NotNull(zipper, nameof(zipper));

            Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> selector
                = arg1 => arg2 => arg3 => arg4 => zipper(arg1, arg2, arg3, arg4);

            return fourth.Gather(
                third.Gather(
                    second.Gather(
                        @this.Select(selector))));
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5, T6}" />
        public static MonadPlus<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this MonadPlus<T1> @this,
            MonadPlus<T2> second,
            MonadPlus<T3> third,
            MonadPlus<T4> fourth,
            MonadPlus<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> zipper)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(third, nameof(third));
            Require.NotNull(fourth, nameof(fourth));
            Require.NotNull(fifth, nameof(fifth));
            Require.NotNull(zipper, nameof(zipper));

            Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> selector
                = arg1 => arg2 => arg3 => arg4 => arg5 => zipper(arg1, arg2, arg3, arg4, arg5);

            return fifth.Gather(
                fourth.Gather(
                    third.Gather(
                        second.Gather(
                            @this.Select(selector)))));
        }

        #endregion

        #region LINQ dialect

        /// <remarks>
        /// Kind of generalisation of <see cref="Zip{T1, T2, T3}" />.
        /// </remarks>
        public static MonadPlus<TResult> SelectMany<TSource, TMiddle, TResult>(
            this MonadPlus<TSource> @this,
            Func<TSource, MonadPlus<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                val => valueSelector(val).Select(
                    middle => resultSelector(val, middle)));
        }

        public static MonadPlus<TResult> Join<TSource, TInner, TKey, TResult>(
            this MonadPlus<TSource> @this,
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
            /* T4: type constraint */
            => JoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static MonadPlus<TResult> Join<TSource, TInner, TKey, TResult>(
            this MonadPlus<TSource> @this,
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: type constraint */
            => JoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);

        public static MonadPlus<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this MonadPlus<TSource> @this,
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadPlus<TInner>, TResult> resultSelector)
            /* T4: type constraint */
            => GroupJoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static MonadPlus<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this MonadPlus<TSource> @this,
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadPlus<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: type constraint */
            => GroupJoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);

        private static MonadPlus<TResult> JoinImpl<TSource, TInner, TKey, TResult>(
            MonadPlus<TSource> outer,
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: type constraint */
        {
            Require.NotNull(outer, nameof(outer));
            Require.NotNull(inner, nameof(inner));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(comparer, nameof(comparer));

            var keyLookup = GetKeyLookup(inner, outerKeySelector, innerKeySelector, comparer);

            return outer.SelectMany(val => keyLookup(val).ReplaceBy(inner), resultSelector);
        }

        private static MonadPlus<TResult> GroupJoinImpl<TSource, TInner, TKey, TResult>(
            MonadPlus<TSource> outer,
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadPlus<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: type constraint */
        {
            Require.NotNull(outer, nameof(outer));
            Require.NotNull(inner, nameof(inner));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(comparer, nameof(comparer));

            var keyLookup = GetKeyLookup(inner, outerKeySelector, innerKeySelector, comparer);

            return outer.Select(val => resultSelector(val, keyLookup(val).ReplaceBy(inner)));
        }

        private static Func<TSource, MonadPlus<TKey>> GetKeyLookup<TSource, TInner, TKey>(
            MonadPlus<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
            /* T4: type constraint */
        {
            Demand.NotNull("inner");
            Demand.NotNull(outerKeySelector);
            Demand.NotNull(innerKeySelector);
            Demand.NotNull(comparer);

            return arg =>
            {
                TKey outerKey = outerKeySelector(arg);

                return inner.Select(innerKeySelector)
                    .Where(key => comparer.Equals(key, outerKey));
            };
        }

        #endregion
    } // End of MonadPlus - T4: EmitMonadExtensions().

    // Provides extension methods for Func<T> in the Kleisli category.
    public static partial class Kleisli
    {
        public static MonadPlus<IEnumerable<TResult>> InvokeForEach<TSource, TResult>(
            this Func<TSource, MonadPlus<TResult>> @this,
            IEnumerable<TSource> seq)
            => seq.SelectWith(@this);

        public static MonadPlus<TResult> InvokeWith<TSource, TResult>(
            this Func<TSource, MonadPlus<TResult>> @this,
            MonadPlus<TSource> value)
            /* T4: type constraint */
        {
            Require.NotNull(value, nameof(value));
            return value.Bind(@this);
        }

        public static Func<TSource, MonadPlus<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, MonadPlus<TMiddle>> first,
            Func<TMiddle, MonadPlus<TResult>> second)
            /* T4: type constraint */
        {
            Require.NotNull(first, nameof(first));
            return arg => first(arg).Bind(second);
        }

        public static Func<TSource, MonadPlus<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, MonadPlus<TResult>> first,
            Func<TSource, MonadPlus<TMiddle>> second)
            /* T4: type constraint */
        {
            Require.NotNull(second, nameof(second));
            return arg => second(arg).Bind(first);
        }
    } // End of Kleisli - T4: EmitKleisliExtensions().

    // Provides extension methods for IEnumerable<MonadPlus<T>>.
    public static partial class MonadPlus
    {
        public static MonadPlus<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<MonadPlus<TSource>> @this)
            => @this.CollectImpl();

        public static MonadPlus<TSource> Sum<TSource>(
            this IEnumerable<MonadPlus<TSource>> @this)
            /* T4: type constraint */
            => @this.SumImpl();
    } // End of Sequence - T4: EmitMonadEnumerableExtensions().
}

namespace Edufun.Haskell.Templates.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx.Linq;

    // Provides default implementations for the extension methods for IEnumerable<MonadPlus<T>>.
    // You will certainly want to override them to improve performance.
    internal static partial class EnumerableExtensions
    {
        internal static MonadPlus<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<MonadPlus<TSource>> @this)
        {
            Demand.NotNull(@this);

            var seed = MonadPlus.Of(Enumerable.Empty<TSource>());
            Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>> append = (seq, item) => seq.Append(item);

            return @this.Aggregate(seed, MonadPlus.Lift<IEnumerable<TSource>, TSource, IEnumerable<TSource>>(append));
        }

        internal static MonadPlus<TSource> SumImpl<TSource>(
            this IEnumerable<MonadPlus<TSource>> @this)
            /* T4: type constraint */
        {
            Demand.NotNull(@this);

            return @this.Aggregate(MonadPlus<TSource>.Zero, (m, n) => m.Plus(n));
        }
    } // End of EnumerableExtensions - T4: EmitMonadEnumerableInternalExtensions().
}

namespace Edufun.Haskell.Templates.Linq
{
    using System;
    using System.Collections.Generic;

    using Edufun.Haskell.Templates;
    using Edufun.Haskell.Templates.Internal;

    // Provides extension methods for IEnumerable<T>.
    // We do not use the standard LINQ names to avoid a confusing API.
    // - Select    -> SelectWith
    // - Where     -> WhereBy
    // - Zip       -> ZipWith
    // - Aggregate -> Reduce or Fold
    public static partial class Qperators
    {
        public static MonadPlus<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, MonadPlus<TResult>> selector)
            => @this.SelectWithImpl(selector);

        public static MonadPlus<IEnumerable<TSource>> WhereBy<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, MonadPlus<bool>> predicate)
            => @this.WhereByImpl(predicate);

        public static MonadPlus<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            SelectUnzip<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, MonadPlus<Tuple<TFirst, TSecond>>> selector)
            => @this.SelectUnzipImpl(selector);

        public static MonadPlus<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, MonadPlus<TResult>> resultSelector)
            => @this.ZipWithImpl(second, resultSelector);

        public static MonadPlus<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadPlus<TAccumulate>> accumulator)
            /* T4: type constraint */
            => @this.FoldImpl(seed, accumulator);

        public static MonadPlus<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadPlus<TAccumulate>> accumulator,
            Func<MonadPlus<TAccumulate>, bool> predicate)
            /* T4: type constraint */
            => @this.FoldImpl(seed, accumulator, predicate);

        public static MonadPlus<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadPlus<TSource>> accumulator)
            /* T4: type constraint */
            => @this.ReduceImpl(accumulator);

        public static MonadPlus<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadPlus<TSource>> accumulator,
            Func<MonadPlus<TSource>, bool> predicate)
            /* T4: type constraint */
            => @this.ReduceImpl(accumulator, predicate);
    } // End of Iterable - T4: EmitEnumerableExtensions().
}

namespace Edufun.Haskell.Templates.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Edufun.Haskell.Templates.Linq;
    using Narvalo.Fx.Linq;

    // Provides default implementations for the extension methods for IEnumerable<T>.
    // You will certainly want to override them to improve performance.
    internal static partial class EnumerableExtensions
    {
        internal static MonadPlus<IEnumerable<TResult>> SelectWithImpl<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, MonadPlus<TResult>> selector)
        {
            Demand.NotNull(@this);
            Demand.NotNull(selector);

            return @this.Select(selector).Collect();
        }

        internal static MonadPlus<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, MonadPlus<bool>> predicate)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            Func<TSource, Func<bool, IEnumerable<TSource>, IEnumerable<TSource>>> func
                = item => (flg, seq) => flg ? seq.Append(item) : seq;

            Func<MonadPlus<IEnumerable<TSource>>, TSource, MonadPlus<IEnumerable<TSource>>> accumulator
                = (mseq, item) => predicate(item).Zip(mseq, func(item));

            return @this.Aggregate(MonadPlus.Of(Enumerable.Empty<TSource>()), accumulator);
        }

        internal static MonadPlus<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            SelectUnzipImpl<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, MonadPlus<Tuple<TFirst, TSecond>>> selector)
        {
            Demand.NotNull(@this);
            Demand.NotNull(selector);

            return @this.SelectWith(selector).Select(
                tuples =>
                {
                    var seq1 = tuples.Select(_ => _.Item1);
                    var seq2 = tuples.Select(_ => _.Item2);

                    return new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(seq1, seq2);
                });
        }

        internal static MonadPlus<IEnumerable<TResult>> ZipWithImpl<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, MonadPlus<TResult>> resultSelector)
        {
            Demand.NotNull(resultSelector);
            Demand.NotNull(@this);
            Demand.NotNull(second);

            return @this.Zip(second, resultSelector).Collect();
        }

        internal static MonadPlus<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadPlus<TAccumulate>> accumulator)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));

            Func<MonadPlus<TAccumulate>, TSource, MonadPlus<TAccumulate>> func
                = (arg1, arg2) => arg1.Bind(arg => accumulator(arg, arg2));

            return @this.Aggregate(MonadPlus.Of(seed), func);
        }

        internal static MonadPlus<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadPlus<TAccumulate>> accumulator,
            Func<MonadPlus<TAccumulate>, bool> predicate)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            MonadPlus<TAccumulate> retval = MonadPlus.Of(seed);

            using (var iter = @this.GetEnumerator())
            {
                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(_ => accumulator.Invoke(_, iter.Current));
                }
            }

            return retval;
        }

        internal static MonadPlus<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadPlus<TSource>> accumulator)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                MonadPlus<TSource> retval = MonadPlus.Of(iter.Current);

                while (iter.MoveNext())
                {
                    retval = retval.Bind(_ => accumulator.Invoke(_, iter.Current));
                }

                return retval;
            }
        }

        internal static MonadPlus<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadPlus<TSource>> accumulator,
            Func<MonadPlus<TSource>, bool> predicate)
            /* T4: type constraint */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                MonadPlus<TSource> retval = MonadPlus.Of(iter.Current);

                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(_ => accumulator.Invoke(_, iter.Current));
                }

                return retval;
            }
        }
    } // End of EnumerableExtensions - T4: EmitEnumerableInternalExtensions().
}

