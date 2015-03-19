// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

    /// <summary>
    /// Represents the sum of two types. An instance of the <see cref="Either{TLeft, TRight}"/> class 
    /// contains either a <c>TLeft</c> value or a <c>TRight</c> value but not both.
    /// </summary>
    /// <remarks>The enclosed value might be <see langword="null"/>.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    public abstract partial class Either<TLeft, TRight>
    {
        protected Either() { }

        public abstract void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight);

        public abstract TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        internal sealed class Left : Either<TLeft, TRight>, IEquatable<Left>
        {
            private readonly TLeft _value;

            public Left(TLeft value)
            {
                _value = value;
            }

            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseLeft, "caseLeft");

                return caseLeft.Invoke(_value);
            }

            public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight)
            {
                Require.NotNull(caseLeft, "caseLeft");

                caseLeft.Invoke(_value);
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
                return _value == null ? 0 : EqualityComparer<TLeft>.Default.GetHashCode(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Left({0})", _value);
            }
        }

        internal sealed class Right : Either<TLeft, TRight>, IEquatable<Right>
        {
            private readonly TRight _value;

            public Right(TRight value)
            {
                _value = value;
            }

            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseRight, "caseRight");

                return caseRight.Invoke(_value);
            }

            public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight)
            {
                Require.NotNull(caseRight, "caseRight");

                caseRight.Invoke(_value);
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
                return _value == null ? 0 : EqualityComparer<TRight>.Default.GetHashCode(_value);
            }

            public override string ToString()
            {
                return Format.CurrentCulture("Right({0})", _value);
            }
        }
    }

#if CONTRACTS_FULL

    [ContractClass(typeof(EitherContract<,>))]
    public abstract partial class Either<TLeft, TRight> { }

    [ContractClassFor(typeof(Either<,>))]
    internal abstract class EitherContract<TLeft, TRight> : Either<TLeft, TRight>
    {
        public override void Apply(Action<TLeft> caseLeft, Action<TRight> caseRight)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);
        }

        public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);

            return default(TResult);
        }
    }

#endif
}
