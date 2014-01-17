namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    public abstract class Outcome<T> : EitherBase<Exception, T> 
    {
        protected Outcome(Exception exception) : base(exception) { }

        protected Outcome(T value) : base(value) { }

        public bool Successful { get { return IsRight; } }

        public bool Unsuccessful { get { return IsLeft; } }

        public Exception Exception { get { return LeftValue; } }

        public T Value { get { return RightValue; } }

        public T ValueOrThrow()
        {
            if (Unsuccessful) {
#if NET_40
                throw LeftValue;
#else
                ExceptionDispatchInfo.Capture(LeftValue).Throw();
#endif
            }

            return Value;
        }

        #region > Opérations monadiques <

        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Requires.NotNull(kun, "kun");

            return Unsuccessful ? Outcome<TResult>.Failure(Exception) : kun(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return Unsuccessful
               ? Outcome<TResult>.Failure(Exception)
               : Outcome<TResult>.Success(selector(Value));
        }

        public Outcome<TResult> Forget<TResult>()
        {
            return Map(_ => default(TResult));
        }

        #endregion

        internal static Outcome<T> Failure(Exception ex)
        {
            return new FailureCore(ex);
        }

        internal static Outcome<T> Failure(string errorMessage)
        {
            return new FailureCore(new OutcomeException(errorMessage));
        }

        internal static Outcome<T> Success(T value)
        {
            return new SuccessCore(value);
        }

        sealed class FailureCore : Outcome<T>
        {
            public FailureCore(Exception exception) : base(exception) { }
        }

        sealed class SuccessCore : Outcome<T>
        {
            public SuccessCore(T value) : base(value) { }
        }
    }
}
