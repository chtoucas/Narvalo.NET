// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents the sum of two types. An instance of the <see cref="Either{TLeft, TRight}"/> class
    /// contains either a <c>TLeft</c> value or a <c>TRight</c> value but not both.
    /// <para>This class is a "monad" for the left parameter, nevertheless using <see cref="Either{TLeft, TRight}.Swap"/>
    /// you can easily turn it into a "monad" for the right parameter.</para>
    /// </summary>
    /// <remarks>The enclosed value might be null.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    public abstract partial class Either<TLeft, TRight> : Internal.IMatchable<TLeft, TRight>
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

        public abstract Maybe<TLeft> LeftOrNone();

        public abstract Maybe<TRight> RightOrNone();

        public abstract Either<TRight, TLeft> Swap();

        /// <summary>
        /// Represents the left side of the <see cref="Either{TLeft, TRight}"/> type.
        /// </summary>
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

            public override Either<TRight, TLeft> Swap() => Either.FromRight<TRight, TLeft>(Left);

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
        }

        /// <summary>
        /// Represents the right side of the <see cref="Either{TLeft, TRight}"/> type.
        /// </summary>
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

            public override Either<TRight, TLeft> Swap() => Either.FromLeft<TRight, TLeft>(Right);

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
        }
    }

    // Conversion operators.
    public abstract partial class Either<TLeft, TRight>
    {
        public abstract TLeft ToLeft();

        public abstract TRight ToRight();

        public static explicit operator TLeft(Either<TLeft, TRight> value)
            => value == null ? default(TLeft) : value.ToLeft();

        public static explicit operator TRight(Either<TLeft, TRight> value)
            => value == null ? default(TRight) : value.ToRight();

        public static explicit operator Either<TLeft, TRight>(TLeft value)
            => Either.FromLeft<TLeft, TRight>(value);

        public static explicit operator Either<TLeft, TRight>(TRight value)
            => Either.FromRight<TLeft, TRight>(value);

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

    // (More or less) provides the core Monad methods.
    public abstract partial class Either<TLeft, TRight>
    {
        public abstract Either<TResult, TRight> Bind<TResult>(Func<TLeft, Either<TResult, TRight>> leftSelector);

        public abstract Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> selector);

        public Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> selector)
            => Bind(selector);

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

            return square.IsLeft ? square.Left : Either.FromRight<TLeft, TRight>(square.Right);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> Flatten(Either<TLeft, Either<TLeft, TRight>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.IsRight ? square.Right : Either.FromLeft<TLeft, TRight>(square.Left);
        }

        private partial class Left_
        {
            public override Either<TResult, TRight> Bind<TResult>(Func<TLeft, Either<TResult, TRight>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Left);
            }

            public override Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> selector)
                => Either.FromLeft<TLeft, TResult>(Left);
        }

        private partial class Right_
        {
            public override Either<TResult, TRight> Bind<TResult>(Func<TLeft, Either<TResult, TRight>> selector)
                => Either.FromRight<TResult, TRight>(Right);

            public override Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Right);
            }
        }
    }

    // Implements the Internal.IMatchable<TLeft, TRight> interface.
    public abstract partial class Either<TLeft, TRight>
    {
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract void Do(Action<TLeft> caseLeft, Action<TRight> caseRight);

        private partial class Left_
        {
            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                return caseLeft.Invoke(Left);
            }

            public override void Do(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                Require.NotNull(onLeft, nameof(onLeft));

                onLeft.Invoke(Left);
            }
        }

        private partial class Right_
        {
            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                return caseRight.Invoke(Right);
            }

            public override void Do(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                Require.NotNull(onRight, nameof(onRight));

                onRight.Invoke(Right);
            }
        }
    }

    // (More or less) implements the Internal.IHooks<TLeft> and IHooks<TRight> interfaces.
    public abstract partial class Either<TLeft, TRight>
    {
        public abstract void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action);

        public abstract void WhenRight(Func<TRight, bool> predicate, Action<TRight> action);

        public abstract void OnLeft(Action<TLeft> action);

        public abstract void OnRight(Action<TRight> action);

        private partial class Left_
        {
            public override void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Left)) { action.Invoke(Left); }
            }

            public override void WhenRight(Func<TRight, bool> predicate, Action<TRight> action) { }

            public override void OnLeft(Action<TLeft> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(Left);
            }

            public override void OnRight(Action<TRight> action) { }
        }

        private partial class Right_
        {
            public override void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action) { }

            public override void WhenRight(Func<TRight, bool> predicate, Action<TRight> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(Right)) { action.Invoke(Right); }
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
