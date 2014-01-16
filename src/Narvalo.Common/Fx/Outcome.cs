namespace Narvalo.Fx
{
    using System;

    public static class Outcome
    {
        public static Outcome<T> Failure<T>(Func<Exception> exceptionFactory)
        {
            return Failure<T>(exceptionFactory());
        }

        public static Outcome<T> Failure<T>(string errorMessage)
        {
            return Failure<T>(new OutcomeException(errorMessage));
        }

        public static Outcome<T> Failure<T>(Func<string> errorMessageFactory)
        {
            return Failure<T>(new OutcomeException(errorMessageFactory()));
        }

        public static Outcome<T> Failure<T>(Exception exception)
        {
            return Outcome<T>.Failure(exception);
        }

        public static Outcome<T> Success<T>(T value)
        {
            return Outcome<T>.Success(value);
        }
    }
}
