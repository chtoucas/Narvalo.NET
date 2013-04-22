namespace Narvalo.Fx
{
    using System;

    public static class Outcome
    {
        public static Outcome<T> Failure<T>(Exception exception)
        {
            return Failure<T>(Error.Create(exception));
        }

        public static Outcome<T> Failure<T>(Func<Exception> exceptionFactory)
        {
            return Failure<T>(Error.Create(exceptionFactory));
        }

        public static Outcome<T> Failure<T>(string errorMessage)
        {
            return Failure<T>(Error.Create(errorMessage));
        }

        public static Outcome<T> Failure<T>(Func<string> errorMessageFactory)
        {
            return Failure<T>(Error.Create(errorMessageFactory));
        }

        public static Outcome<T> Failure<T>(Error error)
        {
            return Outcome<T>.Failure(error);
        }

        public static Outcome<T> Success<T>(T value)
        {
            return Outcome<T>.Success(value);
        }
    }
}
