// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Monads
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the trivial monad. Pretty useless.
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

    /// <content>
    /// Implements the <see cref="IEquatable{T}"/> interface.
    /// </content>
    public partial struct Identity<T>
    {
        public static bool operator ==(Identity<T> left, Identity<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identity<T> left, Identity<T> right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Identity<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Identity<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (Value == null)
            {
                return other.Value == null;
            }

            return comparer.Equals(Value, other.Value);
        }

        public bool Equals(T other)
        {
            return Equals(η(other), EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Contract.Requires(comparer != null);

            return Equals(η(other), comparer);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, EqualityComparer<T>.Default);
        }

        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

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
        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            return Value != null ? comparer.GetHashCode(Value) : 0;
        }
    }

    /// <content>
    /// Provides the core Monad methods.
    /// </content>
    public partial struct Identity<T>
    {
        public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return selector.Invoke(Value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<T> η(T value)
        {
            return new Identity<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<T> μ(Identity<Identity<T>> square)
        {
            return square.Value;
        }
    }

    /// <content>
    /// Provides the core Comonad methods.
    /// </content>
    public partial struct Identity<T>
    {
        public Identity<TResult> Extend<TResult>(Func<Identity<T>, TResult> fun)
        {
            Require.NotNull(fun, "fun");

            return new Identity<TResult>(fun.Invoke(this));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static T ε(Identity<T> monad)
        {
            return monad.Value;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<Identity<T>> δ(Identity<T> monad)
        {
            return new Identity<Identity<T>>(monad);
        }
    }
}
