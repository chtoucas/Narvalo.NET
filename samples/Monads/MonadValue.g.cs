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

namespace Monads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Monads.Linq;

    // Provides a set of static methods for MonadValue<T>.
    // NB: Sometimes we prefer extension methods over static methods to be able to override them locally.
    public static partial class MonadValue
    {
        /// <summary>
        /// The unique object of type <c>MonadValue&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly MonadValue<global::Narvalo.Fx.Unit> s_Unit = Pure(global::Narvalo.Fx.Unit.Single);

        /// <summary>
        /// Gets the unique object of type <c>MonadValue&lt;Unit&gt;</c>.
        /// </summary>
        /// <value>The unique object of type <c>MonadValue&lt;Unit&gt;</c>.</value>
        public static MonadValue<global::Narvalo.Fx.Unit> Unit
        {
            get
            {

                return s_Unit;
            }
        }


        /// <summary>
        /// Gets the zero for <see cref="MonadValue{T}"/>.
        /// </summary>
        /// <remarks>
        /// Named <c>mzero</c> in Haskell parlance.
        /// </remarks>
        /// <value>The zero for <see cref="MonadValue{T}"/>.</value>
        public static MonadValue<global::Narvalo.Fx.Unit> None
        {
            get
            {

                return MonadValue<global::Narvalo.Fx.Unit>.None;
            }
        }


        /// <summary>
        /// Obtains an instance of the <see cref="MonadValue{T}"/> class for the specified value.
        /// </summary>
        /// <remarks>
        /// Named <c>return</c> in Haskell parlance.
        /// </remarks>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into a <see cref="MonadValue{T}"/> object.</param>
        /// <returns>An instance of the <see cref="MonadValue{T}"/> class for the specified value.</returns>
        public static MonadValue<T> Pure<T>(T value)
            where T : struct
        {

            return MonadValue<T>.η(value);
        }

        #region Generalisations of list functions (Prelude)

        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        /// <remarks>
        /// Named <c>join</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<T> Flatten<T>(MonadValue<MonadValue<T>> square)
            where T : struct
        {

            return MonadValue<T>.μ(square);
        }

        #endregion

        #region Conditional execution of monadic expressions (Prelude)


        /// <remarks>
        /// Named <c>guard</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<global::Narvalo.Fx.Unit> Guard(bool predicate)
        {

            return predicate ? MonadValue.Unit : MonadValue<global::Narvalo.Fx.Unit>.None;
        }


        /// <remarks>
        /// <para>Named <c>when</c> in Haskell parlance.</para>
        /// <para>Haskell uses a different signature.</para>
        /// </remarks>
        public static void When<TSource>(
            this MonadValue<TSource> @this,
            Func<TSource, bool> predicate,
            Action<TSource> action)
            where TSource : struct
        {
            /* T4: C# indent */
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            @this.Bind(_ => { if (predicate.Invoke(_)) { action.Invoke(_); } return MonadValue.Unit; });
        }

        /// <remarks>
        /// <para>Named <c>unless</c> in Haskell parlance.</para>
        /// <para>Haskell uses a different signature.</para>
        /// </remarks>
        public static void Unless<TSource>(
            this MonadValue<TSource> @this,
            Func<TSource, bool> predicate,
            Action<TSource> action)
            where TSource : struct
        {
            Expect.NotNull(predicate);
            Expect.NotNull(action);

            @this.When(_ => !predicate.Invoke(_), action);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadValue{T}" /> values.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM</c> in Haskell parlance.
        /// </remarks>
        public static Func<MonadValue<T>, MonadValue<TResult>> Lift<T, TResult>(
            Func<T, TResult> fun)
            where T : struct
            where TResult : struct
        {
            Warrant.NotNull<Func<MonadValue<T>, MonadValue<TResult>>>();

            return m =>
            {
                /* T4: C# indent */
                return m.Select(fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadValue{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM2</c> in Haskell parlance.
        /// </remarks>
        public static Func<MonadValue<T1>, MonadValue<T2>, MonadValue<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
            where T1 : struct
            where T2 : struct
            where TResult : struct
        {
            Warrant.NotNull<Func<MonadValue<T1>, MonadValue<T2>, MonadValue<TResult>>>();

            return (m1, m2) =>
            {
                /* T4: C# indent */
                return m1.Zip(m2, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadValue{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM3</c> in Haskell parlance.
        /// </remarks>
        public static Func<MonadValue<T1>, MonadValue<T2>, MonadValue<T3>, MonadValue<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where TResult : struct
        {
            Warrant.NotNull<Func<MonadValue<T1>, MonadValue<T2>, MonadValue<T3>, MonadValue<TResult>>>();

            return (m1, m2, m3) =>
            {
                /* T4: C# indent */
                return m1.Zip(m2, m3, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadValue{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM4</c> in Haskell parlance.
        /// </remarks>
        public static Func<MonadValue<T1>, MonadValue<T2>, MonadValue<T3>, MonadValue<T4>, MonadValue<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where TResult : struct
        {
            Warrant.NotNull<Func<MonadValue<T1>, MonadValue<T2>, MonadValue<T3>, MonadValue<T4>, MonadValue<TResult>>>();

            return (m1, m2, m3, m4) =>
            {
                /* T4: C# indent */
                return m1.Zip(m2, m3, m4, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadValue{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM5</c> in Haskell parlance.
        /// </remarks>
        public static Func<MonadValue<T1>, MonadValue<T2>, MonadValue<T3>, MonadValue<T4>, MonadValue<T5>, MonadValue<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where TResult : struct
        {
            Warrant.NotNull<Func<MonadValue<T1>, MonadValue<T2>, MonadValue<T3>, MonadValue<T4>, MonadValue<T5>, MonadValue<TResult>>>();

            return (m1, m2, m3, m4, m5) =>
            {
                /* T4: C# indent */
                return m1.Zip(m2, m3, m4, m5, fun);
            };
        }

        #endregion
    } // End of MonadValue - T4: EmitMonadCore().

    // Provides the core monadic extension methods for MonadValue<T>.
    public static partial class MonadValue
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>fmap</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<TResult> Select<TSource, TResult>(
            this MonadValue<TSource> @this,
            Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Require.NotNull(selector, nameof(selector));

            return @this.Bind(_ => MonadValue.Pure(selector.Invoke(_)));
        }

        /// <remarks>
        /// Named <c>&gt;&gt;</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<TResult> Then<TSource, TResult>(
            this MonadValue<TSource> @this,
            MonadValue<TResult> other)
            where TSource : struct
            where TResult : struct
        {
            /* T4: C# indent */

            return @this.Bind(_ => other);
        }


        /// <remarks>
        /// Named <c>void</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<global::Narvalo.Fx.Unit> Ignore<TSource>(this MonadValue<TSource> @this)
            where TSource : struct
        {
            /* T4: C# indent */

            return MonadValue.Unit;
        }

        #endregion

        #region Generalisations of list functions (Prelude)


        /// <remarks>
        /// Named <c>mfilter</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<TSource> Where<TSource>(
            this MonadValue<TSource> @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            /* T4: C# indent */
            Require.NotNull(predicate, nameof(predicate));

            return @this.Bind(
                _ => predicate.Invoke(_) ? @this : MonadValue<TSource>.None);
        }


        #endregion

        #region Monadic lifting operators (Prelude)

        /// <see cref="Lift{T1, T2, T3}" />
        public static MonadValue<TResult> Zip<TFirst, TSecond, TResult>(
            this MonadValue<TFirst> @this,
            MonadValue<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
            where TFirst : struct
            where TSecond : struct
            where TResult : struct
        {
            /* T4: C# indent */
            /* T4: C# indent */
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        /// <see cref="Lift{T1, T2, T3, T4}" />
        public static MonadValue<TResult> Zip<T1, T2, T3, TResult>(
            this MonadValue<T1> @this,
            MonadValue<T2> second,
            MonadValue<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where TResult : struct
        {
            /* T4: C# indent */
            /* T4: C# indent */
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, MonadValue<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5}" />
        public static MonadValue<TResult> Zip<T1, T2, T3, T4, TResult>(
             this MonadValue<T1> @this,
             MonadValue<T2> second,
             MonadValue<T3> third,
             MonadValue<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where TResult : struct
        {
            /* T4: C# indent */
            /* T4: C# indent */
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, MonadValue<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5, T6}" />
        public static MonadValue<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this MonadValue<T1> @this,
            MonadValue<T2> second,
            MonadValue<T3> third,
            MonadValue<T4> fourth,
            MonadValue<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where TResult : struct
        {
            /* T4: C# indent */
            /* T4: C# indent */
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, MonadValue<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    fifth,
                    (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion

        #region Query Expression Pattern


        /// <remarks>
        /// Kind of generalisation of Zip (liftM2).
        /// </remarks>
        public static MonadValue<TResult> SelectMany<TSource, TMiddle, TResult>(
            this MonadValue<TSource> @this,
            Func<TSource, MonadValue<TMiddle>> valueSelectorM,
            Func<TSource, TMiddle, TResult> resultSelector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Require.NotNull(valueSelectorM, nameof(valueSelectorM));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                _ => valueSelectorM.Invoke(_).Select(
                    middle => resultSelector.Invoke(_, middle)));
        }


        public static MonadValue<TResult> Join<TSource, TInner, TKey, TResult>(
            this MonadValue<TSource> @this,
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);

            return @this.Join(
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);
        }

        public static MonadValue<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this MonadValue<TSource> @this,
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadValue<TInner>, TResult> resultSelector)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);

            return @this.GroupJoin(
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);
        }


        #endregion

        #region LINQ extensions


        public static MonadValue<TResult> Join<TSource, TInner, TKey, TResult>(
            this MonadValue<TSource> @this,
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);

            return JoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);
        }

        public static MonadValue<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this MonadValue<TSource> @this,
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadValue<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);

            return GroupJoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);
        }


        private static MonadValue<TResult> JoinImpl<TSource, TInner, TKey, TResult>(
            MonadValue<TSource> seq,
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Require.NotNull(resultSelector, nameof(resultSelector));
            Demand.NotNull(outerKeySelector);
            Demand.NotNull(innerKeySelector);
            Demand.NotNull(comparer);

            var keyLookupM = GetKeyLookup(inner, outerKeySelector, innerKeySelector, comparer);

            return from outerValue in seq
                   from innerValue in keyLookupM.Invoke(outerValue).Then(inner)
                   select resultSelector.Invoke(outerValue, innerValue);
        }

        private static MonadValue<TResult> GroupJoinImpl<TSource, TInner, TKey, TResult>(
            MonadValue<TSource> seq,
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadValue<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Require.NotNull(resultSelector, nameof(resultSelector));
            Demand.NotNull(outerKeySelector);
            Demand.NotNull(innerKeySelector);
            Demand.NotNull(comparer);

            var keyLookupM = GetKeyLookup(inner, outerKeySelector, innerKeySelector, comparer);

            return from outerValue in seq
                   select resultSelector.Invoke(outerValue, keyLookupM.Invoke(outerValue).Then(inner));
        }

        private static Func<TSource, MonadValue<TKey>> GetKeyLookup<TSource, TInner, TKey>(
            MonadValue<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
            where TSource : struct
            where TInner : struct
            where TKey : struct
        {
            /* T4: C# indent */
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(comparer, nameof(comparer));
            Demand.NotNull(innerKeySelector);
            Warrant.NotNull<Func<TSource, MonadValue<TKey>>>();

            return source =>
            {
                TKey outerKey = outerKeySelector.Invoke(source);

                return inner.Select(innerKeySelector).Where(_ => comparer.Equals(_, outerKey));
            };
        }


        #endregion
    } // End of MonadValue - T4: EmitMonadExtensions().

    // Provides non-standard extension methods for MonadValue<T>.
    public static partial class MonadValue
    {
        public static MonadValue<TResult> Coalesce<TSource, TResult>(
            this MonadValue<TSource> @this,
            Func<TSource, bool> predicate,
            MonadValue<TResult> then,
            MonadValue<TResult> otherwise)
            where TSource : struct
            where TResult : struct
        {
            /* T4: C# indent */
            Require.NotNull(predicate, nameof(predicate));

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }


        public static MonadValue<TResult> Then<TSource, TResult>(
            this MonadValue<TSource> @this,
            Func<TSource, bool> predicate,
            MonadValue<TResult> other)
            where TSource : struct
            where TResult : struct
        {
            Expect.NotNull(predicate);

            return @this.Coalesce(predicate, other, MonadValue<TResult>.None);
        }

        public static MonadValue<TResult> Otherwise<TSource, TResult>(
            this MonadValue<TSource> @this,
            Func<TSource, bool> predicate,
            MonadValue<TResult> other)
            where TSource : struct
            where TResult : struct
        {
            Expect.NotNull(predicate);

            return @this.Coalesce(predicate, MonadValue<TResult>.None, other);
        }


        // Like Select() w/ an action.
        public static void Apply<TSource>(
            this MonadValue<TSource> @this,
            Action<TSource> action)
            where TSource : struct
        {
            /* T4: C# indent */
            Require.NotNull(action, nameof(action));

            @this.Bind(_ => { action.Invoke(_); return MonadValue.Unit; });
        }
    } // End of MonadValue - T4: EmitMonadExtraExtensions().

    // Provides extension methods for Func<T> in the Kleisli category.
    public static partial class Func
    {
        #region Basic Monad functions (Prelude)


        /// <remarks>
        /// Named <c>=&lt;&lt;</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<TResult> Invoke<TSource, TResult>(
            this Func<TSource, MonadValue<TResult>> @this,
            MonadValue<TSource> value)
            where TSource : struct
            where TResult : struct
        {
            Expect.NotNull(@this);
            /* T4: C# indent */

            return value.Bind(@this);
        }

        /// <remarks>
        /// Named <c>&gt;=&gt;</c> in Haskell parlance.
        /// </remarks>
        public static Func<TSource, MonadValue<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, MonadValue<TMiddle>> @this,
            Func<TMiddle, MonadValue<TResult>> funM)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(@this, nameof(@this));
            Expect.NotNull(funM);
            Warrant.NotNull<Func<TSource, MonadValue<TResult>>>();

            return _ => @this.Invoke(_).Bind(funM);
        }

        /// <remarks>
        /// Named <c>&lt;=&lt;</c> in Haskell parlance.
        /// </remarks>
        public static Func<TSource, MonadValue<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, MonadValue<TResult>> @this,
            Func<TSource, MonadValue<TMiddle>> funM)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Expect.NotNull(@this);
            Require.NotNull(funM, nameof(funM));
            Warrant.NotNull<Func<TSource, MonadValue<TResult>>>();

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion
    } // End of Func - T4: EmitKleisliExtensions().
}

namespace Monads
{
    using System.Collections.Generic;

    using Monads.Internal;

    // Provides extension methods for IEnumerable<T> where T is a MonadValue<S>.
    public static partial class Sequence
    {
        #region Basic Monad functions (Prelude)


        #endregion


        #region Generalisations of list functions (Prelude)

        /// <remarks>
        /// Named <c>msum</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<TSource> Sum<TSource>(
            this IEnumerable<MonadValue<TSource>> @this)
            where TSource : struct
        {
            Expect.NotNull(@this);

            return @this.SumImpl();
        }

        #endregion

    } // End of Sequence - T4: EmitMonadEnumerableExtensions().
}

namespace Monads.Linq
{
    using System;
    using System.Collections.Generic;

    using Monads;
    using Monads.Internal;

    // Provides extension methods for IEnumerable<T>.
    // We do not use the standard LINQ names to avoid a confusing API (see ZipWithImpl()).
    // - Select    -> Map
    // - Where     -> Filter
    // - Zip       -> ZipWith
    // - Aggregate -> Reduce or Fold
    public static partial class EnumerableExtensions
    {
        #region Basic Monad functions (Prelude)


        #endregion

        #region Generalisations of list functions (Prelude)


        /// <remarks>
        /// Named <c>foldM</c> in Haskell parlance.
        /// </remarks>
        public static MonadValue<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadValue<TAccumulate>> accumulatorM)
            where TSource : struct
            where TAccumulate : struct
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulatorM);

            return @this.FoldImpl(seed, accumulatorM);
        }

        #endregion

        #region Aggregate Operators

        public static MonadValue<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadValue<TAccumulate>> accumulatorM)
            where TSource : struct
            where TAccumulate : struct
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulatorM);

            return @this.FoldBackImpl(seed, accumulatorM);
        }

        public static MonadValue<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadValue<TSource>> accumulatorM)
            where TSource : struct
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulatorM);

            return @this.ReduceImpl(accumulatorM);
        }

        public static MonadValue<TSource> ReduceBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadValue<TSource>> accumulatorM)
            where TSource : struct
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulatorM);

            return @this.ReduceBackImpl(accumulatorM);
        }

        #endregion

        #region Catamorphisms

        /// <remarks>
        /// <para>Haskell uses a different signature.</para>
        /// </remarks>
        public static MonadValue<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadValue<TAccumulate>> accumulatorM,
            Func<MonadValue<TAccumulate>, bool> predicate)
            where TSource : struct
            where TAccumulate : struct
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulatorM);
            Expect.NotNull(predicate);

            return @this.FoldImpl(seed, accumulatorM, predicate);
        }

        /// <remarks>
        /// <para>Haskell uses a different signature.</para>
        /// </remarks>
        public static MonadValue<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadValue<TSource>> accumulatorM,
            Func<MonadValue<TSource>, bool> predicate)
            where TSource : struct
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulatorM);
            Expect.NotNull(predicate);

            return @this.ReduceImpl(accumulatorM, predicate);
        }

        #endregion
    } // End of EnumerableExtensions - T4: EmitEnumerableExtensions().
}

