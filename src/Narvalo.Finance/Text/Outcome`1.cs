// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Text
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract partial class Outcome<T>
    {
        private readonly bool _isSuccess;

        private Outcome(bool isSuccess) { _isSuccess = isSuccess; }

        public bool Success { get { return _isSuccess; } }

        public virtual string Message
        {
            get { throw new InvalidOperationException(); }
        }

        public virtual T Value
        {
            get { throw new InvalidOperationException(); }
        }

        public abstract Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector);

        public abstract Outcome<TResult> Select<TResult>(Func<T, TResult> selector);

        #region Core Monad methods

        [DebuggerHidden]
        internal static Outcome<T> η(T value) => new Success_(value);

        [DebuggerHidden]
        internal static Outcome<T> η(string message)
        {
            Require.NotNull(message, nameof(message));

            return new Failure_(message);
        }

        #endregion

        private sealed partial class Success_ : Outcome<T>, IEquatable<Success_>
        {
            private readonly T _value;

            public Success_(T value) : base(true)
            {
                _value = value;
            }

            public override T Value
            {
                get { return _value; }
            }

            public override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Value);
            }

            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return Outcome<TResult>.η(selector.Invoke(Value));
            }

            public bool Equals(Success_ other)
            {
                if (ReferenceEquals(other, this)) { return true; }
                if (ReferenceEquals(other, null)) { return false; }

                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj) => Equals(obj as Success_);

            public override int GetHashCode()
                => Value == null ? 0 : EqualityComparer<T>.Default.GetHashCode(Value);

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

            public override string Message
            {
                get { Warrant.NotNull<string>(); return _message; }
            }

            public override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
                => Outcome<TResult>.η(Message);

            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
                => Outcome<TResult>.η(Message);

            public bool Equals(Failure_ other)
            {
                if (ReferenceEquals(other, this)) { return true; }
                if (ReferenceEquals(other, null)) { return false; }

                return Message == other.Message;
            }

            public override bool Equals(object obj) => Equals(obj as Failure_);

            public override int GetHashCode() => Message.GetHashCode();

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Failure({0})", Message);
            }
        }
    }
}
