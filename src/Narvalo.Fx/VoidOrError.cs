// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    // Friendly version of Either<ExceptionDispatchInfo, Unit>, VoidOr<ExceptionDispatchInfo>
    // or Outcome<Unit>.
    [DebuggerDisplay("Void")]
    public abstract partial class VoidOrError : Internal.IAlternative<ExceptionDispatchInfo>
    {
        private VoidOrError() { }

        public abstract bool IsError { get; }

        internal abstract ExceptionDispatchInfo ExceptionInfo { get; }

        public bool IsVoid => !IsError;

        public abstract void ThrowIfError();

        public abstract Outcome<Unit> ToOutcome();

        public static explicit operator Outcome<Unit>(VoidOrError value) => value?.ToOutcome();

        [DebuggerDisplay("Void")]
        private sealed partial class Void_ : VoidOrError
        {
            public Void_() { }

            public override bool IsError => false;

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override void ThrowIfError() { }

            #region Internal.IAlternative<ExceptionDispatchInfo> interface.

            public override TResult Match<TResult>(
                Func<ExceptionDispatchInfo, TResult> caseError,
                Func<TResult> caseSuccess)
            {
                Require.NotNull(caseSuccess, nameof(caseSuccess));

                return caseSuccess.Invoke();
            }

            public override TResult Match<TResult>(
                Func<ExceptionDispatchInfo, TResult> caseError,
                TResult caseSuccess)
                => caseSuccess;

            public override TResult Coalesce<TResult>(
                Func<ExceptionDispatchInfo, bool> predicate,
                Func<ExceptionDispatchInfo, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                return otherwise.Invoke();
            }

            public override TResult Coalesce<TResult>(
                Func<ExceptionDispatchInfo, bool> predicate,
                TResult then,
                TResult other)
                => other;

            public override void Do(
                Func<ExceptionDispatchInfo, bool> predicate,
                Action<ExceptionDispatchInfo> action,
                Action otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                otherwise.Invoke();
            }

            public override void Do(Action<ExceptionDispatchInfo> onError, Action onSuccess)
            {
                Require.NotNull(onSuccess, nameof(onSuccess));

                onSuccess.Invoke();
            }

            public override void OnError(Action<ExceptionDispatchInfo> action) { }

            public override void OnSuccess(Action action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke();
            }

            #endregion

            public override Outcome<Unit> ToOutcome() => Outcome.Success(Unit.Single);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Void";
            }
        }

        [DebuggerDisplay("Error")]
        [DebuggerTypeProxy(typeof(Error_.DebugView))]
        private sealed partial class Error_ : VoidOrError
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Error_(ExceptionDispatchInfo exceptionInfo)
            {
                Demand.NotNull(exceptionInfo);

                _exceptionInfo = exceptionInfo;
            }

            public override bool IsError => true;

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { Warrant.NotNull<ExceptionDispatchInfo>(); return _exceptionInfo; }
            }

            public override void ThrowIfError() => ExceptionInfo.Throw();

            #region Internal.IAlternative<ExceptionDispatchInfo> interface.

            public override TResult Match<TResult>(
                Func<ExceptionDispatchInfo, TResult> caseError,
                Func<TResult> caseSuccess)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(ExceptionInfo);
            }

            public override TResult Match<TResult>(
                Func<ExceptionDispatchInfo, TResult> caseError,
                TResult caseSuccess)
            {
                Require.NotNull(caseError, nameof(caseError));

                return caseError.Invoke(ExceptionInfo);
            }

            public override TResult Coalesce<TResult>(
                Func<ExceptionDispatchInfo, bool> predicate,
                Func<ExceptionDispatchInfo, TResult> selector,
                Func<TResult> otherwise)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(selector, nameof(selector));
                Require.NotNull(otherwise, nameof(otherwise));

                return predicate.Invoke(ExceptionInfo) ? selector.Invoke(ExceptionInfo) : otherwise.Invoke();
            }

            public override TResult Coalesce<TResult>(
                Func<ExceptionDispatchInfo, bool> predicate,
                TResult then,
                TResult other)
            {
                Require.NotNull(predicate, nameof(predicate));

                return predicate.Invoke(ExceptionInfo) ? then : other;
            }

            public override void Do(
                Func<ExceptionDispatchInfo, bool> predicate,
                Action<ExceptionDispatchInfo> action,
                Action otherwise)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));
                Require.NotNull(otherwise, nameof(otherwise));

                if (predicate.Invoke(ExceptionInfo))
                {
                    action.Invoke(ExceptionInfo);
                }
                else
                {
                    otherwise.Invoke();
                }
            }

            public override void Do(Action<ExceptionDispatchInfo> onError, Action onSuccess)
            {
                Require.NotNull(onError, nameof(onError));

                onError.Invoke(ExceptionInfo);
            }

            public override void OnError(Action<ExceptionDispatchInfo> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(ExceptionInfo);
            }

            public override void OnSuccess(Action action) { }

            #endregion

            public override Outcome<Unit> ToOutcome() => Outcome.Failure<Unit>(ExceptionInfo);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                Exception exception = ExceptionInfo.SourceException;
                Contract.Assume(exception != null);

                return Format.Current("Error({0})", exception.Message);
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOrError.Error_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly Error_ _inner;

                public DebugView(Error_ inner)
                {
                    _inner = inner;
                }

                public ExceptionDispatchInfo ExceptionInfo => _inner.ExceptionInfo;
            }
        }
    }

    // Factory methods.
    public partial class VoidOrError
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrError s_Void = new VoidOrError.Void_();

        public static VoidOrError Void { get { Warrant.NotNull<VoidOrError>(); return s_Void; } }

        public static VoidOrError Error(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));
            Warrant.NotNull<VoidOrError>();

            return new VoidOrError.Error_(exceptionInfo);
        }
    }

    // Implements the Internal.IAlternative<ExceptionDispatchInfo> interface.
    public partial class VoidOrError
    {
        public abstract TResult Match<TResult>(
            Func<ExceptionDispatchInfo, TResult> caseError,
            Func<TResult> caseSuccess);

        public abstract TResult Match<TResult>(
            Func<ExceptionDispatchInfo, TResult> caseError,
            TResult caseSuccess);

        public abstract TResult Coalesce<TResult>(
            Func<ExceptionDispatchInfo, bool> predicate,
            Func<ExceptionDispatchInfo, TResult> selector,
            Func<TResult> otherwise);

        public abstract TResult Coalesce<TResult>(
            Func<ExceptionDispatchInfo, bool> predicate,
            TResult then,
            TResult other);

        public abstract void Do(
            Func<ExceptionDispatchInfo, bool> predicate,
            Action<ExceptionDispatchInfo> action,
            Action otherwise);

        public abstract void Do(Action<ExceptionDispatchInfo> onError, Action onSuccess);

        public abstract void OnError(Action<ExceptionDispatchInfo> action);

        public abstract void OnSuccess(Action action);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial class VoidOrException
    {
        private sealed partial class Void_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(IsSuccess);
            }
        }

        private sealed partial class Error_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_exceptionInfo != null);
                Contract.Invariant(IsError);
            }
        }
    }
}

#endif
