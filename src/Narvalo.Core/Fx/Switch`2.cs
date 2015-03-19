﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

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

        public abstract void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise);

        // Bind to the left value.
        public abstract Switch<TResult, TRight> Bind<TResult>(Func<TLeft, Switch<TResult, TRight>> leftSelectorM);

        // Bind to the right value.
        public abstract Switch<TLeft, TResult> Bind<TResult>(Func<TRight, Switch<TLeft, TResult>> rightSelectorM);

        // Map the left value.
        public abstract Switch<TResult, TRight> Map<TResult>(Func<TLeft, TResult> leftSelector);

        // Map the right value.
        public abstract Switch<TLeft, TResult> Map<TResult>(Func<TRight, TResult> rightSelector);

        public abstract TResult Match<TResult>(
            Func<TLeft, TResult> caseLeft,
            Func<TRight, TResult> caseRight,
            Func<TResult> otherwise);

        public abstract Switch<TRight, TLeft> Swap();

        public abstract TLeft LeftOrThrow(Exception exception);

        public abstract TLeft LeftOrThrow(Func<Exception> exceptionFactory);

        public abstract TRight RightOrThrow(Exception exception);

        public abstract TRight RightOrThrow(Func<Exception> exceptionFactory);

        public TLeft LeftOrDefault()
        {
            return Match(Stubs<TLeft>.Identity, Stubs<TRight, TLeft>.AlwaysDefault, Stubs<TLeft>.AlwaysDefault);
        }

        public TLeft LeftOrElse(TLeft other)
        {
            return Match(Stubs<TLeft>.Identity, _ => other, Stubs<TLeft>.AlwaysDefault);
        }

        public TLeft LeftOrElse(Func<TRight, TLeft> valueFactory)
        {
            Contract.Requires(valueFactory != null);

            return Match(Stubs<TLeft>.Identity, valueFactory, Stubs<TLeft>.AlwaysDefault);
        }

        public TRight RightOrDefault()
        {
            return Match(Stubs<TLeft, TRight>.AlwaysDefault, Stubs<TRight>.Identity, Stubs<TRight>.AlwaysDefault);
        }

        public TRight RightOrElse(TRight other)
        {
            return Match(_ => other, Stubs<TRight>.Identity, Stubs<TRight>.AlwaysDefault);
        }

        public TRight RightOrElse(Func<TLeft, TRight> valueFactory)
        {
            Contract.Requires(valueFactory != null);

            return Match(valueFactory, Stubs<TRight>.Identity, Stubs<TRight>.AlwaysDefault);
        }
    }

    /// <content>
    /// Implements the empty <see cref="Switch{TLeft, TRight}"/>.
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

            public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
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

            public override TLeft LeftOrThrow(Exception exception)
            {
                Require.NotNull(exception, "exception");

                throw exception;
            }

            public override TLeft LeftOrThrow(Func<Exception> exceptionFactory)
            {
                Require.NotNull(exceptionFactory, "exceptionFactory");

                throw exceptionFactory.Invoke();
            }

            public override TRight RightOrThrow(Exception exception)
            {
                Require.NotNull(exception, "exception");

                throw exception;
            }

            public override TRight RightOrThrow(Func<Exception> exceptionFactory)
            {
                Require.NotNull(exceptionFactory, "exceptionFactory");

                throw exceptionFactory.Invoke();
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
    /// Implements the left side of the <see cref="Switch{TLeft, TRight}"/> type.
    /// </content>
    public abstract partial class Switch<TLeft, TRight>
    {
        /// <summary>
        /// Represents the left side of the <see cref="Switch{TLeft, TRight}"/> type.
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

            /// <copydoc cref="Switch{TLeft, TRight}.Apply" />
            public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseLeft, "caseLeft");

                caseLeft.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Apply" />
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

            public override TLeft LeftOrThrow(Exception exception)
            {
                return _value;
            }

            public override TLeft LeftOrThrow(Func<Exception> exceptionFactory)
            {
                return _value;
            }

            public override TRight RightOrThrow(Exception exception)
            {
                Require.NotNull(exception, "exception");

                throw exception;
            }

            public override TRight RightOrThrow(Func<Exception> exceptionFactory)
            {
                Require.NotNull(exceptionFactory, "exceptionFactory");

                throw exceptionFactory.Invoke();
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Left({0})", _value);
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

        /// <content>
        /// Implements the <see cref="IEquatable{Left}"/> interface.
        /// </content>
        internal partial class Left
        {
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
        }
    }

    /// <content>
    /// Implements the right side of the <see cref="Switch{TLeft, TRight}"/> type.
    /// </content>
    public abstract partial class Switch<TLeft, TRight>
    {
        /// <summary>
        /// Represents the right side of the <see cref="Switch{TLeft, TRight}"/> type.
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

            /// <copydoc cref="Switch{TLeft, TRight}.Apply" />
            public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
            {
                Require.NotNull(caseRight, "caseRight");

                caseRight.Invoke(_value);
            }

            /// <copydoc cref="Switch{TLeft, TRight}.Apply" />
            public override TResult Match<TResult>(
                Func<TLeft, TResult> caseLeft,
                Func<TRight, TResult> caseRight,
                Func<TResult> otherwise)
            {
                Require.NotNull(caseRight, "caseRight");

                return caseRight.Invoke(_value);
            }

            public override TLeft LeftOrThrow(Exception exception)
            {
                Require.NotNull(exception, "exception");

                throw exception;
            }

            public override TLeft LeftOrThrow(Func<Exception> exceptionFactory)
            {
                Require.NotNull(exceptionFactory, "exceptionFactory");

                throw exceptionFactory.Invoke();
            }

            public override TRight RightOrThrow(Exception exception)
            {
                return _value;
            }

            public override TRight RightOrThrow(Func<Exception> exceptionFactory)
            {
                return _value;
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Right({0})", _value);
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

        /// <content>
        /// Implements the <see cref="IEquatable{Right}"/> interface.
        /// </content>
        internal partial class Right
        {
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
        }
    }

#if CONTRACTS_FULL

    [ContractClass(typeof(SwitchContract<,>))]
    public abstract partial class Switch<TLeft, TRight>
    {
        internal partial class Left
        {
            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_value != null);
            }
        }

        internal partial class Right
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
        public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight, Action otherwise)
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

        public override TLeft LeftOrThrow(Exception exception)
        {
            Contract.Requires(exception != null);

            return default(TLeft);
        }

        public override TLeft LeftOrThrow(Func<Exception> exceptionFactory)
        {
            Contract.Requires(exceptionFactory != null);

            return default(TLeft);
        }

        public override TRight RightOrThrow(Exception exception)
        {
            Contract.Requires(exception != null);

            return default(TRight);
        }

        public override TRight RightOrThrow(Func<Exception> exceptionFactory)
        {
            Contract.Requires(exceptionFactory != null);

            return default(TRight);
        }
    }

#endif
}