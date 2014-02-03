namespace Narvalo.Fx
{
    using System;

    public static class Outcome
    {
        public static Outcome<T> Failure<T>(Exception exception)
        {
            return Outcome<T>.η(exception);
        }

        public static Outcome<T> Success<T>(T value)
        {
            return Outcome<T>.η(value);
        }

        public static Outcome<T> Create<T>(Func<T> fun)
        {
            // FIXME: Return Outcome<ArgumentNullException>.
            Require.NotNull(fun, "fun");

            try {
                T value = fun.Invoke();
                return Outcome.Success(value);
            }
            catch (Exception ex) {
                return Outcome.Failure<T>(ex);
            }
        }
    }
}
