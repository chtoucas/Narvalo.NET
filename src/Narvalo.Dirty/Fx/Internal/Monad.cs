namespace Narvalo.Fx.Internal
{
    using System;

    static class Monad
    {
        public static readonly Monad<Unit> Unit = Monad.Return(Narvalo.Fx.Unit.Single);

        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        public static Monad<T> Join<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        // Composition de Kleisli.
        public static Monad<TResult> Compose<TSource, TMiddle, TResult>(
            Kunc<TSource, TMiddle> kunA,
            Kunc<TMiddle, TResult> kunB,
            TSource value)
        {
            return kunA(value).Bind(kunB);
        }

        public static Monad<TResult> Lift<T, TResult>(
            Func<T, TResult> fun,
            Monad<T> monad)
        {
            return monad.Map(fun);
        }

        public static Monad<TResult> Lift<T1, T2, TResult>(
            Func<T1, T2, TResult> fun,
            Monad<T1> monad1,
            Monad<T2> monad2)
        {
            Kunc<T1, TResult> g = t1 => Lift(t2 => fun(t1, t2), monad2);

            return monad1.Bind(g);
        }

        public static Monad<TResult> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> fun,
            Monad<T1> monad1,
            Monad<T2> monad2,
            Monad<T3> monad3)
        {
            Kunc<T1, TResult> g
                = t1 => Lift((t2, t3) => fun(t1, t2, t3), monad2, monad3);

            return monad1.Bind(g);
        }

        public static Monad<TResult> Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun,
            Monad<T1> monad1,
            Monad<T2> monad2,
            Monad<T3> monad3,
            Monad<T4> monad4)
        {
            Kunc<T1, TResult> g
                = t1 => Lift((t2, t3, t4) => fun(t1, t2, t3, t4), monad2, monad3, monad4);

            return monad1.Bind(g);
        }

        public static Monad<TResult> Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun,
            Monad<T1> monad1,
            Monad<T2> monad2,
            Monad<T3> monad3,
            Monad<T4> monad4,
            Monad<T5> monad5)
        {
            Kunc<T1, TResult> g
                = t1 => Lift((t2, t3, t4, t5) => fun(t1, t2, t3, t4, t5), monad2, monad3, monad4, monad5);

            return monad1.Bind(g);
        }

        public static Func<Monad<T>, Monad<TResult>> Promote<T, TResult>(Func<T, TResult> fun)
        {
            return m => Lift(fun, m);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<TResult>> Promote<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            return (m1, m2) => Lift(fun, m1, m2);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>> Promote<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            return (m1, m2, m3) => Lift(fun, m1, m2, m3);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>> Promote<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            return (m1, m2, m3, m4) => Lift(fun, m1, m2, m3, m4);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>> Promote<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            return (m1, m2, m3, m4, m5) => Lift(fun, m1, m2, m3, m4, m5);
        }
    }
}
