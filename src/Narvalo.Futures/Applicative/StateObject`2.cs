// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // Tuple<T, TState>
    public partial struct StateObject<T, TState> : IEquatable<StateObject<T, TState>>, IStructuralEquatable
    {
        public StateObject(T value, TState state)
        {
            Value = value;
            State = state;
        }

        public T Value { get; }

        public TState State { get; }

        public override string ToString() => "Value=" + Value.ToString() + "; State=" + State.ToString();
    }

    // Implements the IEquatable<StateObject<T, TState>> and IStructuralEquatable interfaces.
    public partial struct StateObject<T, TState>
    {
        public static bool operator ==(StateObject<T, TState> left, StateObject<T, TState> right)
            => left.Equals(right);

        public static bool operator !=(StateObject<T, TState> left, StateObject<T, TState> right)
            => !left.Equals(right);

        public bool Equals(StateObject<T, TState> other)
            => EqualityComparer<T>.Default.Equals(Value, other.Value)
            && EqualityComparer<TState>.Default.Equals(State, other.State);

        public override bool Equals(object obj)
            => (obj is StateObject<T, TState>) && Equals((StateObject<T, TState>)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(Value?.GetHashCode() ?? 0, State?.GetHashCode() ?? 0);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (ReferenceEquals(other, null) || !(other is StateObject<T, TState>)) { return false; }

            var obj = (StateObject<T, TState>)other;

            return comparer.Equals(Value, obj.Value) && comparer.Equals(State, obj.State);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return HashCodeHelpers.Combine(comparer.GetHashCode(Value), comparer.GetHashCode(State));
        }
    }
}
