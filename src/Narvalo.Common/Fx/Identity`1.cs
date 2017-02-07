// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the trivial monad.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    public partial struct Identity<T> : IEquatable<Identity<T>>, IEquatable<T>
    {
        private readonly T _value;

        private Identity(T value)
        {
            _value = value;
        }

        internal T Value { get { return _value; } }
    }

    // Implements the IEquatable<T> interface.
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

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<T> η(T value) => new Identity<T>(value);

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<T> μ(Identity<Identity<T>> square) => square.Value;
    }

    // Provides the core Comonad methods.
    public partial struct Identity<T>
    {
        public Identity<TResult> Extend<TResult>(Func<Identity<T>, TResult> fun)
        {
            Require.NotNull(fun, nameof(fun));

            return new Identity<TResult>(fun.Invoke(this));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static T ε(Identity<T> monad) => monad.Value;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<Identity<T>> δ(Identity<T> monad) => new Identity<Identity<T>>(monad);
    }
}
