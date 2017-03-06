// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // ValueTuple<T, TState>
    public partial struct StateObject<T, TState>
        : IEquatable<StateObject<T, TState>>, IStructuralEquatable,
            IComparable<StateObject<T, TState>>, IComparable, IStructuralComparable
    {
        public StateObject(T value, TState result)
        {
            Result = value;
            State = result;
        }

        public T Result { get; }

        public TState State { get; }

        public override string ToString() => "Result=" + Result.ToString() + "; State=" + State.ToString();
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

    // Implements the IComparable<StateObject<T, TState>>, IComparable and IStructuralComparable interfaces.
    public partial struct StateObject<T, TState>
    {
        public int CompareTo(StateObject<T, TState> other)
        {
            int c = Comparer<T>.Default.Compare(Result, other.Result);
            if (c != 0) { return c; }

            return Comparer<TState>.Default.Compare(State, other.State);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is StateObject<T, TState>))
            {
                throw new ArgumentException("XXX", nameof(obj));
            }

            return CompareTo((StateObject<T, TState>)obj);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null) { return 1; }

            if (!(other is StateObject<T, TState>))
            {
                throw new ArgumentException("XXX", nameof(other));
            }

            var obj = (StateObject<T, TState>)other;

            int c = comparer.Compare(Result, obj.Result);
            if (c != 0) { return c; }

            return comparer.Compare(State, obj.State);
        }
    }
}
