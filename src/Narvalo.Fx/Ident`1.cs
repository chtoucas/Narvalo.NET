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

    // Provides the core Monad methods.
    public partial struct Ident<T>
    {
        public Ident<TResult> Bind<TResult>(Func<T, Ident<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return selector(Value);
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
        public Ident<TResult> Extend<TResult>(Func<Ident<T>, TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return new Ident<TResult>(func(this));
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
        public bool Contains(T value) => Equals(value);

        public bool Contains(T value, IEqualityComparer<T> comparer) => Equals(value, comparer);

        public void When(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (predicate(Value)) { action(Value); }
        }

        public void Do(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            action(Value);
        }

        #region In between IContainer<T> and IMaybe<T>.

        public TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(otherwise, nameof(otherwise));

            return predicate(Value) ? selector(Value) : otherwise();
        }

        public TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return predicate(Value) ? thenResult : elseResult;
        }

        public void When(Func<T, bool> predicate, Action<T> action, Action otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));
            Require.NotNull(otherwise, nameof(otherwise));

            if (predicate(Value)) { action(Value); } else { otherwise(); }
        }

        #endregion
    }

    // Implements the Internal.Iterable<TError> interface.
    public partial struct Ident<T>
    {
        public IEnumerable<T> ToEnumerable() => Sequence.Of(Value);

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();
    }

    // Implements the IEquatable<Ident<T>> and IEquatable<T> interfaces.
    public partial struct Ident<T>
    {
        public static bool operator ==(Ident<T> left, Ident<T> right) => left.Equals(right);

        public static bool operator !=(Ident<T> left, Ident<T> right) => !left.Equals(right);

        public bool Equals(Ident<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

        public bool Equals(Ident<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.Equals(Value, other.Value);
        }

        public bool Equals(T other) => EqualityComparer<T>.Default.Equals(Value, other);

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.Equals(Value, other);
        }

        public override bool Equals(object obj)
        {
            if (obj is Ident<T>) { return Equals((Ident<T>)obj); }
            if (obj is T) { return Equals((T)obj); }
            return false;
        }

        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            if (other is Ident<T>) { return Equals((Ident<T>)other, comparer); }
            if (other is T) { return Equals((T)other, comparer); }
            return false;
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.GetHashCode(Value);
        }
    }
}
