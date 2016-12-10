// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Represents the (possibly empty) sum of two types. An instance of the
    /// <see cref="Switch{TLeft, TRight}"/> class contains either a <c>TLeft</c>
    /// value or a <c>TRight</c> value or nothing.
    /// </summary>
    /// <remarks>Any enclosed value is not <see langword="null"/>.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    /// <seealso cref="Outcome{T}"/>
    /// <seealso cref="Either{T1, T2}"/>
    /// <seealso cref="VoidOrBreak"/>
    /// <seealso cref="VoidOrError"/>
    public abstract partial class Switch<TLeft, TRight>
    {
        private static readonly Switch<TLeft, TRight> s_Empty = new Switch<TLeft, TRight>.Empty_();

#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
        protected Switch() { }
#else
        private Switch() { }
#endif

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Switch<TLeft, TRight> Empty
        {
            get
            {
                Ensures(Result<Switch<TLeft, TRight>>() != null);

                return s_Empty;
            }
        }

        public abstract void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise);

        // Bind to the left value.
        public abstract Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM);

        // Bind to the right value.
        public abstract Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM);

        // Map the left value.
        public abstract Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector);

        // Map the right value.
        public abstract Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector);

        public abstract TResult Map<TResult>(
            Func<TLeft, TResult> caseLeft,
            Func<TRight, TResult> caseRight,
            Func<TResult> otherwise);

        public abstract Switch<TRight, TLeft> Swap();

        public abstract Maybe<TLeft> LeftOrNone();

        public abstract Maybe<TRight> RightOrNone();

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Switch<TLeft, TRight> η(TLeft value)
        {
            Ensures(Result<Switch<TLeft, TRight>>() != null);

            return value != null ? new Left_(value) : Switch<TLeft, TRight>.Empty;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Switch<TLeft, TRight> η(TRight value)
        {
            Ensures(Result<Switch<TLeft, TRight>>() != null);

            return value != null ? new Right_(value) : Switch<TLeft, TRight>.Empty;
        }

        private sealed class Empty_ : Switch<TLeft, TRight>, IEquatable<Empty_>
        {
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
                => Switch<TResult, TRight>.Empty;

            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
                => Switch<TLeft, TResult>.Empty;

            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
                => Switch<TResult, TRight>.Empty;

            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
                => Switch<TLeft, TResult>.Empty;

            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                otherwise.Invoke();
            }

            public override TResult Map<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                return otherwise.Invoke();
            }

            public override Switch<TRight, TLeft> Swap() => Switch<TRight, TLeft>.Empty;

            public override Maybe<TLeft> LeftOrNone() => Maybe<TLeft>.None;

            public override Maybe<TRight> RightOrNone() => Maybe<TRight>.None;

            public override string ToString()
            {
                Ensures(Result<string>() != null);

                return "Either(Empty)";
            }

            public bool Equals(Empty_ other) => true;

            public override bool Equals(object obj) => obj is Empty_;

            public override int GetHashCode() => 0;
        }

        /// <summary>
        /// Represents the left side of the <see cref="Switch{TLeft, TRight}"/> type.
        /// </summary>
        private sealed partial class Left_ : Switch<TLeft, TRight>, IEquatable<Left_>
        {
            private readonly TLeft _value;

            public Left_(TLeft value)
            {
                Demand.NotNullUnconstrained(value);

                _value = value;
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TLeft, Switch{TResult, TRight}})" />
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                Require.NotNull(leftSelectorM, nameof(leftSelectorM));

                return leftSelectorM.Invoke(_value);
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TRight, Switch{TLeft, TResult}})" />
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
                => new Switch<TLeft, TResult>.Left_(_value);

            /// <inheritdoc cref="Switch{TLeft, TRight}.Map{TResult}(Func{TLeft, TResult})" />
            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                Require.NotNull(leftSelector, nameof(leftSelector));

                return Switch<TResult, TRight>.η(leftSelector.Invoke(_value));
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Map{TResult}(Func{TRight, TResult})" />
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
                => new Switch<TLeft, TResult>.Left_(_value);

            /// <inheritdoc cref="Switch{TLeft, TRight}.Invoke" />
            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                caseLeft.Invoke(_value);
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Invoke" />
            public override TResult Map<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                return caseLeft.Invoke(_value);
            }

            /// <inheritdoc cref="Switch{TRight, TLeft}.Swap" />
            public override Switch<TRight, TLeft> Swap() => Switch<TRight, TLeft>.η(_value);

            public override Maybe<TLeft> LeftOrNone() => Maybe.Of(_value);

            public override Maybe<TRight> RightOrNone() => Maybe<TRight>.None;

            public bool Equals(Left_ other)
            {
                if (other == this)
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

                return EqualityComparer<TLeft>.Default.Equals(_value, other._value);
            }

            public override bool Equals(object obj) => Equals(obj as Left_);

            public override int GetHashCode() => EqualityComparer<TLeft>.Default.GetHashCode(_value);

            public override string ToString()
            {
                Ensures(Result<string>() != null);

                return Format.Current("Left({0})", _value);
            }
        }

        /// <summary>
        /// Represents the right side of the <see cref="Switch{TLeft, TRight}"/> type.
        /// </summary>
        private sealed partial class Right_ : Switch<TLeft, TRight>, IEquatable<Right_>
        {
            private readonly TRight _value;

            public Right_(TRight value)
            {
                Demand.NotNullUnconstrained(value);

                _value = value;
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TLeft, Switch{TResult, TRight}})" />
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
                => new Switch<TResult, TRight>.Right_(_value);

            /// <inheritdoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TRight, Switch{TLeft, TResult}})" />
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                Require.NotNull(rightSelectorM, nameof(rightSelectorM));

                return rightSelectorM.Invoke(_value);
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Map{TResult}(Func{TLeft, TResult})" />
            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
                => new Switch<TResult, TRight>.Right_(_value);

            /// <inheritdoc cref="Switch{TLeft, TRight}.Map{TResult}(Func{TRight, TResult})" />
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                Require.NotNull(rightSelector, nameof(rightSelector));

                return Switch<TLeft, TResult>.η(rightSelector.Invoke(_value));
            }

            /// <inheritdoc cref="Switch{TRight, TLeft}.Swap" />
            public override Switch<TRight, TLeft> Swap() => Switch<TRight, TLeft>.η(_value);

            /// <inheritdoc cref="Switch{TLeft, TRight}.Invoke" />
            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                caseRight.Invoke(_value);
            }

            /// <inheritdoc cref="Switch{TLeft, TRight}.Invoke" />
            public override TResult Map<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                return caseRight.Invoke(_value);
            }

            public override Maybe<TLeft> LeftOrNone() => Maybe<TLeft>.None;

            public override Maybe<TRight> RightOrNone() => Maybe.Of(_value);

            public bool Equals(Right_ other)
            {
                if (other == this)
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

                return EqualityComparer<TRight>.Default.Equals(_value, other._value);
            }

            public override bool Equals(object obj) => Equals(obj as Right_);

            public override int GetHashCode() => EqualityComparer<TRight>.Default.GetHashCode(_value);

            public override string ToString()
            {
                Ensures(Result<string>() != null);

                return Format.Current("Right({0})", _value);
            }
        }
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    [ContractClass(typeof(SwitchContract<,>))]
    public partial class Switch<TLeft, TRight>
    {
        private partial class Left_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_value != null);
            }
        }

        private partial class Right_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_value != null);
            }
        }
    }

    [ContractClassFor(typeof(Switch<,>))]
    internal abstract class SwitchContract<TLeft, TRight> : Switch<TLeft, TRight>
    {
        public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);
            Contract.Requires(otherwise != null);
        }

        public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
        {
            Contract.Requires(leftSelectorM != null);

            return default(Switch<TResult, TRight>);
        }

        public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
        {
            Contract.Requires(rightSelectorM != null);

            return default(Switch<TLeft, TResult>);
        }

        public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
        {
            Contract.Requires(leftSelector != null);

            return default(Switch<TResult, TRight>);
        }

        public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
        {
            Contract.Requires(rightSelector != null);

            return default(Switch<TLeft, TResult>);
        }

        public override TResult Map<TResult>(
            Func<TLeft, TResult> caseLeft,
            Func<TRight, TResult> caseRight,
            Func<TResult> otherwise)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);
            Contract.Requires(otherwise != null);

            return default(TResult);
        }

        public override Switch<TRight, TLeft> Swap()
        {
            Ensures(Result<Switch<TRight, TLeft>>() != null);

            return default(Switch<TRight, TLeft>);
        }

        public override Maybe<TLeft> LeftOrNone() => default(Maybe<TLeft>);

        public override Maybe<TRight> RightOrNone() => default(Maybe<TRight>);
    }

#endif
}
