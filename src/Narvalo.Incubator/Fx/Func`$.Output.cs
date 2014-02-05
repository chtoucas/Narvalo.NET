namespace Narvalo.Fx
{
    using System;

    public static partial class FuncExtensions
    {
        public static Func<Output<TResult>> Guard<TResult>(this Func<TResult> @this)
        {
            Require.Object(@this);

            return () => Output.Return(@this);
        }

        public static Func<TSource, Output<TResult>> Guard<TSource, TResult>(this Func<TSource, TResult> @this)
        {
            Require.Object(@this);

            return (TSource value) => Output.Return(() => @this.Invoke(value));
        }

        public static Func<T1, T2, Output<TResult>> Guard<T1, T2, TResult>(this Func<T1, T2, TResult> @this)
        {
            Require.Object(@this);

            return (T1 v1, T2 v2) => Output.Return(() => @this.Invoke(v1, v2));
        }

        public static Func<T1, T2, T3, Output<TResult>> Guard<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this)
        {
            Require.Object(@this);

            return (T1 v1, T2 v2, T3 v3) => Output.Return(() => @this.Invoke(v1, v2, v3));
        }

        public static Func<T1, T2, T3, T4, Output<TResult>> Guard<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this)
        {
            Require.Object(@this);

            return (T1 v1, T2 v2, T3 v3, T4 v4) => Output.Return(() => @this.Invoke(v1, v2, v3, v4));
        }

        public static Func<T1, T2, T3, T4, T5, Output<TResult>> Guard<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this)
        {
            Require.Object(@this);

            return (T1 v1, T2 v2, T3 v3, T4 v4, T5 v5) => Output.Return(() => @this.Invoke(v1, v2, v3, v4, v5));
        }
    }
}
