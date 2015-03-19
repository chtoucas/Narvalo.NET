// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the (possibly empty) sum of two types. 
    /// An instance of the <see cref="Switch{TLeft, TRight}"/> class contains 
    /// either a TLeft value or a TRight value or nothing.
    /// </summary>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    public abstract partial class Switch<TLeft, TRight>
    {
        private static readonly Switch<TLeft, TRight> s_Empty = new Switch<TLeft, TRight>.Empty_();

        protected Switch() { }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Switch<TLeft, TRight> Empty
        {
            get
            {
                Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

                return s_Empty;
            }
        }

        // Bind to the left value.
        public abstract Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM);

        // Bind to the right value.
        public abstract Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM);

        // Map the left value.
        public abstract Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector);

        // Map the right value.
        public abstract Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector);

        public abstract void Match(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise);

        public abstract TResult Match<TResult>(
            Func<TLeft, TResult> caseLeft,
            Func<TRight, TResult> caseRight,
            Func<TResult> otherwise);

        public abstract Switch<TRight, TLeft> Swap();

        public TLeft LeftOrDefault()
        {
            return LeftOrElse(Stubs<TRight, TLeft>.AlwaysDefault);
        }

        public TLeft LeftOrElse(Func<TRight, TLeft> other)
        {
            Contract.Requires(other != null);

            return Match(Stubs<TLeft>.Identity, other, Stubs<TLeft>.AlwaysDefault);
        }

        public TRight RightOrDefault()
        {
            return RightOrElse(Stubs<TLeft, TRight>.AlwaysDefault);
        }

        public TRight RightOrElse(Func<TLeft, TRight> other)
        {
            Contract.Requires(other != null);

            return Match(other, Stubs<TRight>.Identity, Stubs<TRight>.AlwaysDefault);
        }
    }

    /// <content>
    /// Implements the empty switch.
    /// </content>
    public abstract partial class Switch<TLeft, TRight>
    {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        private sealed class Empty_ : Switch<TLeft, TRight>, IEquatable<Empty_>
        {
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                return Switch<TResult, TRight>.Empty;
            }

            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                return Switch<TLeft, TResult>.Empty;
            }

            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                return Switch<TResult, TRight>.Empty;
            }

            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                return Switch<TLeft, TResult>.Empty;
            }

            public override void Match(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(otherwise, "otherwise");

                otherwise.Invoke();
            }

            public override TResult Match<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(otherwise, "otherwise");

                return otherwise.Invoke();
            }

            public override Switch<TRight, TLeft> Swap()
            {
                return Switch<TRight, TLeft>.Empty;
            }

            public override string ToString()
            {
                return "Either(Empty)";
            }

            public bool Equals(Empty_ other)
            {
                return true;
            }

            public override bool Equals(object obj)
            {
                return obj is Empty_;
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }
    }

    /// <content>
    /// Implements the left side of the switch type.
    /// </content>
    public abstract partial class Switch<TLeft, TRight>
    {
        /// <summary>
        /// Represents the left side of the switch type.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        internal sealed partial class Left : Switch<TLeft, TRight>, IEquatable<Left>
        {
            private readonly TLeft _value;

            private Left(TLeft value)
            {
                Contract.Requires(value != null);

                _value = value;
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TLeft, Switch{TResult, TRight}})" />
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                Require.NotNull(leftSelectorM, "leftSelectorM");

                return leftSelectorM.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TRight, Switch{TLeft, TResult}})" />
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                return new Switch<TLeft, TResult>.Left(_value);
            }

            /// <copydoc cref="Switch{TResult, TRight}.Map{TResult}(Func{TLeft, TResult})" />
            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                Require.NotNull(leftSelector, "leftSelector");

                return Switch<TResult, TRight>.Left.η(leftSelector.Invoke(_value));
            }

            /// <copydoc cref="Switch{TLeft, TResult}.Map{TResult}(Func{TRight, TResult})" />
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                return new Switch<TLeft, TResult>.Left(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Match" />
            public override void Match(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseLeft, "caseLeft");

                caseLeft.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Match" />
            public override TResult Match<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(caseLeft, "caseLeft");

                return caseLeft.Invoke(_value);
            }

            /// <copydoc cref="Switch{TRight, TLeft}.Swap" />
            public override Switch<TRight, TLeft> Swap()
            {
                return Switch<TRight, TLeft>.Right.η(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Left({0})", _value);
            }

            public bool Equals(Left other)
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

            public override bool Equals(object obj)
            {
                return Equals(obj as Left);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TLeft>.Default.GetHashCode(_value);
            }

            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
                Justification = "Standard naming convention from mathematics. Only used internally.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static Switch<TLeft, TRight> η(TLeft value)
            {
                Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

                return value != null ? new Left(value) : Switch<TLeft, TRight>.Empty;
            }
        }
    }

    /// <content>
    /// Implements the right side of the switch type.
    /// </content>
    public abstract partial class Switch<TLeft, TRight>
    {
        /// <summary>
        /// Represents the right side of the switch type.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        internal sealed partial class Right : Switch<TLeft, TRight>, IEquatable<Right>
        {
            private readonly TRight _value;

            private Right(TRight value)
            {
                Contract.Requires(value != null);

                _value = value;
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TLeft, Switch{TResult, TRight}})" />
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                return new Switch<TResult, TRight>.Right(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TRight, Switch{TLeft, TResult}})" />
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                Require.NotNull(rightSelectorM, "rightSelectorM");

                return rightSelectorM.Invoke(_value);
            }

            /// <copydoc cref="Switch{TResult, TRight}.Map{TResult}(Func{TLeft, TResult})" />
            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                return new Switch<TResult, TRight>.Right(_value);
            }

            /// <copydoc cref="Switch{TLeft, TResult}.Map{TResult}(Func{TRight, TResult})" />
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                Require.NotNull(rightSelector, "rightSelector");

                return Switch<TLeft, TResult>.Right.η(rightSelector.Invoke(_value));
            }

            /// <copydoc cref="Switch{TRight, TLeft}.Swap" />
            public override Switch<TRight, TLeft> Swap()
            {
                return Switch<TRight, TLeft>.Left.η(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Match" />
            public override void Match(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseRight, "caseRight");

                caseRight.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Match" />
            public override TResult Match<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(caseRight, "caseRight");

                return caseRight.Invoke(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Right({0})", _value);
            }

            public bool Equals(Right other)
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

            public override bool Equals(object obj)
            {
                return Equals(obj as Right);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TRight>.Default.GetHashCode(_value);
            }

            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
                Justification = "Standard naming convention from mathematics. Only used internally.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static Switch<TLeft, TRight> η(TRight value)
            {
                Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

                return value != null ? new Right(value) : Switch<TLeft, TRight>.Empty;
            }
        }
    }

#if CONTRACTS_FULL

    [ContractClass(typeof(WitchContract<,>))]
    public abstract partial class Switch<TLeft, TRight>
    {
        internal sealed partial class Left
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_value != null);
            }
        }

        internal sealed partial class Right
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_value != null);
            }
        }
    }

    [ContractClassFor(typeof(Switch<,>))]
    internal abstract class WitchContract<TLeft, TRight> : Switch<TLeft, TRight>
    {
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

        public override void Match(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);
            Contract.Requires(otherwise != null);
        }

        public override TResult Match<TResult>(
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
            Contract.Ensures(Contract.Result<Switch<TRight, TLeft>>() != null);

            return default(Switch<TRight, TLeft>);
        }
    }

#endif
}
