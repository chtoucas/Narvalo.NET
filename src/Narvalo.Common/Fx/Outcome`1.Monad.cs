namespace Narvalo.Fx
{
    using System;

    public partial struct Outcome<T>
    {
        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Require.NotNull(kun, "kun");

            return Unsuccessful ? Outcome<TResult>.Failure(_exception) : kun(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return Unsuccessful
               ? Outcome<TResult>.Failure(_exception)
               : Outcome<TResult>.Success(selector(Value));
        }

        internal static Outcome<T> Failure(Exception ex)
        {
            return new Outcome<T>(ex);
        }

        internal static Outcome<T> Failure(string errorMessage)
        {
            return new Outcome<T>(new OutcomeException(errorMessage));
        }

        internal static Outcome<T> Success(T value)
        {
            return new Outcome<T>(value);
        }
    }
}
