﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo.Fx {
	using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Narvalo;      // For Require
	using Narvalo.Fx;   // For Unit

	// Monad methods.
    public static partial class Maybe
    {
        static readonly Maybe<Unit> Unit_ = Create(Narvalo.Fx.Unit.Single);
        static readonly Maybe<Unit> None_ = Maybe<Unit>.None;

        public static Maybe<Unit> Unit { get { return Unit_; } }

        // [Haskell] mzero
        public static Maybe<Unit> None { get { return None_; } }

        // [Haskell] return
        public static Maybe<T> Create<T>(T value)
        {
            return Maybe<T>.η(value);
        }
		
        #region Generalisations of list functions (Prelude)

        // [Haskell] join
        public static Maybe<T> Flatten<T>(Maybe<Maybe<T>> square)
        {
            return Maybe<T>.μ(square);
        }

        #endregion

		#region Monadic lifting operators

        public static Func<Maybe<T>, Maybe<TResult>> Lift<T, TResult>(Func<T, TResult> fun)
        {
            return m => m.Select(fun);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => m1.Zip(m2, fun);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) => m1.Zip(m2, m3, fun);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) => m1.Zip(m2, m3, m4, fun);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) => m1.Zip(m2, m3, m4, m5, fun);
        }

        #endregion
    }
	// Extensions for Maybe<T>.
    public static partial class MaybeExtensions
    {
		#region Basic Monad functions (Prelude)

        // [Haskell] fmap
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> @this, Func<TSource, TResult> selector)
        {
            return @this.Bind(_ => Maybe.Create(selector.Invoke(_)));
        }

		// [Haskell] >>
        public static Maybe<TResult> Then<TSource, TResult>(this Maybe<TSource> @this, Maybe<TResult> other)
        
        {
            return @this.Bind(_ => other);
        }
		
        #endregion

        #region Generalisations of list functions (Prelude)

        // [Haskell] mfilter
        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? @this : Maybe<TSource>.None);
        }
        // [Haskell] replicateM
        public static Maybe<IEnumerable<TSource>> Repeat<TSource>(this Maybe<TSource> @this, int count)
        {
            return @this.Select(_ => Enumerable.Repeat(_, count));
        }
		
        #endregion

        #region Conditional execution of monadic expressions (Prelude)

        // [Haskell] guard
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static Maybe<Unit> Guard<TSource>(this Maybe<TSource> @this, bool predicate)
        {
            return predicate ? Maybe.Unit : Maybe.None;
        }

        // [Haskell] when
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static Maybe<Unit> When<TSource>(this Maybe<TSource> @this, bool predicate, Action action)
        {
            Require.NotNull(action, "action");

			if (predicate) {
				action.Invoke();
			}

            return Maybe.Unit;
        }

        // [Haskell] unless
        public static Maybe<Unit> Unless<TSource>(this Maybe<TSource> @this, bool predicate, Action action)
        {
            return @this.When(!predicate, action);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        // [Haskell] liftM2
        public static Maybe<TResult> Zip<TFirst, TSecond, TResult>(
            this Maybe<TFirst> @this,
            Maybe<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        // [Haskell] liftM3
        public static Maybe<TResult> Zip<T1, T2, T3, TResult>(
            this Maybe<T1> @this,
            Maybe<T2> second,
            Maybe<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Maybe<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        // [Haskell] liftM4
        public static Maybe<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Maybe<T1> @this,
             Maybe<T2> second,
             Maybe<T3> third,
             Maybe<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Maybe<TResult>> g
                = t1 => second.Zip(third, fourth, (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        // [Haskell] liftM5
        public static Maybe<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Maybe<T1> @this,
            Maybe<T2> second,
            Maybe<T3> third,
            Maybe<T4> fourth,
            Maybe<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Maybe<TResult>> g
                = t1 => second.Zip(third, fourth, fifth, (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion

        #region Query Expression Pattern


        // Kind of generalisation of Zip (liftM2).
        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TMiddle>> valueSelectorM,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelectorM, "valueSelectorM");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelectorM.Invoke(_).Select(middle => resultSelector.Invoke(_, middle)));
        }

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            return @this.Join(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static Maybe<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Maybe<TInner>, TResult> resultSelector)
        {
            return @this.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        #endregion
        
        #region Linq extensions

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            return JoinCore_(
				@this,
				inner,
				outerKeySelector,
				innerKeySelector,
				resultSelector,
				comparer ?? EqualityComparer<TKey>.Default);
        }

        public static Maybe<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            return GroupJoinCore_(
				@this,
				inner,
				outerKeySelector,
				innerKeySelector,
				resultSelector,
				comparer ?? EqualityComparer<TKey>.Default);
        }

        static Maybe<TResult> JoinCore_<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.Object(@this);
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");
            
            var keyLookupM = GetKeyLookup_(inner, outerKeySelector, innerKeySelector, comparer);

            return from outerValue in @this
                   from innerValue in keyLookupM.Invoke(outerValue).Then(inner)
                   select resultSelector.Invoke(outerValue, innerValue);
        }
        
        static Maybe<TResult> GroupJoinCore_<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.Object(@this);
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            var keyLookupM = GetKeyLookup_(inner, outerKeySelector, innerKeySelector, comparer);

            return from outerValue in @this
                   select resultSelector.Invoke(outerValue, keyLookupM.Invoke(outerValue).Then(inner));
        }

        static Func<TSource, Maybe<TKey>> GetKeyLookup_<TSource, TInner, TKey>(
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
        {
            return source =>
            {
                TKey outerKey = outerKeySelector.Invoke(source);
            
                return inner.Select(innerKeySelector).Where(_ => comparer.Equals(_, outerKey));
            };
        }

        #endregion

        #region Non-standard extensions
        
        public static Maybe<TResult> Coalesce<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate,
            Maybe<TResult> then,
            Maybe<TResult> otherwise)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }

        public static Maybe<TResult> Then<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate,
            Maybe<TResult> other)
        {
            return @this.Coalesce(predicate, other, Maybe<TResult>.None);
        }

        public static Maybe<TResult> Otherwise<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate,
            Maybe<TResult> other)
        {
            return @this.Coalesce(predicate, Maybe<TResult>.None, other);
        }

        public static Maybe<TSource> Run<TSource>(this Maybe<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(_ => { action.Invoke(_); return @this; });
        }

        public static Maybe<TSource> OnNone<TSource>(this Maybe<TSource> @this, Action action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            // REVIEW
            return @this.Then(Maybe.Unit).Run(_ => action.Invoke()).Then(@this);
        }

        #endregion
    }
	// Extensions for Func<T, Maybe<TResult>>.
	public static partial class FuncExtensions
    {
        #region Basic Monad functions (Prelude)

        // [Haskell] =<<
        public static Maybe<TResult> Invoke<TSource, TResult>(
            this Func<TSource, Maybe<TResult>> @this,
            Maybe<TSource> monad)
        {
            return monad.Bind(@this);
        }

        // [Haskell] >=>
        public static Func<TSource, Maybe<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Maybe<TMiddle>> @this,
            Func<TMiddle, Maybe<TResult>> funM)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(funM);
        }

        // [Haskell] <=<
        public static Func<TSource, Maybe<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Maybe<TResult>> @this,
            Func<TSource, Maybe<TMiddle>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion
    }
}
