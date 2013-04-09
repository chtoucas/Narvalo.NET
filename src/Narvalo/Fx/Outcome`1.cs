namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    public abstract class Outcome<T> : IEquatable<Outcome<T>>
    {
        readonly bool _successful;
        readonly Error _error;
        readonly T _value;

        Outcome(Error error)
        {
            _successful = false;
            _error = error;
            _value = default(T);
        }

        Outcome(T value)
        {
            _successful = true;
            _error = default(Error);
            _value = value;
        }

        public bool Failed { get { return !_successful; } }

        public bool Successful { get { return _successful; } }

        public Error Error
        {
            get
            {
                if (_successful) {
                    throw new InvalidOperationException("XXX");
                }
                return _error;
            }
        }

        public T Value
        {
            get
            {
                if (!_successful) {
                    throw new InvalidOperationException("XXX");
                }
                return _value;
            }
        }

        public abstract TResult Switch<TResult>(
            Func<Error, TResult> caseLeft,
            Func<T, TResult> caseRight);

        public abstract void Switch(
            Action<Error> caseLeft,
            Action<T> caseRight);

        #region > Opérations monadiques <

        public static Outcome<T> Failure(string errorMessage)
        {
            return new Outcome<T>.FailureImpl(new Error(errorMessage));
        }

        public static Outcome<T> Failure(Exception exception)
        {
            return new Outcome<T>.FailureImpl(new Error(exception));
        }

        public static Outcome<T> Failure(Error error)
        {
            return new Outcome<T>.FailureImpl(error);
        }

        public static Outcome<T> Success(T value)
        {
            return new Outcome<T>.SuccessImpl(value);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Requires.NotNull(kun, "kun");

            return !_successful ? Outcome<TResult>.Failure(_error) : kun(_value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return !_successful
               ? Outcome<TResult>.Failure(_error)
               : Outcome<TResult>.Success(selector(_value));
        }

        public Outcome<TResult> Forget<TResult>()
        {
            return Map(_ => default(TResult));
        }

        #endregion

        public sealed class FailureImpl : Outcome<T>, IEquatable<FailureImpl>
        {
            public FailureImpl(Error error) : base(error) { }

            public Error Result { get { return Error; } }

            public override TResult Switch<TResult>(
                Func<Error, TResult> caseLeft,
                Func<T, TResult> caseRight)
            {
                return caseLeft(Error);
            }

            public override void Switch(
                Action<Error> caseLeft,
                Action<T> caseRight)
            {
                caseLeft(Error);
            }

            #region IEquatable<Left>

            public bool Equals(FailureImpl other)
            {
                if (other == this) { return true; }
                if (other == null) { return false; }

                return EqualityComparer<Error>.Default.Equals(Error, other.Error);
            }

            #endregion

            public override bool Equals(object obj)
            {
                return Equals(obj as Error);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<Error>.Default.GetHashCode(Error);
            }

            public override string ToString()
            {
                return String.Format(CultureInfo.CurrentCulture, "Failure({0})", Value);
            }
        }

        public sealed class SuccessImpl : Outcome<T>, IEquatable<SuccessImpl>
        {
            public SuccessImpl(T value) : base(value) { }

            public T Result { get { return Value; } }

            public override TResult Switch<TResult>(
                Func<Error, TResult> caseLeft,
                Func<T, TResult> caseRight)
            {
                return caseRight(Value);
            }

            public override void Switch(
                Action<Error> caseLeft,
                Action<T> caseRight)
            {
                caseRight(Value);
            }

            #region IEquatable<Right>

            public bool Equals(SuccessImpl other)
            {
                if (other == this) { return true; }
                if (other == null) { return false; }

                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            #endregion

            public override bool Equals(object obj)
            {
                return Equals(obj as SuccessImpl);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<T>.Default.GetHashCode(Value);
            }

            public override string ToString()
            {
                return String.Format(CultureInfo.CurrentCulture, "Success({0})", Value);
            }
        }

        #region IEquatable<Error<T>>

        public bool Equals(Outcome<T> other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
