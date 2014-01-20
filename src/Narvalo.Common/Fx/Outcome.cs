namespace Narvalo.Fx
{
    using System;

    public static class Outcome
    {
        //// Create

        public static Outcome<T> Create<T>(Exception exception)
        {
            return Outcome<T>.η(exception);
        }

        public static Outcome<T> Create<T>(T value)
        {
            return Outcome<T>.η(value);
        }

        //// Failure

        public static Outcome<T> Failure<T>(Exception exception)
        {
            return Create<T>(exception);
        }

        //// Success

        public static Outcome<T> Success<T>(T value)
        {
            return Create<T>(value);
        }
    }
}
