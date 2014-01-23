namespace Narvalo.Fx
{
    using System;

    public static class Outcome
    {
        //// Failure

        public static Outcome<T> Failure<T>(Exception exception)
        {
            return Outcome<T>.η(exception);
        }

        //// Success

        public static Outcome<T> Success<T>(T value)
        {
            return Outcome<T>.η(value);
        }
    }
}
