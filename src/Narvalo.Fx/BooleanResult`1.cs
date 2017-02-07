// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    public partial struct BooleanResult<TMessage> : IEquatable<BooleanResult<TMessage>>
    {
        private readonly TMessage _message;

        private BooleanResult(bool isTrue, TMessage message)
        {
            IsTrue = isTrue;
            _message = message;
        }

        public static BooleanResult<TMessage> True => new BooleanResult<TMessage>(true, default(TMessage));

        public bool IsFalse => !IsTrue;

        public bool IsTrue { get; }

        public TMessage Message
        {
            get
            {
                if (IsTrue) { throw new InvalidOperationException("XXX"); }
                return _message;
            }
        }

        public static BooleanResult<TMessage> False(TMessage message)
        {
            Require.NotNullUnconstrained(message, nameof(message));

            return new BooleanResult<TMessage>(false, message);
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return IsTrue ? "True" : ("False(" + _message.ToString() + ")");
        }

        public static implicit operator bool(BooleanResult<TMessage> value) => value.IsTrue;

    }

    // Provides the core Monad methods.
    public partial struct BooleanResult<TMessage>
    {
        public BooleanResult<TResult> Bind<TResult>(Func<TMessage, BooleanResult<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsFalse ? selector.Invoke(Message) : BooleanResult<TResult>.True;
        }

        [DebuggerHidden]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static BooleanResult<TMessage> η(TMessage value)
        {
            Require.NotNullUnconstrained(value, nameof(value));

            return new BooleanResult<TMessage>(false, value);
        }

        [DebuggerHidden]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static BooleanResult<TMessage> μ(BooleanResult<BooleanResult<TMessage>> square)
            => square.IsFalse ? square.Message : BooleanResult<TMessage>.True;
    }

    // Implements the IEquatable<BooleanResult<TMessage>> interface.
    public partial struct BooleanResult<TMessage>
    {
        public static bool operator ==(BooleanResult<TMessage> left, BooleanResult<TMessage> right)
            => left.Equals(right);

        public static bool operator !=(BooleanResult<TMessage> left, BooleanResult<TMessage> right)
            => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{TMessage}.Equals" />
        public bool Equals(BooleanResult<TMessage> other) => Equals(other, EqualityComparer<TMessage>.Default);

        public bool Equals(BooleanResult<TMessage> other, IEqualityComparer<TMessage> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.Equals(Message, other.Message);
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj) => Equals(obj, EqualityComparer<TMessage>.Default);

        public bool Equals(object other, IEqualityComparer<TMessage> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (!(other is BooleanResult<TMessage>))
            {
                return false;
            }

            return Equals((BooleanResult<TMessage>)other);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode() => GetHashCode(EqualityComparer<TMessage>.Default);

        public int GetHashCode(IEqualityComparer<TMessage> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.GetHashCode(Message);
        }
    }
}
