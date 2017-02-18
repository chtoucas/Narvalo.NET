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
    public partial struct Ident<T>
        : IEquatable<Ident<T>>, IEquatable<T>, Internal.IContainer<T>, Internal.Iterable<T>
    {
        public Ident(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    // Implements the IEquatable<Ident<T>> and IEquatable<T> interfaces.
    public partial struct Ident<T>
    {
        public static bool operator ==(Ident<T> left, Ident<T> right) => left.Equals(right);

        public static bool operator !=(Ident<T> left, Ident<T> right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Ident<T> other) => Equals(other, EqualityComparer<T>.Default);

        public bool Equals(Ident<T> other, IEqualityComparer<T> comparer)
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

            if (other is Ident<T>)
            {
                return Equals((Ident<T>)other, comparer);
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
    public partial struct Ident<T>
    {
        public Ident<TResult> Bind<TResult>(Func<T, Ident<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(Value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Ident<T> η(T value) => new Ident<T>(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Ident<T> μ(Ident<Ident<T>> square) => square.Value;
    }

    // Provides the core Comonad methods.
    public partial struct Ident<T>
    {
        public Ident<TResult> Extend<TResult>(Func<Ident<T>, TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));

            return new Ident<TResult>(thunk.Invoke(this));
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T ε(Ident<T> value) => value.Value;

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Ident<Ident<T>> δ(Ident<T> value) => new Ident<Ident<T>>(value);
    }

    // Implements the Internal.IContainer<T> interface.
    public partial struct Ident<T>
    {
        public TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(otherwise, nameof(otherwise));

            return predicate.Invoke(Value) ? selector.Invoke(Value) : otherwise.Invoke();
        }

        public TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return predicate.Invoke(Value) ? thenResult : elseResult;
        }

        public void When(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (predicate.Invoke(Value)) { action.Invoke(Value); }
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

        public void Do(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            action.Invoke(Value);
        }
    }

    // Implements the Internal.IMaybe<TError> interface.
    public partial struct Ident<T>
    {
        public IEnumerable<T> ToEnumerable()
        {
            Warrant.NotNull<IEnumerator<T>>();

            return Sequence.Of(Value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Warrant.NotNull<IEnumerator<T>>();

            return ToEnumerable().GetEnumerator();
        }
    }
}
