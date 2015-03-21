// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the trivial monad. Useless in the context of C#.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    public sealed partial class Identity<T> : IEquatable<Identity<T>>, IEquatable<T>
    {
        private readonly T _value;

        private Identity(T value)
        {
            _value = value;
        }

        public T Value { get { return _value; } }
    }

    /// <content>
    /// Implements the <see cref="IEquatable{T}"/> interface.
    /// </content>
    public partial class Identity<T>
    {
        public bool Equals(Identity<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Identity<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (ReferenceEquals(other, null))
            {
                return Value == null;
            }

            if (Value == null)
            {
                return other.Value == null;
            }

            return comparer.Equals(Value, other.Value);
        }

        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
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

            if (other == null)
            {
                return Value == null;
            }

            if (other is T)
            {
                return Equals((T)other, comparer);
            }

            // Usually, we test the condition obj.GetType() == this.GetType(), in case "this" or "obj" 
            // is an instance of a derived type, something that can not happen here because Identity is sealed.
            return Equals(other as Identity<T>, comparer);
        }

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
    public partial class Identity<T>
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
            Contract.Ensures(Contract.Result<Identity<T>>() != null);

            return new Identity<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<T> μ(Identity<Identity<T>> square)
        {
            Require.NotNull(square, "square");

            return square.Value;
        }
    }

    /// <content>
    /// Provides the core Comonad methods.
    /// </content>
    public partial class Identity<T>
    {
        public Identity<TResult> Extend<TResult>(Func<Identity<T>, TResult> fun)
        {
            Require.NotNull(fun, "fun");
            Contract.Ensures(Contract.Result<Identity<TResult>>() != null);

            return new Identity<TResult>(fun.Invoke(this));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static T ε(Identity<T> monad)
        {
            Require.NotNull(monad, "monad");

            return monad.Value;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Identity<Identity<T>> δ(Identity<T> monad)
        {
            Contract.Ensures(Contract.Result<Identity<Identity<T>>>() != null);

            return new Identity<Identity<T>>(monad);
        }
    }
}
