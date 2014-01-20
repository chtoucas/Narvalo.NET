namespace Narvalo.Fx
{
    using System;
    using Narvalo.Internal;

    public partial struct Outcome<T>
    {
        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Require.NotNull(kun, "kun");

            return Unsuccessful ? Outcome<TResult>.η(_exception) : kun(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> fun)
        {
            Require.NotNull(fun, "fun");

            return Unsuccessful ? Outcome<TResult>.η(_exception) : Outcome<TResult>.η(fun(Value));
        }

        internal static Outcome<T> η(Exception ex)
        {
            Require.NotNull(ex, "ex");

            return new Outcome<T>(ex);
        }

        internal static Outcome<T> η(T value)
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            return new Outcome<T>(value);
        }
    }
}
