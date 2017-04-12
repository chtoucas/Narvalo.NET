// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    using Narvalo.Properties;

    /// <summary>
    /// Represents the sum of two types. An instance of the <see cref="Either{TLeft, TRight}"/> class
    /// contains either a <c>TLeft</c> value or a <c>TRight</c> value but not both.
    /// <para>This class is a "monad" for the left type parameter, nevertheless using
    /// <see cref="Either{TLeft, TRight}.Swap"/> or <see cref="Either{TLeft, TRight}.SwapUnchecked"/>
    /// you can easily turn it into a "monad" for the right type parameter.</para>
    /// </summary>
    /// <remarks>The enclosed value might be null.</remarks>
    /// <typeparam name="TLeft">The underlying type of the left part.</typeparam>
    /// <typeparam name="TRight">The underlying type of the right part.</typeparam>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class Either<TLeft, TRight> : IStructuralEquatable, Internal.IEither<TLeft, TRight>
    {
        private Either() { }

        public abstract void Deconstruct(out bool isLeft, out TLeft left, out TRight right);

        public abstract bool IsLeft { get; }

        public bool IsRight => !IsLeft;

        internal abstract TLeft Left { get; }

        internal abstract TRight Right { get; }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsLeft ? "Left" : "Right";

        public abstract Maybe<TLeft> LeftOrNone();

        public abstract Maybe<TRight> RightOrNone();

        // Swap() must be understood as SwapLeft(), kind of a "monad"-aware swap methods,
        // it fails if IsLeft is false.
        public abstract Either<TRight, TLeft> Swap();

        public abstract Either<TRight, TLeft> SwapUnchecked();

        /// <summary>
        /// Represents the left side of the <see cref="Either{TLeft, TRight}"/> type.
        /// </summary>
        [DebuggerTypeProxy(typeof(Either<,>.Left_.DebugView))]
        private sealed partial class Left_ : Either<TLeft, TRight>, IEquatable<Left_>
        {
            public Left_(TLeft value) => Left = value;

            public override void Deconstruct(out bool isLeft, out TLeft left, out TRight right)
            {
                isLeft = true;
                left = Left;
                right = default(TRight);
            }

            public override bool IsLeft => true;

            internal override TLeft Left { get; }

            internal override TRight Right => throw new InvalidOperationException();

            public override Maybe<TLeft> LeftOrNone() => Maybe.Of(Left);

            public override Maybe<TRight> RightOrNone() => Maybe<TRight>.None;

            public override Either<TRight, TLeft> Swap() => Either<TRight, TLeft>.OfRight(Left);

            public override Either<TRight, TLeft> SwapUnchecked() => Either<TRight, TLeft>.OfRight(Left);

            public bool Equals(Left_ other)
            {
                if (ReferenceEquals(other, null)) { return false; }
                if (ReferenceEquals(other, this)) { return true; }
                // This class is sealed so no need to check:
                // > if (other.GetType() != GetType()) { return false; }

                return EqualityComparer<TLeft>.Default.Equals(Left, other.Left);
            }

            public override bool Equals(object obj) => Equals(obj as Left_);

            public override int GetHashCode() => Left?.GetHashCode() ?? 0;

            public override string ToString() => "Left(" + Left?.ToString() + ")";

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Either{TLeft, TRight}.Left_"/>.
            /// </summary>
            /// <remarks>Ensure that <see cref="Either{TLeft, TRight}.Left"/> does not throw
            /// in the debugger for DEBUG builds.</remarks>
            [ExcludeFromCodeCoverage]
            private sealed class DebugView
            {
                private readonly Either<TLeft, TRight> _inner;

                public DebugView(Either<TLeft, TRight> inner) => _inner = inner;

                public TLeft Left => _inner.Left;
            }
        }

        /// <summary>
        /// Represents the right side of the <see cref="Either{TLeft, TRight}"/> type.
        /// </summary>
        [DebuggerTypeProxy(typeof(Either<,>.Right_.DebugView))]
        private sealed partial class Right_ : Either<TLeft, TRight>, IEquatable<Right_>
        {
            public Right_(TRight value) => Right = value;

            public override void Deconstruct(out bool isLeft, out TLeft left, out TRight right)
            {
                isLeft = false;
                left = default(TLeft);
                right = Right;
            }

            public override bool IsLeft => false;

            internal override TLeft Left => throw new InvalidOperationException();

            internal override TRight Right { get; }

            public override Maybe<TLeft> LeftOrNone() => Maybe<TLeft>.None;

            public override Maybe<TRight> RightOrNone() => Maybe.Of(Right);

            public override Either<TRight, TLeft> Swap()
                => throw new InvalidOperationException(Strings.InvalidOperation_SwapRightyEither);

            public override Either<TRight, TLeft> SwapUnchecked()
                => Either<TRight, TLeft>.OfLeft(Right);

            public bool Equals(Right_ other)
            {
                if (ReferenceEquals(other, null)) { return false; }
                if (ReferenceEquals(other, this)) { return true; }
                // This class is sealed so no need to check:
                // > if (other.GetType() != GetType()) { return false; }

                return EqualityComparer<TRight>.Default.Equals(Right, other.Right);
            }

            public override bool Equals(object obj) => Equals(obj as Right_);

            public override int GetHashCode() => Right?.GetHashCode() ?? 0;

            public override string ToString() => "Right(" + Right?.ToString() + ")";

            /// <summary>
            /// Represents a debugger type proxy for <see cref="Either{TLeft, TRight}.Right_"/>.
            /// </summary>
            /// <remarks>Ensure that <see cref="Either{TLeft, TRight}.Right"/> does not throw
            /// in the debugger for DEBUG builds.</remarks>
            [ExcludeFromCodeCoverage]
            private sealed class DebugView
            {
                private readonly Either<TLeft, TRight> _inner;

                public DebugView(Either<TLeft, TRight> inner) => _inner = inner;

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

        public static explicit operator Either<TLeft, TRight>(TLeft value) => OfLeft(value);

        public static explicit operator Either<TLeft, TRight>(TRight value) => OfRight(value);

        private partial class Left_
        {
            public override TLeft ToLeft() => Left;

            public override TRight ToRight()
                => throw new InvalidCastException(Strings.InvalidConversionToRight);
        }

        private partial class Right_
        {
            public override TLeft ToLeft()
                => throw new InvalidCastException(Strings.InvalidConversionToLeft);

            public override TRight ToRight() => Right;
        }
    }

    // Provides the core Monad methods + minimalist implementation of a Monad on the right.
    public partial class Either<TLeft, TRight>
    {
        public Either<TResult, TRight> Bind<TResult>(Func<TLeft, Either<TResult, TRight>> leftBinder)
            => BindLeft(leftBinder);

        public abstract Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> binder);

        public abstract Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> binder);

        // NB: This method is normally internal, but Either<TLeft, TRight>.OfLeft() is more readable
        // than Either.OfLeft<TLeft, TRight>() - no type inference.
        // See also Either.Left<TRight>.Return(TLeft).
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
        public static Either<TLeft, TRight> OfLeft(TLeft leftValue) => new Left_(leftValue);

        // NB: This method is normally internal, but Either<TLeft, TRight>.OfRight() is more readable
        // than Either.OfRight<TLeft, TRight>() - no type inference.
        // See also Either.Right<TLeft>.Return(TRight).
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
        public static Either<TLeft, TRight> OfRight(TRight value) => new Right_(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> μ(Either<Either<TLeft, TRight>, TRight> square)
        {
            Require.NotNull(square, nameof(square));

            return square.IsLeft ? square.Left : OfRight(square.Right);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Either<TLeft, TRight> FlattenRight(Either<TLeft, Either<TLeft, TRight>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.IsRight ? square.Right : OfLeft(square.Left);
        }

        private partial class Left_
        {
            public override Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> binder)
            {
                Require.NotNull(binder, nameof(binder));

                return binder(Left);
            }

            public override Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> binder)
                => Either<TLeft, TResult>.OfLeft(Left);
        }

        private partial class Right_
        {
            public override Either<TResult, TRight> BindLeft<TResult>(Func<TLeft, Either<TResult, TRight>> binder)
                => Either<TResult, TRight>.OfRight(Right);

            public override Either<TLeft, TResult> BindRight<TResult>(Func<TRight, Either<TLeft, TResult>> binder)
            {
                Require.NotNull(binder, nameof(binder));

                return binder(Right);
            }
        }
    }

    // Implements the Internal.IEither<TLeft, TRight> interface.
    public partial class Either<TLeft, TRight>
    {
        public abstract bool ContainsLeft(TLeft value);
        public abstract bool ContainsLeft(TLeft value, IEqualityComparer<TLeft> comparer);

        public abstract bool ContainsRight(TRight value);
        public abstract bool ContainsRight(TRight value, IEqualityComparer<TRight> comparer);

        public abstract TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        public abstract void Do(Action<TLeft> onLeft, Action<TRight> onRight);

        public abstract void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action);
        public abstract void WhenRight(Func<TRight, bool> predicate, Action<TRight> action);

        public abstract void OnLeft(Action<TLeft> action);
        public abstract void OnRight(Action<TRight> action);

        #region Publicly hidden methods.

        // Alias for ContainsLeft().
        [ExcludeFromCodeCoverage]
        bool Internal.IContainer<TLeft>.Contains(TLeft value)
            => ContainsLeft(value);

        // Alias for ContainsLeft().
        [ExcludeFromCodeCoverage]
        bool Internal.IContainer<TLeft>.Contains(TLeft value, IEqualityComparer<TLeft> comparer)
            => ContainsLeft(value, comparer);

        // Alias for ContainsRight().
        [ExcludeFromCodeCoverage]
        bool Internal.ISecondaryContainer<TRight>.Contains(TRight value)
            => ContainsRight(value);

        // Alias for ContainsRight().
        [ExcludeFromCodeCoverage]
        bool Internal.ISecondaryContainer<TRight>.Contains(TRight value, IEqualityComparer<TRight> comparer)
           => ContainsRight(value, comparer);

        // Alias for WhenLeft().
        [ExcludeFromCodeCoverage]
        void Internal.IContainer<TLeft>.When(Func<TLeft, bool> predicate, Action<TLeft> action)
            => WhenLeft(predicate, action);

        // Alias for WhenRight().
        [ExcludeFromCodeCoverage]
        void Internal.ISecondaryContainer<TRight>.When(Func<TRight, bool> predicate, Action<TRight> action)
            => WhenRight(predicate, action);

        // Alias for OnLeft().
        [ExcludeFromCodeCoverage]
        void Internal.IContainer<TLeft>.Do(Action<TLeft> action) => OnLeft(action);

        // Alias for OnRight().
        [ExcludeFromCodeCoverage]
        void Internal.ISecondaryContainer<TRight>.Do(Action<TRight> action) => OnRight(action);

        #endregion

        private partial class Left_
        {
            public override bool ContainsLeft(TLeft value)
                => EqualityComparer<TLeft>.Default.Equals(Left, value);

            public override bool ContainsLeft(TLeft value, IEqualityComparer<TLeft> comparer)
                => (comparer ?? EqualityComparer<TLeft>.Default).Equals(Left, value);

            public override bool ContainsRight(TRight value) => false;

            public override bool ContainsRight(TRight value, IEqualityComparer<TRight> comparer) => false;

            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseLeft, nameof(caseLeft));

                return caseLeft(Left);
            }

            public override void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate(Left)) { action(Left); }
            }

            public override void WhenRight(Func<TRight, bool> predicate, Action<TRight> action) { }

            public override void Do(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                Require.NotNull(onLeft, nameof(onLeft));

                onLeft(Left);
            }

            public override void OnLeft(Action<TLeft> action)
            {
                Require.NotNull(action, nameof(action));

                action(Left);
            }

            public override void OnRight(Action<TRight> action) { }
        }

        private partial class Right_
        {
            public override bool ContainsLeft(TLeft value) => false;

            public override bool ContainsLeft(TLeft value, IEqualityComparer<TLeft> comparer) => false;

            public override bool ContainsRight(TRight value)
                => EqualityComparer<TRight>.Default.Equals(Right, value);

            public override bool ContainsRight(TRight value, IEqualityComparer<TRight> comparer)
                => (comparer ?? EqualityComparer<TRight>.Default).Equals(Right, value);

            public override TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight)
            {
                Require.NotNull(caseRight, nameof(caseRight));

                return caseRight(Right);
            }

            public override void WhenLeft(Func<TLeft, bool> predicate, Action<TLeft> action) { }

            public override void WhenRight(Func<TRight, bool> predicate, Action<TRight> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate(Right)) { action(Right); }
            }

            public override void Do(Action<TLeft> onLeft, Action<TRight> onRight)
            {
                Require.NotNull(onRight, nameof(onRight));

                onRight(Right);
            }

            public override void OnLeft(Action<TLeft> action) { }

            public override void OnRight(Action<TRight> action)
            {
                Require.NotNull(action, nameof(action));

                action(Right);
            }
        }
    }

    // Implements the IStructuralEquatable interface.
    public partial class Either<TLeft, TRight>
    {
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (ReferenceEquals(other, null)) { return false; }
            if (ReferenceEquals(other, this)) { return true; }

            if (IsLeft)
            {
                var obj = other as Either<TLeft, TRight>.Left_;
                return obj != null && comparer.Equals(Left, obj.Left);
            }
            else
            {
                var obj = other as Either<TLeft, TRight>.Right_;
                return obj != null && comparer.Equals(Right, obj.Right);
            }
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return HashCodeHelpers.Combine(
                IsLeft ? comparer.GetHashCode(Left) : 0,
                IsRight ? comparer.GetHashCode(Right) : 0);
        }
    }
}
