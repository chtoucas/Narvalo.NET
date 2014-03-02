// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Fx;

    public partial class Identity<T>
    {
        public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return selector.Invoke(_value);
        }

        public Identity<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return new Identity<TResult>(selector.Invoke(_value));
        }

        public Identity<TResult> Then<TResult>(Identity<TResult> other)
        {
            return other;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter")]
        internal static Identity<T> η(T value)
        {
            return new Identity<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter")]
        internal static Identity<T> μ(Identity<Identity<T>> square)
        {
            Require.NotNull(square, "square");

            return square._value;
        }

        public Identity<TResult> Coalesce<TResult>(
            Func<T, bool> predicate,
            Identity<TResult> then,
            Identity<TResult> otherwise)
        {
            Require.NotNull(predicate, "predicate");

            return predicate.Invoke(Value) ? then : otherwise;
        }

        public Identity<T> Run(Action<T> action)
        {
            Require.NotNull(action, "action");

            action.Invoke(Value);

            return this;
        }
    }
}
