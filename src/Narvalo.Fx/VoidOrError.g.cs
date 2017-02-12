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


namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Provides a set of static methods for VoidOrError<T>.
    /// </summary>
    // NB: Sometimes we prefer extension methods over static methods to be able to override them locally.
    public static partial class VoidOrError
    {
        /// <summary>
        /// The unique object of type <c>VoidOrError&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly VoidOrError<global::Narvalo.Fx.Unit> s_Unit = Error(global::Narvalo.Fx.Unit.Single);

        /// <summary>
        /// Gets the unique object of type <c>VoidOrError&lt;Unit&gt;</c>.
        /// </summary>
        /// <value>The unique object of type <c>VoidOrError&lt;Unit&gt;</c>.</value>
        public static VoidOrError<global::Narvalo.Fx.Unit> Unit
        {
            get
            {
                Warrant.NotNull<VoidOrError<global::Narvalo.Fx.Unit>>();

                return s_Unit;
            }
        }


        /// <summary>
        /// Gets the zero for <see cref="VoidOrError{T}"/>.
        /// </summary>
        /// <value>The zero for <see cref="VoidOrError{T}"/>.</value>
        // Named "mzero" in Haskell parlance.
        public static VoidOrError<global::Narvalo.Fx.Unit> Void
        {
            get
            {
                Warrant.NotNull<VoidOrError<global::Narvalo.Fx.Unit>>();

                return VoidOrError<global::Narvalo.Fx.Unit>.Void;
            }
        }


        /// <summary>
        /// Obtains an instance of the <see cref="VoidOrError{T}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into a <see cref="VoidOrError{T}"/> object.</param>
        /// <returns>An instance of the <see cref="VoidOrError{T}"/> class for the specified value.</returns>
        // Named "return" in Haskell parlance.
        public static VoidOrError<T> Error<T>(T value)
            /* T4: C# indent */
        {
            Warrant.NotNull<VoidOrError<T>>();

            return VoidOrError<T>.η(value);
        }

        #region Generalisations of list functions (Prelude)

        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        // Named "join" in Haskell parlance.
        public static VoidOrError<T> Flatten<T>(VoidOrError<VoidOrError<T>> square)
            /* T4: C# indent */
        {
            Expect.NotNull(square);
            Warrant.NotNull<VoidOrError<T>>();

            return VoidOrError<T>.μ(square);
        }

        #endregion

        #region Conditional execution of monadic expressions (Prelude)


        // Named "guard" in Haskell parlance.
        public static VoidOrError<global::Narvalo.Fx.Unit> Guard(bool predicate)
        {
            Warrant.NotNull<VoidOrError<global::Narvalo.Fx.Unit>>();

            return predicate ? VoidOrError.Unit : VoidOrError<global::Narvalo.Fx.Unit>.Void;
        }


        // Named "when" in Haskell parlance. Haskell uses a different signature.
        public static void When<TSource>(
            this VoidOrError<TSource> @this,
            Func<TSource, bool> predicate,
            Action<TSource> action)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            @this.Bind(_ => { if (predicate.Invoke(_)) { action.Invoke(_); } return VoidOrError.Unit; });
        }

        // Named "unless" in Haskell parlance. Haskell uses a different signature.
        public static void Unless<TSource>(
            this VoidOrError<TSource> @this,
            Func<TSource, bool> predicate,
            Action<TSource> action)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            @this.Bind(_ => { if (!predicate.Invoke(_)) { action.Invoke(_); } return VoidOrError.Unit; });
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        /// <summary>
        /// Promotes a function to use and return <see cref="VoidOrError{T}" /> values.
        /// </summary>
        // Named "liftM" in Haskell parlance.
        public static Func<VoidOrError<T>, VoidOrError<TResult>> Lift<T, TResult>(
            Func<T, TResult> fun)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<VoidOrError<T>, VoidOrError<TResult>>>();

            return m =>
            {
                Require.NotNull(m, nameof(m));
                return m.Select(fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="VoidOrError{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM2" in Haskell parlance.
        public static Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<TResult>>>();

            return (m1, m2) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="VoidOrError{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM3" in Haskell parlance.
        public static Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<T3>, VoidOrError<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<T3>, VoidOrError<TResult>>>();

            return (m1, m2, m3) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, m3, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="VoidOrError{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM4" in Haskell parlance.
        public static Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<T3>, VoidOrError<T4>, VoidOrError<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<T3>, VoidOrError<T4>, VoidOrError<TResult>>>();

            return (m1, m2, m3, m4) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, m3, m4, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="VoidOrError{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM5" in Haskell parlance.
        public static Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<T3>, VoidOrError<T4>, VoidOrError<T5>, VoidOrError<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<VoidOrError<T1>, VoidOrError<T2>, VoidOrError<T3>, VoidOrError<T4>, VoidOrError<T5>, VoidOrError<TResult>>>();

            return (m1, m2, m3, m4, m5) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, m3, m4, m5, fun);
            };
        }

        #endregion
    } // End of VoidOrError - T4: EmitMonadCore().

    // Provides the core monadic extension methods for VoidOrError<T>.
    public static partial class VoidOrError
    {
        #region Basic Monad functions (Prelude)

        // Named "fmap", "liftA" or "<$>" in Haskell parlance.
        public static VoidOrError<TResult> Select<TSource, TResult>(
            this VoidOrError<TSource> @this,
            Func<TSource, TResult> selector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(_ => VoidOrError.Error(selector.Invoke(_)));
        }

        // Named ">>" in Haskell parlance.
        public static VoidOrError<TResult> Then<TSource, TResult>(
            this VoidOrError<TSource> @this,
            VoidOrError<TResult> other)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(_ => other);
        }

        // Named "forever" in Haskell parlance.
        public static VoidOrError<TResult> Forever<TSource, TResult>(
            this VoidOrError<TSource> @this,
            Func<VoidOrError<TResult>> fun
            )
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Then(@this.Forever(fun));
        }

        // Named "void" in Haskell parlance.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this", Justification = "[Intentionally] This method always returns the same result.")]
        public static VoidOrError<global::Narvalo.Fx.Unit> Forget<TSource>(this VoidOrError<TSource> @this)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<VoidOrError<global::Narvalo.Fx.Unit>>();

            return VoidOrError.Unit;
        }

        #endregion

        #region Generalisations of list functions (Prelude)


        // Named "mfilter" in Haskell parlance.
        public static VoidOrError<TSource> Where<TSource>(
            this VoidOrError<TSource> @this,
            Func<TSource, bool> predicate)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<VoidOrError<TSource>>();

            return @this.Bind(
                _ => predicate.Invoke(_) ? @this : VoidOrError<TSource>.Void);
        }


        // Named "replicateM" in Haskell parlance.
        public static VoidOrError<IEnumerable<TSource>> Repeat<TSource>(
            this VoidOrError<TSource> @this,
            int count)
        {
            Require.NotNull(@this, nameof(@this));
            Require.Range(count >= 1, nameof(count));
            Warrant.NotNull<VoidOrError<IEnumerable<TSource>>>();

            return @this.Select(_ => Enumerable.Repeat(_, count));
        }


        #endregion

        #region Monadic lifting operators (Prelude)

        /// <see cref="Lift{T1, T2, T3}" />
        // Named "liftA2" in Haskell parlance.
        public static VoidOrError<TResult> Zip<TFirst, TSecond, TResult>(
            this VoidOrError<TFirst> @this,
            VoidOrError<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        /// <see cref="Lift{T1, T2, T3, T4}" />
        // Named "liftA3" in Haskell parlance.
        public static VoidOrError<TResult> Zip<T1, T2, T3, TResult>(
            this VoidOrError<T1> @this,
            VoidOrError<T2> second,
            VoidOrError<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<VoidOrError<TResult>>();

            Func<T1, VoidOrError<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5}" />
        // Named "liftA4" in Haskell parlance.
        public static VoidOrError<TResult> Zip<T1, T2, T3, T4, TResult>(
             this VoidOrError<T1> @this,
             VoidOrError<T2> second,
             VoidOrError<T3> third,
             VoidOrError<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<VoidOrError<TResult>>();

            Func<T1, VoidOrError<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5, T6}" />
        // Named "liftA5" in Haskell parlance.
        public static VoidOrError<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this VoidOrError<T1> @this,
            VoidOrError<T2> second,
            VoidOrError<T3> third,
            VoidOrError<T4> fourth,
            VoidOrError<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<VoidOrError<TResult>>();

            Func<T1, VoidOrError<TResult>> g
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
        /// Kind of generalisation of <see cref="Zip{T1, T2, T3}" /> (liftM2).
        /// </remarks>
        public static VoidOrError<TResult> SelectMany<TSource, TMiddle, TResult>(
            this VoidOrError<TSource> @this,
            Func<TSource, VoidOrError<TMiddle>> valueSelectorM,
            Func<TSource, TMiddle, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(valueSelectorM, nameof(valueSelectorM));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(
                _ => valueSelectorM.Invoke(_).Select(
                    middle => resultSelector.Invoke(_, middle)));
        }


        public static VoidOrError<TResult> Join<TSource, TInner, TKey, TResult>(
            this VoidOrError<TSource> @this,
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Expect.NotNull(inner);
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);
            Warrant.NotNull<VoidOrError<TResult>>();

            return JoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);
        }

        public static VoidOrError<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this VoidOrError<TSource> @this,
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, VoidOrError<TInner>, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Expect.NotNull(inner);
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);
            Warrant.NotNull<VoidOrError<TResult>>();

            return GroupJoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);
        }


        #endregion

        #region LINQ extensions


        public static VoidOrError<TResult> Join<TSource, TInner, TKey, TResult>(
            this VoidOrError<TSource> @this,
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(inner);
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);
            Warrant.NotNull<VoidOrError<TResult>>();

            return JoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);
        }

        public static VoidOrError<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this VoidOrError<TSource> @this,
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, VoidOrError<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(inner);
            Expect.NotNull(outerKeySelector);
            Expect.NotNull(innerKeySelector);
            Expect.NotNull(resultSelector);
            Warrant.NotNull<VoidOrError<TResult>>();

            return GroupJoinImpl(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer ?? EqualityComparer<TKey>.Default);
        }


        private static VoidOrError<TResult> JoinImpl<TSource, TInner, TKey, TResult>(
            VoidOrError<TSource> seq,
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: C# indent */
        {
            Require.NotNull(seq, nameof(seq));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Demand.NotNull(inner);
            Demand.NotNull(outerKeySelector);
            Demand.NotNull(innerKeySelector);
            Demand.NotNull(comparer);
            Warrant.NotNull<VoidOrError<TResult>>();

            var keyLookupM = GetKeyLookup(inner, outerKeySelector, innerKeySelector, comparer);

            return from outerValue in seq
                   from innerValue in keyLookupM.Invoke(outerValue).Then(inner)
                   select resultSelector.Invoke(outerValue, innerValue);
        }

        private static VoidOrError<TResult> GroupJoinImpl<TSource, TInner, TKey, TResult>(
            VoidOrError<TSource> seq,
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, VoidOrError<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
            /* T4: C# indent */
        {
            Require.NotNull(seq, nameof(seq));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Demand.NotNull(inner);
            Demand.NotNull(outerKeySelector);
            Demand.NotNull(innerKeySelector);
            Demand.NotNull(comparer);
            Warrant.NotNull<VoidOrError<TResult>>();

            var keyLookupM = GetKeyLookup(inner, outerKeySelector, innerKeySelector, comparer);

            return from outerValue in seq
                   select resultSelector.Invoke(outerValue, keyLookupM.Invoke(outerValue).Then(inner));
        }

        private static Func<TSource, VoidOrError<TKey>> GetKeyLookup<TSource, TInner, TKey>(
            VoidOrError<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
            /* T4: C# indent */
        {
            Require.NotNull(inner, nameof(inner));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(comparer, nameof(comparer));
            Demand.NotNull(innerKeySelector);
            Warrant.NotNull<Func<TSource, VoidOrError<TKey>>>();

            return source =>
            {
                TKey outerKey = outerKeySelector.Invoke(source);

                return inner.Select(innerKeySelector).Where(_ => comparer.Equals(_, outerKey));
            };
        }


        #endregion
    } // End of VoidOrError - T4: EmitMonadExtensions().

    // Provides more extension methods for VoidOrError<T>.
    public static partial class VoidOrError
    {
        #region Applicative

        // Named "<$" in Haskell parlance.
        public static VoidOrError<TSource> Replace<TSource>(
            this VoidOrError<TSource> @this,
            TSource value)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<VoidOrError<TSource>>();

            return @this.Select(_ => value);
        }

        #endregion

        public static VoidOrError<TResult> Coalesce<TSource, TResult>(
            this VoidOrError<TSource> @this,
            Func<TSource, bool> predicate,
            VoidOrError<TResult> then,
            VoidOrError<TResult> otherwise)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }


        // Generalizes the standard Then().
        public static VoidOrError<TResult> Then<TSource, TResult>(
            this VoidOrError<TSource> @this,
            Func<TSource, bool> predicate,
            VoidOrError<TResult> other)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(_ => predicate.Invoke(_) ? other : VoidOrError<TResult>.Void);
        }

        public static VoidOrError<TResult> Otherwise<TSource, TResult>(
            this VoidOrError<TSource> @this,
            Func<TSource, bool> predicate,
            VoidOrError<TResult> other)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<VoidOrError<TResult>>();

            return @this.Bind(_ => !predicate.Invoke(_) ? other : VoidOrError<TResult>.Void);
        }

        public static void Do<TSource>(
            this VoidOrError<TSource> @this,
            Action<TSource> action)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError<TSource>>();

            @this.Bind(_ => { action.Invoke(_); return VoidOrError.Unit; });
        }
    } // End of VoidOrError - T4: EmitMonadExtraExtensions().

    // Provides extension methods for Func<T> in the Kleisli category.
    public static partial class Func
    {
        #region Basic Monad functions (Prelude)


        // Named "=<<" in Haskell parlance. Same as Bind (>>=) with its arguments flipped.
        public static VoidOrError<TResult> Invoke<TSource, TResult>(
            this Func<TSource, VoidOrError<TResult>> @this,
            VoidOrError<TSource> value)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Require.NotNull(value, nameof(value));
            Warrant.NotNull<VoidOrError<TResult>>();

            return value.Bind(@this);
        }

        // Named ">=>" in Haskell parlance.
        public static Func<TSource, VoidOrError<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, VoidOrError<TMiddle>> @this,
            Func<TMiddle, VoidOrError<TResult>> funM)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Expect.NotNull(funM);
            Warrant.NotNull<Func<TSource, VoidOrError<TResult>>>();

            return _ => @this.Invoke(_).Bind(funM);
        }

        // Named "<=<" in Haskell parlance.
        public static Func<TSource, VoidOrError<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, VoidOrError<TResult>> @this,
            Func<TSource, VoidOrError<TMiddle>> funM)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Require.NotNull(funM, nameof(funM));
            Warrant.NotNull<Func<TSource, VoidOrError<TResult>>>();

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion
    } // End of Func - T4: EmitKleisliExtensions().
}