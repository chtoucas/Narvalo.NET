// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the sum of two types. An instance of the <see cref="Either{TLeft, TRight}"/> class
    /// contains either a <c>TLeft</c> value or a <c>TRight</c> value but not both.
    /// <para>This class is a "monad" of the left type parameter, nevertheless using
    /// <see cref="Either{TLeft, TRight}.Swap"/> you can easily turn it into a "monad" of the
    /// right type parameter.</para>
    /// </summary>
    /// <remarks>The enclosed value might be null.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class Either<TLeft, TRight> : Internal.IEither<TLeft, TRight>
    {
#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
        protected Either() { }
#else
        private Either() { }
#endif

        public abstract bool IsLeft { get; }

        public bool IsRight => !IsLeft;

        internal abstract TLeft Left { get; }

        internal abstract TRight Right { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsLeft ? "Left" : "Right";

        public abstract Maybe<TLeft> LeftOrNone();

        public abstract Maybe<TRight> RightOrNone();

        public abstract Either<TRight, TLeft> Swap();

        public abstract Either<TRight, TLeft> SwapUnchecked();

        /// <summary>
        /// Represents the left side of the <see cref="Either{TLeft, TRight}"/> type.
        /// </summary>
        [DebuggerTypeProxy(typeof(Either<,>.Left_.DebugView))]
        private sealed partial class Left_ : Either<TLeft, TRight>, IEquatable<Left_>
        {
            private readonly TLeft _value;

            public Left_(TLeft value)
            {
                _value = value;
            }

            public override bool IsLeft => true;

            internal override TLeft Left => _value;

            internal override TRight Right { get { throw new InvalidOperationException("XXX"); } }

            public override Maybe<TLeft> LeftOrNone() => Maybe.Of(Left);

            public override Maybe<TRight> RightOrNone() => Maybe<TRight>.None;

            public override Either<TRight, TLeft> Swap() { throw new InvalidOperationException("XXX"); }

            public override Either<TRight, TLeft> SwapUnchecked() => Either.OfRight<TRight, TLeft>(Left);

            public bool Equals(Left_ other)
            {
                if (other == this) { return true; }
                if (other == null) { return false; }

                return EqualityComparer<TLeft>.Default.Equals(Left, other.Left);
            }

            public override bool Equals(object obj) => Equals(obj as Left_);

            public override int GetHashCode()
                => Left == null ? 0 : EqualityComparer<TLeft>.Default.GetHashCode(Left);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Left({0})", Left);
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Either{TLeft, TRight}.Left_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly Either<TLeft, TRight> _inner;

                public DebugView(Either<TLeft, TRight> inner)
                {
                    _inner = inner;
                }

                public TLeft Left => _inner.Left;
            }
        }

        /// <summary>
        /// Represents the right side of the <see cref="Either{TLeft, TRight}"/> type.
        /// </summary>
        [DebuggerTypeProxy(typeof(Either<,>.Right_.DebugView))]
        private sealed partial class Right_ : Either<TLeft, TRight>, IEquatable<Right_>
        {
            private readonly TRight _value;

            public Right_(TRight value)
            {
                _value = value;
            }

            public override bool IsLeft => false;

            internal override TLeft Left { get { throw new InvalidOperationException("XXX"); } }

            internal override TRight Right => _value;

            public override Maybe<TLeft> LeftOrNone() => Maybe<TLeft>.None;

            public override Maybe<TRight> RightOrNone() => Maybe.Of(Right);

            public override Either<TRight, TLeft> Swap() => Either.OfLeft<TRight, TLeft>(Right);

            public override Either<TRight, TLeft> SwapUnchecked() => Either.OfLeft<TRight, TLeft>(Right);

            public bool Equals(Right_ other)
            {
                if (other == this) { return true; }
                if (other == null) { return false; }

                return EqualityComparer<TRight>.Default.Equals(Right, other.Right);
            }

            public override bool Equals(object obj) => Equals(obj as Right_);

            public override int GetHashCode()
                => Right == null ? 0 : EqualityComparer<TRight>.Default.GetHashCode(Right);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Right({0})", Right);
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Either{TLeft, TRight}.Right_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly Either<TLeft, TRight> _inner;

                public DebugView(Either<TLeft, TRight> inner)
                {
                    _inner = inner;
                }

                public TRight Right => _inner.Right;
            }
        }
    }

    // Conversion operators.
    public partial class Either<TLeft, TRight>
    {
        public abstract TLeft ToLeft();

        public abstract TRight ToRight();

        public static explicit operator TLeft(Either<TLeft, TRight> value)
            => value == null ? default(TLeft) : value.ToLeft();

        public static explicit operator TRight(Either<TLeft, TRight> value)
            => value == null ? default(TRight) : value.ToRight();

        public static explicit operator Either<TLeft, TRight>(TLeft value)
            => Either.OfLeft<TLeft, TRight>(value);

        public static explicit operator Either<TLeft, TRight>(TRight value)
            => Either.OfRight<TLeft, TRight>(value);

        private partial class Left_
        {
            public override TLeft ToLeft() => Left;

            public override TRight ToRight()
            {
                throw new InvalidCastException("XXX");
            }
        }

        private partial class Right_
        {
            public override TLeft ToLeft()
            {
                throw new InvalidCastException("XXX");
            }

            public override TRight ToRight() => Right;
        }
    }

    // Provides the core Monad methods.
    public partial class Either<TLeft, TRight>
    {
        public Either<TResult, TRight> Bind<TResult>(Func<TLeft, Either<TResult, TRight>> leftSelector)
            => BindLeft(leftSelector);

        public abstract Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> selector);

        public abstract Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> selector);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> η(TLeft leftValue)
        {
            Warrant.NotNull<Either<TLeft, TRight>>();

            return new Left_(leftValue);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> OfRight(TRight value)
        {
            Warrant.NotNull<Either<TLeft, TRight>>();

            return new Right_(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> μ(Either<Either<TLeft, TRight>, TRight> square)
        {
            Require.NotNull(square, nameof(square));

            return square.IsLeft ? square.Left : Either.OfRight<TLeft, TRight>(square.Right);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> FlattenRight(Either<TLeft, Either<TLeft, TRight>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.IsRight ? square.Right : Either.OfLeft<TLeft, TRight>(square.Left);
        }

        private partial class Left_
        {
            public override Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Left);
            }

            public override Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> selector)
                => Either.OfLeft<TLeft, TResult>(Left);
        }

        private partial class Right_
        {
            public override Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> selector)
                => Either.OfRight<TResult, TRight>(Right);

            public override Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Right);
            }
        }
    }

    // Implements the Internal.IEither<TLeft, TRight> interface.
    public partial class Either<TLeft, TRight>
    {
        public abstract TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        // Alias for WhenLeft().
        // NB: We keep this one public as it overrides the auto-generated method.
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        public void When(Func<TLeft, bool> leftPredicate, Action<TLeft> action)
            => WhenLeft(leftPredicate, action);

        // Alias for WhenRight(). Publicly hidden.
        void Internal.ISecondaryContainer<TRight>.When(Func<TRight, bool> rightPredicate, Action<TRight> action)
            => WhenRight(rightPredicate, action);

        public abstract void Do(Action<TLeft> onLeft, Action<TRight> onRight);

        // Alias for OnLeft(). Publicly hidden.
        void Internal.IContainer<TLeft>.Do(Action<TLeft> onLeft) => OnLeft(onLeft);

        // Alias for OnRight(). Publicly hidden.
        void Internal.ISecondaryContainer<TRight>.Do(Action<TRight> onRight) => OnRight(onRight);

        public abstract void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action);

        public abstract void WhenRight(Func<TRight, bool> predicate, Action<TRight> action);

        public abstract void OnLeft(Action<TLeft> action);

        public abstract void OnRight(Action<TRight> action);

        private partial class Left_
        {
            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                return caseLeft.Invoke(Left);
            }

            public override void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Left)) { action.Invoke(Left); }
            }

            public override void WhenRight(Func<TRight, bool> predicate, Action<TRight> action) { }

            public override void Do(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                Require.NotNull(onLeft, nameof(onLeft));

                onLeft.Invoke(Left);
            }

            public override void OnLeft(Action<TLeft> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Left);
            }

            public override void OnRight(Action<TRight> action) { }
        }

        private partial class Right_
        {
            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                return caseRight.Invoke(Right);
            }

            public override void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action) { }

            public override void WhenRight(Func<TRight, bool> predicate, Action<TRight> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Right)) { action.Invoke(Right); }
            }

            public override void Do(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                Require.NotNull(onRight, nameof(onRight));

                onRight.Invoke(Right);
            }

            public override void OnLeft(Action<TLeft> action) { }

            public override void OnRight(Action<TRight> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Right);
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
