// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Finance.Properties;

    public abstract partial class Result<T> where T : struct
    {
        private Result(bool isSuccess) { IsSuccess = isSuccess; }

        public bool IsSuccess { get; }

        public bool IsError => !IsSuccess;

        public abstract string Error { get; }

        public abstract T Value { get; }

        internal abstract Result<TResult> Bind<TResult>(Func<T, Result<TResult>> selector)
             where TResult : struct;

        internal abstract Result<TResult> Select<TResult>(Func<T, TResult> selector)
             where TResult : struct;

        public static implicit operator Result<T>(T value) => Return(value);

        public static explicit operator T? (Result<T> value)
        {
            if (value == null) { return null; }

            return value.ToNullable();
        }

        internal static Result<T> FromError(string message)
        {
            Require.NotNull(message, nameof(message));

            return new Error_(message);
        }

        [DebuggerHidden]
        internal static Result<T> Return(T value) => new Success_(value);

        public T? ToNullable()
        {
            if (!IsSuccess) { return null; }

            return Value;
        }

        private sealed partial class Success_ : Result<T>, IEquatable<Success_>
        {
            private readonly T _value;

            public Success_(T value) : base(true)
            {
                _value = value;
            }

            public override string Error
            {
                get { throw new InvalidOperationException(Strings.Result_NoErrorMessage); }
            }

            public override T Value { get { return _value; } }

            internal override Result<TResult> Bind<TResult>(Func<T, Result<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Value);
            }

            internal override Result<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return Result<TResult>.Return(selector.Invoke(Value));
            }

            public bool Equals(Success_ other)
            {
                if (ReferenceEquals(other, null)) { return false; }

                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) { return false; }
                if (ReferenceEquals(obj, this)) { return true; }
                // This class being sealed, we can ignore the next sentence.
                // if (obj.GetType() != GetType()) { return false; }

                return Equals(obj as Success_);
            }

            public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

            public override string ToString() => Format.Current("Success({0})", Value);
        }

        private sealed partial class Error_ : Result<T>, IEquatable<Error_>
        {
            private readonly string _message;

            public Error_(string message) : base(false)
            {
                Demand.NotNull(message);

                _message = message;
            }

            public override string Error => _message;

            public override T Value
            {
                get { throw new InvalidOperationException(Strings.Result_NoValue); }
            }

            internal override Result<TResult> Bind<TResult>(Func<T, Result<TResult>> selector)
                => Result<TResult>.FromError(Error);

            internal override Result<TResult> Select<TResult>(Func<T, TResult> selector)
                => Result<TResult>.FromError(Error);

            public bool Equals(Error_ other)
            {
                if (ReferenceEquals(other, null)) { return false; }

                return Error == other.Error;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) { return false; }
                if (ReferenceEquals(obj, this)) { return true; }
                // This class being sealed, we can ignore the next sentence.
                // if (obj.GetType() != GetType()) { return false; }

                return Equals(obj as Error_);
            }

            public override int GetHashCode() => Error.GetHashCode();

            public override string ToString() => Format.Current("Error({0})", Error);
        }
    }
}
