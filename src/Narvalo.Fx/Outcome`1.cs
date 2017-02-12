// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;

    using Narvalo.Fx.Properties;

    /// <summary>
    /// Represents the outcome of a computation which might throw an exception.
    /// An instance of the <see cref="Outcome{T}"/> class contains either a <c>T</c>
    /// value or the exception state at the point it was thrown.
    /// </summary>
    /// <remarks>
    /// <para>WARNING: We do not catch exceptions throw by any supplied delegate...</para>
    /// <para>This class is not meant to replace the standard exception mechanism.</para>
    /// </remarks>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    // Friendly version of Either<ExceptionDispatchInfo, T>.
    public abstract partial class Outcome<T> : Internal.IMatchable<T, ExceptionDispatchInfo>
    {
#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
        protected Outcome(bool isSuccess) { IsSuccess = isSuccess; }
#else
        private Outcome(bool isSuccess) { IsSuccess = isSuccess; }
#endif

        /// <summary>
        /// Gets a value indicating whether the outcome is successful.
        /// </summary>
        /// <remarks>Most of the time, you don't need to access this property.
        /// You are better off using the rich vocabulary that this class offers.</remarks>
        /// <value>true if the outcome is successful; otherwise false.</value>
        public bool IsSuccess { get; }

        #region Operators

        public static explicit operator Outcome<T>(T value)
        {
            Warrant.NotNull<Outcome<T>>();

            return η(value);
        }

        public static explicit operator Outcome<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Expect.NotNull(exceptionInfo);
            Warrant.NotNull<Outcome<T>>();

            return η(exceptionInfo);
        }

        public static explicit operator T(Outcome<T> value)
        {
            Require.NotNull(value, nameof(value));

            // We check the value of the property IsSuccess even if this is not really necessary
            // since a direct cast would have worked too:
            //  return ((Success_)value).Value;
            // but doing so allows us to throw a more meaningful exception and to effectively
            // hide the implementation details; the default exception would say something
            // like "Unable to cast a Failure_ type to a Success_ type".
            if (!value.IsSuccess)
            {
                throw new InvalidCastException(Strings.Outcome_CannotCastFailureToValue);
            }

            var success = value as Success_;
            Contract.Assume(success != null);

            return success.Value;
        }

        public static explicit operator ExceptionDispatchInfo(Outcome<T> value)
        {
            Require.NotNull(value, nameof(value));
            Warrant.NotNull<ExceptionDispatchInfo>();

            // We check the value of the property IsSuccess even if this is not really necessary
            // since a direct cast would have worked too:
            //  return ((Failure_)value).Value;
            // but doing so allows us to throw a more meaningful exception and to effectively
            // hide the implementation details; the default exception would say something
            // like "Unable to cast a Success_ type to a Failure_ type".
            if (value.IsSuccess)
            {
                throw new InvalidCastException(Strings.Outcome_CannotCastSuccessToException);
            }

            var failure = value as Failure_;
            Contract.Assume(failure != null);

            return failure.ExceptionInfo;
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Obtains the enclosed value.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsSuccess"/> is true.</remarks>
        internal abstract T Value { get; }

        /// <summary>
        /// Obtains the enclosed exception state.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsSuccess"/> is false.</remarks>
        internal abstract ExceptionDispatchInfo ExceptionInfo { get; }

        // Core monad Bind method.
        public abstract Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector);

        // Overrides the 'Select' auto-generated (extension) method (see Outcome.g.cs).
        // Since Select is a building block, we override it in Failure_ and Success_.
        // Otherwise we would have to call ToValue() or ToExceptionInfo() which imply a casting.
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract Outcome<TResult> Select<TResult>(Func<T, TResult> selector);

        #endregion

        /// <summary>
        /// Obtains the underlying value if any; otherwise the default value of the type T.
        /// </summary>
        /// <returns>The underlying value if any; otherwise the default value of the type T.</returns>
        public abstract T ValueOrDefault();

        /// <summary>
        /// Returns the underlying value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The underlying value if any; otherwise <paramref name="other"/>.</returns>
        public abstract T ValueOrElse(T other);

        public abstract T ValueOrElse(Func<T> valueFactory);

        public abstract Maybe<T> ValueOrNone();

        public abstract T ValueOrThrow();

        /// <summary>
        /// Represents the "success" part of the <see cref="Outcome{T}"/> type.
        /// </summary>
        private sealed partial class Success_ : Outcome<T>, IEquatable<Success_>
        {
            public Success_(T value) : base(true) { Value = value; }

            internal override T Value { get; }

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return selector.Invoke(Value);
            }

            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return Outcome<TResult>.η(selector.Invoke(Value));
            }

            public override T ValueOrDefault() => Value;

            public override T ValueOrElse(T other) => Value;

            public override T ValueOrElse(Func<T> valueFactory) => Value;

            public override Maybe<T> ValueOrNone() => Maybe.Of(Value);

            public override T ValueOrThrow() => Value;

            public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<ExceptionDispatchInfo, TResult> caseFailure)
            {
                Require.NotNull(caseSuccess, nameof(caseSuccess));

                return caseSuccess.Invoke(Value);
            }

            public override void Do(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
            {
                Require.NotNull(caseSuccess, nameof(caseSuccess));

                caseSuccess.Invoke(Value);
            }

            public bool Equals(Success_ other)
            {
                if (other == this)
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object obj) => Equals(obj as Success_);

            public override int GetHashCode()
                => Value == null ? 0 : EqualityComparer<T>.Default.GetHashCode(Value);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Success({0})", Value);
            }
        }

        /// <summary>
        /// Represents the "failure" part of the <see cref="Outcome{T}"/> type.
        /// </summary>
        private sealed partial class Failure_ : Outcome<T>, IEquatable<Failure_>
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Failure_(ExceptionDispatchInfo exceptionInfo)
                : base(false)
            {
                Demand.NotNull(exceptionInfo);

                _exceptionInfo = exceptionInfo;
            }

            internal override T Value
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get
                {
                    Warrant.NotNull<ExceptionDispatchInfo>();

                    return _exceptionInfo;
                }
            }

            public override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
                => Outcome<TResult>.η(ExceptionInfo);

            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
                => Outcome<TResult>.η(ExceptionInfo);

            public override T ValueOrDefault() => default(T);

            public override T ValueOrElse(T other) => other;

            public override T ValueOrElse(Func<T> valueFactory)
            {
                Require.NotNull(valueFactory, nameof(valueFactory));

                return valueFactory.Invoke();
            }

            public override Maybe<T> ValueOrNone() => Maybe<T>.None;

            public override T ValueOrThrow()
            {
                ExceptionInfo.Throw();

                return default(T);
            }

            public override TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<ExceptionDispatchInfo, TResult> caseFailure)
            {
                Require.NotNull(caseFailure, nameof(caseFailure));

                return caseFailure.Invoke(ExceptionInfo);
            }

            public override void Do(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure)
            {
                Require.NotNull(caseFailure, nameof(caseFailure));

                caseFailure.Invoke(ExceptionInfo);
            }

            public bool Equals(Failure_ other)
            {
                if (other == this)
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

                return EqualityComparer<ExceptionDispatchInfo>.Default.Equals(ExceptionInfo, other.ExceptionInfo);
            }

            public override bool Equals(object obj) => Equals(obj as Failure_);

            public override int GetHashCode()
                => EqualityComparer<ExceptionDispatchInfo>.Default.GetHashCode(ExceptionInfo);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Failure({0})", ExceptionInfo);
            }
        }
    }

    // Core Monad methods.
    public abstract partial class Outcome<T>
    {
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T> η(T value) => new Success_(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T> η(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));

            return new Failure_(exceptionInfo);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T> μ(Outcome<Outcome<T>> square)
        {
            Require.NotNull(square, nameof(square));

            if (square.IsSuccess)
            {
                return square.Value;
            }
            else
            {
                return η(square.ExceptionInfo);
            }
        }
    }

    // Implements the Internal.IMatchable<T, ExceptionDispatchInfo> interface.
    public abstract partial class Outcome<T>
    {
        public abstract TResult Match<TResult>(
            Func<T, TResult> caseSuccess,
            Func<ExceptionDispatchInfo, TResult> caseFailure);

        public abstract void Do(Action<T> caseSuccess, Action<ExceptionDispatchInfo> caseFailure);
    }

#if CONTRACTS_FULL

    // In real world, only Success_ and Failure_ can inherit from Outcome.
    // Adding the following object invariants on Outcome<T>:
    //  (IsSuccess && this is Success_) || (this is Failure_)
    // should make unecessary any call to Contract.Assume but I have not been able to make this work.
    [ContractClass(typeof(OutcomeContract<>))]
    public partial class Outcome<T>
    {
        private partial class Success_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(IsSuccess);
            }
        }

        private partial class Failure_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(!IsSuccess);
                Contract.Invariant(_exceptionInfo != null);
            }
        }
    }

    [ContractClassFor(typeof(Outcome<>))]
    internal abstract class OutcomeContract<T> : Outcome<T>
    {
        protected OutcomeContract(bool isSuccess) : base(isSuccess) { }

        public override Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> selector)
        {
            Contract.Requires(selector != null);

            return default(Outcome<TResult>);
        }

        public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Contract.Requires(selector != null);

            return default(Outcome<TResult>);
        }
    }

#endif
}
