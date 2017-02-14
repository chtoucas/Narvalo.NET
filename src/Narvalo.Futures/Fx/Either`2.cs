// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the sum of two types. An instance of the <see cref="Either{TLeft, TRight}"/> class
    /// contains either a <c>TLeft</c> value or a <c>TRight</c> value but not both.
    /// </summary>
    /// <remarks>The enclosed value might be <see langword="null"/>.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    public abstract partial class Either<TLeft, TRight> : Internal.IMatchable<TLeft, TRight>
    {
#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
        protected Either() { }
#else
        private Either() { }
#endif

        public abstract Maybe<TLeft> LeftOrNone();

        public abstract Maybe<TRight> RightOrNone();

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> η(TLeft value)
        {
            Warrant.NotNull<Either<TLeft, TRight>>();

            return new Left_(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> η(TRight value)
        {
            Warrant.NotNull<Either<TLeft, TRight>>();

            return new Right_(value);
        }

        private sealed partial class Left_ : Either<TLeft, TRight>
        {
            private readonly TLeft _value;

            public Left_(TLeft value)
            {
                _value = value;
            }

            public override Maybe<TLeft> LeftOrNone() => Maybe.Of(_value);

            public override Maybe<TRight> RightOrNone() => Maybe<TRight>.None;

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Left({0})", _value);
            }
        }

        private sealed partial class Right_ : Either<TLeft, TRight>
        {
            private readonly TRight _value;

            public Right_(TRight value)
            {
                _value = value;
            }

            public override Maybe<TLeft> LeftOrNone() => Maybe<TLeft>.None;

            public override Maybe<TRight> RightOrNone() => Maybe.Of(_value);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Right({0})", _value);
            }
        }
    }

    // Implements the Internal.IMatchable<TLeft, TRight> interface.
    public abstract partial class Either<TLeft, TRight>
    {
        public abstract TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        public abstract void Do(Action<TLeft> caseLeft, Action<TRight> caseRight);

        public abstract void OnLeft(Action<TLeft> action);

        public abstract void OnRight(Action<TRight> action);

        private partial class Left_
        {
            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                return caseLeft.Invoke(_value);
            }

            public override void Do(Action<TLeft> caseLeft, Action<TRight> caseRight)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                caseLeft.Invoke(_value);
            }

            public override void OnLeft(Action<TLeft> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(_value);
            }

            public override void OnRight(Action<TRight> action) { }
        }

        private partial class Right_
        {
            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                return caseRight.Invoke(_value);
            }

            public override void Do(Action<TLeft> caseLeft, Action<TRight> caseRight)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                caseRight.Invoke(_value);
            }

            public override void OnLeft(Action<TLeft> action) { }

            public override void OnRight(Action<TRight> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(_value);
            }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(EitherContract<,>))]
    public partial class Either<TLeft, TRight> { }

    [ContractClassFor(typeof(Either<,>))]
    internal abstract class EitherContract<TLeft, TRight> : Either<TLeft, TRight>
    {
        public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);

            return default(TResult);
        }

        public override void Do(Action<TLeft> caseLeft, Action<TRight> caseRight)
        {
            Contract.Requires(caseLeft != null);
            Contract.Requires(caseRight != null);
        }

        public override Maybe<TLeft> LeftOrNone() => default(Maybe<TLeft>);

        public override Maybe<TRight> RightOrNone() => default(Maybe<TRight>);
    }
}

#endif
