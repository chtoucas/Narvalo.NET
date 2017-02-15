// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the trivial monad.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    public partial struct Identity<T> : IEquatable<Identity<T>>, IEquatable<T>, Internal.IHooks<T>
    {
        public Identity(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    // Implements the IEquatable<Identity<T>> and IEquatable<T> interfaces.
    public partial struct Identity<T>
    {
        public static bool operator ==(Identity<T> left, Identity<T> right) => left.Equals(right);

        public static bool operator !=(Identity<T> left, Identity<T> right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Identity<T> other) => Equals(other, EqualityComparer<T>.Default);

        public bool Equals(Identity<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (Value == null)
            {
                return other.Value == null;
            }

            return comparer.Equals(Value, other.Value);
        }

        public bool Equals(T other) => Equals(η(other), EqualityComparer<T>.Default);

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Expect.NotNull(comparer);

            return Equals(η(other), comparer);
        }

        public override bool Equals(object obj) => Equals(obj, EqualityComparer<T>.Default);

        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (other is Identity<T>)
            {
                return Equals((Identity<T>)other, comparer);
            }

            if (other is T)
            {
                return Equals((T)other, comparer);
            }

            return false;
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode() => GetHashCode(EqualityComparer<T>.Default);

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return Value != null ? comparer.GetHashCode(Value) : 0;
        }
    }

    // Provides the core Monad methods.
    public partial struct Identity<T>
    {
        public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(Value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Identity<T> η(T value) => new Identity<T>(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Identity<T> μ(Identity<Identity<T>> square) => square.Value;
    }

    // Provides the core Comonad methods.
    public partial struct Identity<T>
    {
        public Identity<TResult> Extend<TResult>(Func<Identity<T>, TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));

            return new Identity<TResult>(thunk.Invoke(this));
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T ε(Identity<T> monad) => monad.Value;

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Identity<Identity<T>> δ(Identity<T> monad) => new Identity<Identity<T>>(monad);
    }

    // Implements the Internal.IHooks<T> interface.
    public partial struct Identity<T>
    {
        public void Do(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            action.Invoke(Value);
        }

        public void Do(Func<T, bool> predicate, Action<T> action, Action otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));
            Require.NotNull(otherwise, nameof(otherwise));

            if (predicate.Invoke(Value))
            {
                action.Invoke(Value);
            }
            else
            {
                otherwise.Invoke();
            }
        }

        public void When(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (predicate.Invoke(Value)) { action.Invoke(Value); }
        }
    }
}
