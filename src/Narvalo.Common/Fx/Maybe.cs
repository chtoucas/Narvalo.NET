// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Maybe
    {
        static readonly Maybe<Unit?> None_ = Maybe<Unit?>.None;
        static readonly Maybe<Unit> Unit_ = Maybe.Create(Narvalo.Fx.Unit.Single);

        public static Maybe<Unit?> None { get { return None_; } }

        public static Maybe<Unit> Unit { get { return Unit_; } }

        //// Create

        public static Maybe<T> Create<T>(T value)
        {
            return Maybe<T>.η(value);
        }

        public static Maybe<T> Create<T>(T? value) where T : struct
        {
            return value.HasValue ? Maybe<T>.η(value.Value) : Maybe<T>.None;
        }

        //// Join

        public static Maybe<T> Join<T>(Maybe<Maybe<T>> square)
        {
            return Maybe<T>.μ(square);
        }

        //// Compose

        public static Maybe<TResult> Compose<TSource, TMiddle, TResult>(
            Func<TSource, Maybe<TMiddle>> kunA,
            Func<TMiddle, Maybe<TResult>> kunB,
            TSource value)
        {
            Require.NotNull(kunA, "kunA");
            Require.NotNull(kunB, "kunB");

            return kunA.Invoke(value).Bind(kunB);
        }

        //// Lift

        public static Maybe<TResult> Lift<TSource, TResult>(
            Func<TSource, TResult> fun,
            Maybe<TSource> option)
        {
            Require.NotNull(fun, "fun");

            return option.Map(fun);
        }

        public static Maybe<TResult> Lift<T1, T2, TResult>(
            Func<T1, T2, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2,
            Maybe<T3> option3)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value, option3.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2,
            Maybe<T3> option3,
            Maybe<T4> option4)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome && option4.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value, option3.Value, option4.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, T4, T5, TResult>(
           Func<T1, T2, T3, T4, T5, TResult> fun,
           Maybe<T1> option1,
           Maybe<T2> option2,
           Maybe<T3> option3,
           Maybe<T4> option4,
           Maybe<T5> option5)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome && option4.IsSome && option5.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value, option3.Value, option4.Value, option5.Value))
                : Maybe<TResult>.None;
        }

        //// Promote

        public static Func<Maybe<TSource>, Maybe<TResult>>
            Promote<TSource, TResult>(Func<TSource, TResult> fun)
        {
            return m => Lift(fun, m);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>>
            Promote<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => Lift(fun, m1, m2);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>>
            Promote<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) => Lift(fun, m1, m2, m3);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>>
            Promote<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) => Lift(fun, m1, m2, m3, m4);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>>
            Promote<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) => Lift(fun, m1, m2, m3, m4, m5);
        }
    }
}
