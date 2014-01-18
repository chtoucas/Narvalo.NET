namespace Narvalo.Fx
{
    using System;
    using System.Globalization;
    using System.Runtime.ExceptionServices;

    public class Outcome<T>
    {
        readonly bool _successful;
        readonly Exception _exception;
        readonly T _value;

        Outcome(Exception exception)
        {
            _successful = false;
            _exception = exception;
        }

        Outcome(T value)
        {
            _successful = true;
            _value = value;
        }

        public bool Successful { get { return _successful; } }

        public bool Unsuccessful { get { return !_successful; } }

        public Exception Exception
        {
            get
            {
                if (Successful) {
                    throw new InvalidOperationException(SR.Outcome_SuccessfulHasNoException);
                }

                return _exception;
            }
        }

        public T Value
        {
            get
            {
                if (Unsuccessful) {
                    throw new InvalidOperationException(SR.Outcome_UnsuccessfulHasNoValue);
                }

                return _value;
            }
        }

        public T ValueOrThrow()
        {
            if (Unsuccessful) {
#if NET_40
                throw LeftValue;
#else
                ExceptionDispatchInfo.Capture(Exception).Throw();
#endif
            }

            return Value;
        }

        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Require.NotNull(kun, "kun");

            return Unsuccessful ? Outcome<TResult>.Failure(Exception) : kun(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return Unsuccessful
               ? Outcome<TResult>.Failure(Exception)
               : Outcome<TResult>.Success(selector(Value));
        }

        public Outcome<TResult> Forget<TResult>()
        {
            return Map(_ => default(TResult));
        }

        public override string ToString()
        {
            return Successful ? SR.Outcome_Successful : SR.Outcome_Unsuccessful;
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
