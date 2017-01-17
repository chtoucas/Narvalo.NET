// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Finance.Properties;

    public abstract partial class Outcome<T> where T : struct
    {
        private Outcome(bool success) { Success = success; }

        public bool Success { get; }

        public abstract string ErrorMessage { get; }

        public abstract T Value { get; }

        internal abstract Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
             where TResult : struct;

        internal abstract Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
             where TResult : struct;

        public static implicit operator Outcome<T>(T value) => Return(value);

        public static explicit operator T? (Outcome<T> value)
        {
            if (value == null) { return null; }

            return value.ToNullable();
        }

        internal static Outcome<T> Failure(string message)
        {
            Require.NotNull(message, nameof(message));
            Warrant.NotNull<Outcome<T>>();

            return new Failure_(message);
        }

        [DebuggerHidden]
        internal static Outcome<T> Return(T value)
        {
            Warrant.NotNull<Outcome<T>>();

            return new Success_(value);
        }

        public T? ToNullable()
        {
            if (!Success) { return null; }

            return Value;
        }

        private sealed partial class Success_ : Outcome<T>, IEquatable<Success_>
        {
            private readonly T _value;

            public Success_(T value) : base(true)
            {
                _value = value;
            }

            public override string ErrorMessage
            {
                get { throw new InvalidOperationException(Strings.Outcome_NoErrorMessage); }
            }

            public override T Value { get { return _value; } }

            internal override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Value);
            }

            internal override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return Outcome<TResult>.Return(selector.Invoke(Value));
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

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Success({0})", Value);
            }
        }

        private sealed partial class Failure_ : Outcome<T>, IEquatable<Failure_>
        {
            private readonly string _message;

            public Failure_(string message) : base(false)
            {
                Demand.NotNull(message);

                _message = message;
            }

            public override string ErrorMessage { get { Warrant.NotNull<string>(); return _message; } }

            public override T Value
            {
                get { throw new InvalidOperationException(Strings.Outcome_NoValue); }
            }

            internal override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
                => Outcome<TResult>.Failure(ErrorMessage);

            internal override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
                => Outcome<TResult>.Failure(ErrorMessage);

            public bool Equals(Failure_ other)
            {
                if (ReferenceEquals(other, null)) { return false; }

                return ErrorMessage == other.ErrorMessage;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) { return false; }
                if (ReferenceEquals(obj, this)) { return true; }
                // This class being sealed, we can ignore the next sentence.
                // if (obj.GetType() != GetType()) { return false; }

                return Equals(obj as Failure_);
            }

            public override int GetHashCode() => ErrorMessage.GetHashCode();

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Failure({0})", ErrorMessage);
            }
        }
    }
}
