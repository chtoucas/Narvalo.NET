namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public abstract class Outcome<T> : EitherBase<Error, T> 
    {
        protected Outcome(Error error) : base(error) { }

        protected Outcome(T value) : base(value) { }

        public bool Successful { get { return IsRight; } }

        public bool Unsuccessful { get { return IsLeft; } }

        public Error Error { get { return LeftValue; } }

        public T Value { get { return RightValue; } }

        public T ValueOrThrow()
        {
            if (Unsuccessful) {
                Error.ThrowException();
            }
            return Value;
        }

        #region > Opérations monadiques <

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Outcome<T> Failure(Error error)
        {
            return new FailureImpl(error);
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Outcome<T> Failure(Exception ex)
        {
            return new FailureImpl(Error.Create(ex));
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Outcome<T> Failure(string errorMessage)
        {
            return new FailureImpl(Error.Create(errorMessage));
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Outcome<T> Success(T value)
        {
            return new SuccessImpl(value);
        }

        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Requires.NotNull(kun, "kun");

            return Unsuccessful ? Outcome<TResult>.Failure(Error) : kun(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return Unsuccessful
               ? Outcome<TResult>.Failure(Error)
               : Outcome<TResult>.Success(selector(Value));
        }

        public Outcome<TResult> Forget<TResult>()
        {
            return Map(_ => default(TResult));
        }

        #endregion

        sealed class FailureImpl : Outcome<T>
        {
            public FailureImpl(Error error) : base(error) { }
        }

        sealed class SuccessImpl : Outcome<T>
        {
            public SuccessImpl(T value) : base(value) { }
        }
    }
}