namespace Monads.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Monads.Linq;

    // Provides the core extension methods for IEnumerable<T> where T is a MonadValue<S>.
    internal static partial class EnumerableExtensions
    {

        internal static MonadValue<TSource> SumImpl<TSource>(
            this IEnumerable<MonadValue<TSource>> @this)
            where TSource : struct
        {
            Demand.NotNull(@this);

            var retval = @this.Aggregate(MonadValue<TSource>.None, (m, n) => m.OrElse(n));

            return retval;
        }

    } // End of EnumerableExtensions - T4: EmitMonadEnumerableInternalExtensions().

    // Provides the core extension methods for IEnumerable<T>.
    internal static partial class EnumerableExtensions
    {

        internal static MonadValue<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadValue<TAccumulate>> accumulatorM)
            where TSource : struct
            where TAccumulate : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulatorM, nameof(accumulatorM));

            MonadValue<TAccumulate> retval = MonadValue.Pure(seed);

            foreach (TSource item in @this)
            {
                retval = retval.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return retval;
        }

        internal static MonadValue<TAccumulate> FoldBackImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadValue<TAccumulate>> accumulatorM)
            where TSource : struct
            where TAccumulate : struct
        {
            Demand.NotNull(@this);
            Demand.NotNull(accumulatorM);

            return @this.Reverse().EmptyIfNull().Fold(seed, accumulatorM);
        }

        internal static MonadValue<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadValue<TSource>> accumulatorM)
            where TSource : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulatorM, nameof(accumulatorM));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                MonadValue<TSource> retval = MonadValue.Pure(iter.Current);

                while (iter.MoveNext())
                {
                    retval = retval.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return retval;
            }
        }

        internal static MonadValue<TSource> ReduceBackImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadValue<TSource>> accumulatorM)
            where TSource : struct
        {
            Demand.NotNull(@this);
            Demand.NotNull(accumulatorM);

            return @this.Reverse().EmptyIfNull().Reduce(accumulatorM);
        }

        internal static MonadValue<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadValue<TAccumulate>> accumulatorM,
            Func<MonadValue<TAccumulate>, bool> predicate)
            where TSource : struct
            where TAccumulate : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulatorM, nameof(accumulatorM));
            Require.NotNull(predicate, nameof(predicate));

            MonadValue<TAccumulate> retval = MonadValue.Pure(seed);

            using (var iter = @this.GetEnumerator())
            {
                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }
            }

            return retval;
        }

        internal static MonadValue<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, MonadValue<TSource>> accumulatorM,
            Func<MonadValue<TSource>, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulatorM, nameof(accumulatorM));
            Require.NotNull(predicate, nameof(predicate));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                MonadValue<TSource> retval = MonadValue.Pure(iter.Current);

                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return retval;
            }
        }
    } // End of EnumerableExtensions - T4: EmitEnumerableInternalExtensions().
}

