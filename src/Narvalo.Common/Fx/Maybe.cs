namespace Narvalo.Fx
{
    using System;

    public static partial class Maybe
    {
        // REVIEW: Maybe<Unit>.None ?
        public static readonly Maybe<Unit> Unit = default(Maybe<Narvalo.Fx.Unit>);

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
            MayFunc<TSource, TMiddle> kunA,
            MayFunc<TMiddle, TResult> kunB,
            TSource value)
        {
            Require.NotNull(kunA, "kunA");
            Require.NotNull(kunB, "kunB");

            return kunA(value).Bind(kunB);
        }

        //// Lift

        public static Maybe<TResult> Lift<T, TResult>(
            Func<T, TResult> fun,
            Maybe<T> option)
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
                ? Maybe.Create(fun(option1.Value, option2.Value))
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
                ? Maybe.Create(fun(option1.Value, option2.Value, option3.Value))
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
                ? Maybe.Create(fun(option1.Value, option2.Value, option3.Value, option4.Value))
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
                ? Maybe.Create(fun(option1.Value, option2.Value, option3.Value, option4.Value, option5.Value))
                : Maybe<TResult>.None;
        }

        //// Promote

        public static Func<Maybe<T>, Maybe<TResult>> Promote<T, TResult>(Func<T, TResult> fun)
        {
            return m => Lift(fun, m);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>> Promote<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => Lift(fun, m1, m2);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>> Promote<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) => Lift(fun, m1, m2, m3);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>> Promote<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) => Lift(fun, m1, m2, m3, m4);
        }

        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>> Promote<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) => Lift(fun, m1, m2, m3, m4, m5);
        }
    }
}
