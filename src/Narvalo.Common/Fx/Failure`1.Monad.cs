namespace Narvalo.Fx
{
    using System;

    public partial struct Failure<T>
    {
        public Failure<TResult> Bind<TResult>(Func<T, Failure<TResult>> kun) where TResult : Exception
        {
            Require.NotNull(kun, "kun");

            return kun(_exception);
        }

        public Failure<TResult> Map<TResult>(Func<T, TResult> fun) where TResult : Exception
        {
            Require.NotNull(fun, "fun");

            return Failure<TResult>.η(fun(_exception));
        }

        internal static Failure<T> η(T ex)
        {
            Require.NotNull(ex, "ex");

            return new Failure<T>(ex);
        }
    }
}