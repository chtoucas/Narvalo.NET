// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the (possibly empty) sum of two types. An instance of the 
    /// <see cref="Switch{TLeft, TRight}"/> class contains either a <c>TLeft</c>
    /// value or a <c>TRight</c> value or nothing.
    /// </summary>
    /// <remarks>Any enclosed value is not <see langword="null"/>.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
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
                Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

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
            Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

            return value != null ? new Left_(value) : Switch<TLeft, TRight>.Empty;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
            Justification = "[Ignore] Weird. This method does have callers inside the assembly.")]
        internal static Switch<TLeft, TRight> η(TRight value)
        {
            Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

            return value != null ? new Right_(value) : Switch<TLeft, TRight>.Empty;
        }

        private sealed class Empty_ : Switch<TLeft, TRight>, IEquatable<Empty_>
        {
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                return Switch<TResult, TRight>.Empty;
            }

            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                return Switch<TLeft, TResult>.Empty;
            }

            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                return Switch<TResult, TRight>.Empty;
            }

            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                return Switch<TLeft, TResult>.Empty;
            }

            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(otherwise, "otherwise");

                otherwise.Invoke();
            }

            public override TResult Map<TResult>(
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

            public override Maybe<TLeft> LeftOrNone()
            {
                return Maybe<TLeft>.None;
            }

            public override Maybe<TRight> RightOrNone()
            {
                return Maybe<TRight>.None;
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

        /// <summary>
        /// Represents the left side of the <see cref="Switch{TLeft, TRight}"/> type.
        /// </summary>
        private sealed partial class Left_ : Switch<TLeft, TRight>, IEquatable<Left_>
        {
            private readonly TLeft _value;

            public Left_(TLeft value)
            {
                Contract.Requires(value != null);

                _value = value;
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TLeft, Switch{TResult, TRight}})" />
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                Require.NotNull(leftSelectorM, "leftSelectorM");

                return leftSelectorM.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TRight, Switch{TLeft, TResult}})" />
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                return new Switch<TLeft, TResult>.Left_(_value);
            }

            /// <copydoc cref="Switch{TResult, TRight}.Map{TResult}(Func{TLeft, TResult})" />
            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                Require.NotNull(leftSelector, "leftSelector");

                return Switch<TResult, TRight>.η(leftSelector.Invoke(_value));
            }

            /// <copydoc cref="Switch{TLeft, TResult}.Map{TResult}(Func{TRight, TResult})" />
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                return new Switch<TLeft, TResult>.Left_(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Invoke" />
            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseLeft, "caseLeft");

                caseLeft.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Invoke" />
            public override TResult Map<TResult>(
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
                return Switch<TRight, TLeft>.η(_value);
            }

            public override Maybe<TLeft> LeftOrNone()
            {
                return Maybe.Of(_value);
            }

            public override Maybe<TRight> RightOrNone()
            {
                return Maybe<TRight>.None;
            }

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

            public override bool Equals(object obj)
            {
                return Equals(obj as Left_);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TLeft>.Default.GetHashCode(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Left({0})", _value);
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
                Contract.Requires(value != null);

                _value = value;
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TLeft, Switch{TResult, TRight}})" />
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM)
            {
                return new Switch<TResult, TRight>.Right_(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Bind{TResult}(Func{TRight, Switch{TLeft, TResult}})" />
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM)
            {
                Require.NotNull(rightSelectorM, "rightSelectorM");

                return rightSelectorM.Invoke(_value);
            }

            /// <copydoc cref="Switch{TResult, TRight}.Map{TResult}(Func{TLeft, TResult})" />
            public override Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector)
            {
                return new Switch<TResult, TRight>.Right_(_value);
            }

            /// <copydoc cref="Switch{TLeft, TResult}.Map{TResult}(Func{TRight, TResult})" />
            [SuppressMessage("Gendarme.Rules.Naming", "ParameterNamesShouldMatchOverriddenMethodRule",
                Justification = "[Ignore] Weird. Parameter names do match.")]
            public override Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector)
            {
                Require.NotNull(rightSelector, "rightSelector");

                return Switch<TLeft, TResult>.η(rightSelector.Invoke(_value));
            }

            /// <copydoc cref="Switch{TRight, TLeft}.Swap" />
            public override Switch<TRight, TLeft> Swap()
            {
                return Switch<TRight, TLeft>.η(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Invoke" />
            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseRight, "caseRight");

                caseRight.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Invoke" />
            public override TResult Map<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(caseRight, "caseRight");

                return caseRight.Invoke(_value);
            }

            public override Maybe<TLeft> LeftOrNone()
            {
                return Maybe<TLeft>.None;
            }

            public override Maybe<TRight> RightOrNone()
            {
                return Maybe.Of(_value);
            }

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

            public override bool Equals(object obj)
            {
                return Equals(obj as Right_);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<TRight>.Default.GetHashCode(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Right({0})", _value);
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
            private void ObjectInvariants()
            {
                Contract.Invariant(_value != null);
            }
        }

        private partial class Right_
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
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
            Contract.Ensures(Contract.Result<Switch<TRight, TLeft>>() != null);

            return default(Switch<TRight, TLeft>);
        }

        public override Maybe<TLeft> LeftOrNone() 
        {
            Contract.Ensures(Contract.Result<Maybe<TLeft>>() != null);

            return default(Maybe<TLeft>);
        }

        public override Maybe<TRight> RightOrNone()
        {
            Contract.Ensures(Contract.Result<Maybe<TRight>>() != null);

            return default(Maybe<TRight>);
        }
    }

#endif
}
