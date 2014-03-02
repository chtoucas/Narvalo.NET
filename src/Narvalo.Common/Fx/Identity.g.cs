﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Creation Time: 03/02/2014 20:32:11
// Runtime Version: 4.0.30319.34011
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
    public static partial class Identity
    {
        static readonly Identity<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Identity<Unit> Unit { get { return Unit_; } }


        // [Haskell] return
        public static Identity<T> Return<T>(T value)
        {
            return Identity<T>.η(value);
        }
        
        #region Generalisations of list functions (Prelude)

        // [Haskell] join
        public static Identity<T> Flatten<T>(Identity<Identity<T>> square)
        {
            return Identity<T>.μ(square);
        }

        #endregion

        #region Monadic lifting operators

        public static Func<Identity<T>, Identity<TResult>> Lift<T, TResult>(Func<T, TResult> fun)
        {
            return m =>
            {
	            Require.NotNull(m, "m");
                return m.Select(fun);
            };
        }

        public static Func<Identity<T1>, Identity<T2>, Identity<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => 
            {
	            Require.NotNull(m1, "m1");
                return m1.Zip(m2, fun);
            };
        }

        public static Func<Identity<T1>, Identity<T2>, Identity<T3>, Identity<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) =>
            {
	            Require.NotNull(m1, "m1");
                return m1.Zip(m2, m3, fun);
            };
        }

        public static Func<Identity<T1>, Identity<T2>, Identity<T3>, Identity<T4>, Identity<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) =>
            {
	            Require.NotNull(m1, "m1");
                return m1.Zip(m2, m3, m4, fun);
            };
        }

        public static Func<Identity<T1>, Identity<T2>, Identity<T3>, Identity<T4>, Identity<T5>, Identity<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) =>
            {
	            Require.NotNull(m1, "m1");
                return m1.Zip(m2, m3, m4, m5, fun);
            };
        }

        #endregion
    }

    // Extensions for Identity<T>.
    public static partial class Identity
    {
        #region Basic Monad functions (Prelude)

        // [Haskell] fmap
        public static Identity<TResult> Select<TSource, TResult>(this Identity<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return @this.Bind(_ => Identity.Return(selector.Invoke(_)));
        }

        // [Haskell] >>
        public static Identity<TResult> Then<TSource, TResult>(this Identity<TSource> @this, Identity<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }
        
        #endregion

        #region Generalisations of list functions (Prelude)


        // [Haskell] replicateM
        public static Identity<IEnumerable<TSource>> Repeat<TSource>(this Identity<TSource> @this, int count)
        {
            return @this.Select(_ => Enumerable.Repeat(_, count));
        }
        
        #endregion

        #region Conditional execution of monadic expressions (Prelude)


        // [Haskell] when
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static Identity<Unit> When<TSource>(this Identity<TSource> @this, bool predicate, Action action)
        {
            Require.NotNull(action, "action");

            if (predicate) {
                action.Invoke();
            }

            return Identity.Unit;
        }

        // [Haskell] unless
        public static Identity<Unit> Unless<TSource>(this Identity<TSource> @this, bool predicate, Action action)
        {
            return @this.When(!predicate, action);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        // [Haskell] liftM2
        public static Identity<TResult> Zip<TFirst, TSecond, TResult>(
            this Identity<TFirst> @this,
            Identity<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        // [Haskell] liftM3
        public static Identity<TResult> Zip<T1, T2, T3, TResult>(
            this Identity<T1> @this,
            Identity<T2> second,
            Identity<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Identity<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        // [Haskell] liftM4
        public static Identity<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Identity<T1> @this,
             Identity<T2> second,
             Identity<T3> third,
             Identity<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Identity<TResult>> g
                = t1 => second.Zip(third, fourth, (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        // [Haskell] liftM5
        public static Identity<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Identity<T1> @this,
            Identity<T2> second,
            Identity<T3> third,
            Identity<T4> fourth,
            Identity<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Identity<TResult>> g
                = t1 => second.Zip(third, fourth, fifth, (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion

        #region Query Expression Pattern


        // Kind of generalisation of Zip (liftM2).
        public static Identity<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Identity<TSource> @this,
            Func<TSource, Identity<TMiddle>> valueSelectorM,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelectorM, "valueSelectorM");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelectorM.Invoke(_).Select(middle => resultSelector.Invoke(_, middle)));
        }


        #endregion
        
        #region Linq extensions


        #endregion

        #region Non-standard extensions
        
        public static Identity<TResult> Coalesce<TSource, TResult>(
            this Identity<TSource> @this,
            Func<TSource, bool> predicate,
            Identity<TResult> then,
            Identity<TResult> otherwise)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }


        public static Identity<TSource> Run<TSource>(this Identity<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(_ => { action.Invoke(_); return @this; });
        }


        #endregion
    }

    // Extensions for Func<T, Identity<TResult>>.
    public static partial class FuncExtensions
    {
        #region Basic Monad functions (Prelude)

        // [Haskell] =<<
        public static Identity<TResult> Invoke<TSource, TResult>(
            this Func<TSource, Identity<TResult>> @this,
            Identity<TSource> value)
        {
            return value.Bind(@this);
        }

        // [Haskell] >=>
        public static Func<TSource, Identity<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Identity<TMiddle>> @this,
            Func<TMiddle, Identity<TResult>> funM)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(funM);
        }

        // [Haskell] <=<
        public static Func<TSource, Identity<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Identity<TResult>> @this,
            Func<TSource, Identity<TMiddle>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion
    }
}

namespace Narvalo.Fx {
    // Comonad methods.
    public static partial class Identity
    {
        // [Haskell] extract
        public static T Extract<T>(Identity<T> monad)
        {
            return Identity<T>.ε(monad);
        }

        // [Haskell] duplicate
        public static Identity<Identity<T>> Duplicate<T>(Identity<T> monad)
        {
            return Identity<T>.δ(monad);
        }
    }
}
