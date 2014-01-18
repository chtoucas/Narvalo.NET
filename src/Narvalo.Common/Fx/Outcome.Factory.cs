namespace Narvalo.Fx
{
    using System;

    public partial class Outcome
    {
        public static readonly Outcome Success = new Outcome();

        public static Outcome Failure(string errorMessage)
        {
            return new Outcome(new OutcomeException(errorMessage));
        }

        public static Outcome Failure(Exception ex)
        {
            return new Outcome(ex);
        }

        public static Outcome<T> Failure<T>(string errorMessage)
        {
            return Outcome<T>.Failure(new OutcomeException(errorMessage));
        }

        public static Outcome<T> Failure<T>(Exception exception)
        {
            return Outcome<T>.Failure(exception);
        }

        public static Outcome<T> Create<T>(T value)
        {
            return Outcome<T>.Success(value);
        }
    }
}
