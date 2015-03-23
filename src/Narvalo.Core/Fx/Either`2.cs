// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the sum of two types. An instance of the <see cref="Either{TLeft, TRight}"/> class 
    /// contains either a <c>TLeft</c> value or a <c>TRight</c> value but not both.
    /// </summary>
    /// <remarks>The enclosed value might be <see langword="null"/>.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    public abstract partial class Either<TLeft, TRight>
    {
#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
        protected Either() { }
#else
        private Either() { }
#endif

        public abstract void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight);

        public abstract TResult Map<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        public abstract Maybe<TLeft> LeftOrNone();

        public abstract Maybe<TRight> RightOrNone();

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        internal static Either<TLeft, TRight> η(TLeft value)
        {
            Contract.Ensures(Contract.Result<Either<TLeft, TRight>>() != null);

            return new Left_(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
            Justification = "[Ignore] Weird. This method does have a caller, namely Either.Right().")]
        internal static Either<TLeft, TRight> η(TRight value)
        {
            Contract.Ensures(Contract.Result<Either<TLeft, TRight>>() != null);

            return new Right_(value);
        }

        private sealed class Left_ : Either<TLeft, TRight>, IEquatable<Left_>
        {
            private readonly TLeft _value;

            public Left_(TLeft value)
            {
                _value = value;
            }

            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight)
            {
                Require.NotNull(caseLeft, "caseLeft");

                caseLeft.Invoke(_value);
            }

            public override TResult Map<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseLeft, "caseLeft");

                return caseLeft.Invoke(_value);
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
                return _value == null ? 0 : EqualityComparer<TLeft>.Default.GetHashCode(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Left({0})", _value);
            }
        }

        private sealed class Right_ : Either<TLeft, TRight>, IEquatable<Right_>
        {
            private readonly TRight _value;

            public Right_(TRight value)
            {
                _value = value;
            }

            public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight)
            {
                Require.NotNull(caseRight, "caseRight");

                caseRight.Invoke(_value);
            }

            public override TResult Map<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
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
                return _value == null ? 0 : EqualityComparer<TRight>.Default.GetHashCode(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Right({0})", _value);
            }
        }
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    [ContractClass(typeof(EitherContract<,>))]
    public partial class Either<TLeft, TRight> { }

    [ContractClassFor(typeof(Either<,>))]
    internal abstract class EitherContract<TLeft, TRight> : Either<TLeft, TRight>
    {
        public override void Invoke(Action<TLeft> caseLeft, Action<TRight> caseRight)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);
        }

        public override TResult Map<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);

            return default(TResult);
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
