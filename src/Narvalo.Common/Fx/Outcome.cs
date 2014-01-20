namespace Narvalo.Fx
{
    using System;

    public static class Outcome
    {
        //// Create

        public static Outcome<T> Create<T>(Exception ex)
        {
            return Outcome<T>.η(ex);
        }

        public static Outcome<T> Create<T>(T value)
        {
            return Outcome<T>.η(value);
        }

        //// Failure

        public static Outcome<T> Failure<T>(string errorMessage)
        {
            Require.NotNullOrEmpty(errorMessage, "errorMessage");

            return Create<T>(new OutcomeException(errorMessage));
        }

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
