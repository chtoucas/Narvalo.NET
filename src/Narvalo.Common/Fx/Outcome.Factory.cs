namespace Narvalo.Fx
{
    using System;

    public partial struct Outcome
    {
        // REVIEW: Utiliser default ici ne revient-il pas à null ?
        static readonly Outcome Success_ = new Outcome(true, default(OutcomeException));

        public static Outcome Success
        {
            get { return Success_; }
        }

        public static Outcome Failure(string errorMessage)
        {
            return new Outcome(false, new OutcomeException(errorMessage));
        }

        public static Outcome Failure(Exception ex)
        {
            return new Outcome(false, ex);
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
