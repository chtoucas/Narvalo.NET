// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class StateObject
    {
        public static StateObject<T, TState> Create<T, TState>(T value, TState state)
            => new StateObject<T, TState>(value, state);
    }

    // To be replaced by ValueTuple<T, TState> when available.
    public partial struct StateObject<T, TState> : IEquatable<StateObject<T, TState>>, IStructuralEquatable
    {
        public StateObject(T result, TState state)
        {
            Result = result;
            State = state;
        }

        public T Result { get; }

        public TState State { get; }

        public override string ToString() => "Result=" + Result?.ToString() + "; State=" + State?.ToString();
    }

    // Implements the IEquatable<StateObject<T, TState>> and IStructuralEquatable interfaces.
    public partial struct StateObject<T, TState>
    {
        public static bool operator ==(StateObject<T, TState> left, StateObject<T, TState> right)
            => left.Equals(right);
        public static bool operator !=(StateObject<T, TState> left, StateObject<T, TState> right)
            => !left.Equals(right);

        public bool Equals(StateObject<T, TState> other)
            => EqualityComparer<T>.Default.Equals(Result, other.Result)
            && EqualityComparer<TState>.Default.Equals(State, other.State);

        public override bool Equals(object obj)
            => (obj is StateObject<T, TState>) && Equals((StateObject<T, TState>)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(Result?.GetHashCode() ?? 0, State?.GetHashCode() ?? 0);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (ReferenceEquals(other, null) || !(other is StateObject<T, TState>)) { return false; }

            var obj = (StateObject<T, TState>)other;

            return comparer.Equals(Result, obj.Result) && comparer.Equals(State, obj.State);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return HashCodeHelpers.Combine(comparer.GetHashCode(Result), comparer.GetHashCode(State));
        }
    }
}
