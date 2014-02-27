﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect behavior
// and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo.Fx {
	using System;
    using System.Collections.Generic;
    using System.Linq;
	using Narvalo.Fx;

    public static partial class Maybe
    {
        static readonly Maybe<Unit> Unit_ = Create(Narvalo.Fx.Unit.Single);
        static readonly Maybe<Unit> None_ = Maybe<Unit>.None;

        public static Maybe<Unit> Unit { get { return Unit_; } }

        public static Maybe<Unit> None { get { return None_; } }

        public static Maybe<T> Create<T>(T value)
        {
            return Maybe<T>.η(value);
        }

        public static Maybe<T> Flatten<T>(Maybe<Maybe<T>> square)
        {
            return Maybe<T>.μ(square);
        }

        public static Func<Maybe<T>, Maybe<TResult>> Lift<T, TResult>(Func<T, TResult> fun)
        {
            return m => m.Map(fun);
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
    }

    public static partial class MaybeExtensions
    {
        public static Maybe<TResult> Map<TSource, TResult>(this Maybe<TSource> @this, Func<TSource, TResult> selector)
        {
            return @this.Bind(_ => Maybe.Create(selector.Invoke(_)));
        }

        public static Maybe<TResult> Then<TSource, TResult>(this Maybe<TSource> @this, Maybe<TResult> other)
        {
            return @this.Bind(_ => other);
        }

        public static Maybe<TSource> Filter<TSource>(this Maybe<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? @this : Maybe<TSource>.None);
        }

        public static Maybe<IEnumerable<T>> Repeat<T>(this Maybe<T> @this, int count)
        {
            return @this.Map(_ => Enumerable.Repeat(_, count));
        }

        public static Maybe<TResult> Zip<TFirst, TSecond, TResult>(
            this Maybe<TFirst> @this,
            Maybe<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(v1 => second.Map(v2 => resultSelector.Invoke(v1, v2)));
        }

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
    }

	public static partial class KleisliExtensions
    {
        public static Maybe<TResult> Invoke<TSource, TResult>(
            this Func<TSource, Maybe<TResult>> @this,
            Maybe<TSource> monad)
        {
            return monad.Bind(@this);
        }

        public static Func<TSource, Maybe<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Maybe<TMiddle>> @this,
            Func<TMiddle, Maybe<TResult>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => @this.Invoke(_).Bind(kun);
        }

        public static Func<TSource, Maybe<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Maybe<TResult>> @this,
            Func<TSource, Maybe<TMiddle>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => kun.Invoke(_).Bind(@this);
        }
    }
}

namespace Narvalo.Fx {
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

	public static partial class MaybeExtensions
    {
        #region Query Expression Pattern

        public static Maybe<TSource> Where<TSource>(
            this Maybe<TSource> @this, 
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);

            return @this.Filter(predicate);
        }

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> @this, 
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);

            return @this.Map(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector.Invoke(_).Select(middle => resultSelector.Invoke(_, middle)));
        }

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            return Join(@this, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static Maybe<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Maybe<TInner>, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            throw new NotImplementedException();
        }

        #endregion

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
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

            throw new NotImplementedException();
        }
	}

	public static class EnumerableMonadExtensions
    {
        #region Basic Monad functions

        // [Haskell] sequence
        public static Maybe<IEnumerable<TSource>> Collect<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.Object(@this);

            var seed = Maybe.Create(Enumerable.Empty<TSource>());
            Func<Maybe<IEnumerable<TSource>>, Maybe<TSource>, Maybe<IEnumerable<TSource>>> fun
                = (m, n) =>
                    m.Bind(list =>
                    {
                        return n.Bind(item => Maybe.Create(list.Concat(Enumerable.Repeat(item, 1))));
                    });

            return @this.Aggregate(seed, fun);
        }
		
        #endregion

        #region Generalisations of list functions

        // [Haskell] msum
        public static Maybe<TSource> Sum<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.Object(@this);

            return @this.Aggregate(Maybe<TSource>.None, (m, n) => m.OrElse(n));
        }

        #endregion
	}

    public static partial class EnumerableExtensions
    {
        // [Haskell] mapM
        public static Maybe<IEnumerable<TResult>> Map<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return (from _ in @this select kun.Invoke(_)).Collect();
        }

        // [Haskell] filterM
        public static Maybe<IEnumerable<TSource>> Filter<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");

            // NB: Haskell uses tail recursion, we don't.
            var list = new List<TSource>();

            foreach (var item in @this) {
                predicateM.Invoke(item)
                    .Bind(_ =>
                    {
                        if (_ == true) {
                            list.Add(item);
                        }

                        return Maybe.Unit;
                    });
            }

            return Maybe.Create(list.AsEnumerable());
        }

        // [Haskell] mapAndUnzipM
        public static Maybe<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> MapAndUnzip<TSource, TFirst, TSecond>(
           this IEnumerable<TSource> @this,
           Func<TSource, Maybe<Tuple<TFirst, TSecond>>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return from _ in
                       (from _ in @this select kun.Invoke(_)).Collect()
                   let item1 = from item in _ select item.Item1
                   let item2 = from item in _ select item.Item2
                   select new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(item1, item2);
        }

        // [Haskell] zipWithM
        public static Maybe<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Maybe<TResult>> resultSelectorM)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelectorM, "resultSelectorM");

            Func<TFirst, TSecond, Maybe<TResult>> resultSelector = (v1, v2) => resultSelectorM.Invoke(v1, v2);

			// WARNING: Do not remove resultSelector, otherwise .NET will make a recursive call to Zip 
			// instead of using the Zip from Linq.
            return @this.Zip(second, resultSelector: resultSelector).Collect();
        }

        // [Haskell] foldM
        public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TAccumulate> result = Maybe.Create(seed);

            foreach (TSource item in @this) {
                result = result.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return result;
        }
    }
}
