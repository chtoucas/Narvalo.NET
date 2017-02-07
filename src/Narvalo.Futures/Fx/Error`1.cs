// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    public partial struct Error<TMessage> : IEquatable<Error<TMessage>>
    {
        private readonly TMessage _message;

        public Error(TMessage error)
        {
            Require.NotNullUnconstrained(error, nameof(error));

            _message = error;
        }

        public TMessage Message
        {
            get { Warrant.NotNullUnconstrained<TMessage>(); return _message; }
        }

        public override string ToString() => Message.ToString();
    }

    // Implements the IEquatable<Error<TMessage>> interface.
    public partial struct Error<TMessage>
    {
        public static bool operator ==(Error<TMessage> left, Error<TMessage> right) => left.Equals(right);

        public static bool operator !=(Error<TMessage> left, Error<TMessage> right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{TMessage}.Equals" />
        public bool Equals(Error<TMessage> other) => Equals(other, EqualityComparer<TMessage>.Default);

        public bool Equals(Error<TMessage> other, IEqualityComparer<TMessage> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.Equals(Message, other.Message);
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj) => Equals(obj, EqualityComparer<TMessage>.Default);

        public bool Equals(object other, IEqualityComparer<TMessage> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (!(other is Error<TMessage>))
            {
                return false;
            }

            return Equals((Error<TMessage>)other);
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
