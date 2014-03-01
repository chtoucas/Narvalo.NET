// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class Maybe<T>
    {
        static readonly Maybe<T> None_ = new Maybe<T>();

        #region Monoid

        /// <summary>
        /// Returns an instance of <see cref="Narvalo.Fx.Maybe&lt;T&gt;" /> that does not hold any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        public static Maybe<T> None { get { return None_; } }

        public Maybe<T> OrElse(Maybe<T> other)
        {
            return IsNone ? other : this;
        }

        #endregion

        #region Monad

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return IsSome ? selector.Invoke(Value) : Maybe<TResult>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Maybe<T> η(T value)
        {
            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            Require.NotNull(square, "square");

            return square.IsSome ? square.Value : Maybe<T>.None;
        }

        #endregion

        #region Basic Monad functions

        public Maybe<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return IsSome ? Maybe<TResult>.η(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            return IsSome ? other : Maybe<TResult>.None;
        }
        
        #endregion

        #region Monadic lifting operators

        public Maybe<TResult> Zip<TSecond, TResult>(
            Maybe<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Require.NotNull(second, "second");

            return IsSome && second.IsSome
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, second.Value))
                : Maybe<TResult>.None;
        }

        #endregion

        #region Linq extensions

        public Maybe<TResult> Join<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            if (IsNone || inner.IsNone) {
                return Maybe<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, inner.Value))
                : Maybe<TResult>.None;
        }

        public Maybe<TResult> GroupJoin<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            if (IsNone) {
                return Maybe<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, inner))
                : Maybe<TResult>.None;
        }

        #endregion

        #region Non-standard extensions

        public Maybe<T> Run(Action<T> action)
        {
            Require.NotNull(action, "action");

            if (IsSome) {
                action.Invoke(Value);
            }

            return this;
        }

        public Maybe<T> OnNone(Action action)
        {
            Require.NotNull(action, "action");

            if (IsNone) {
                action.Invoke();
            }

            return this;
        }

        #endregion

        public Maybe<T> OnSome(Action<T> action)
        {
            return Run(action);
        }
    }
}
