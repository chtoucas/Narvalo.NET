// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class VoidOrError : Internal.IMaybe<ExceptionDispatchInfo>
    {
        private VoidOrError() { }

        public abstract bool IsVoid { get; }

        public bool IsError => !IsVoid;

        internal abstract ExceptionDispatchInfo ExceptionInfo { get; }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsVoid ? "Void" : "Error";

        public abstract void ThrowIfError();

        [DebuggerTypeProxy(typeof(Void_.DebugView))]
        private sealed partial class Void_ : VoidOrError
        {
            public Void_() { }

            public override bool IsVoid => true;

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { throw new InvalidOperationException("XXX"); }
            }

            public override void ThrowIfError() { }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return "Void";
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOrError.Void_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView { }
        }

        [DebuggerTypeProxy(typeof(Error_.DebugView))]
        private sealed partial class Error_ : VoidOrError
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Error_(ExceptionDispatchInfo exceptionInfo)
            {
                Demand.NotNull(exceptionInfo);

                _exceptionInfo = exceptionInfo;
            }

            public override bool IsVoid => false;

            internal override ExceptionDispatchInfo ExceptionInfo
            {
                get { Warrant.NotNull<ExceptionDispatchInfo>(); return _exceptionInfo; }
            }

            public override void ThrowIfError() => ExceptionInfo.Throw();

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

    // Conversion operators.
    public partial class VoidOrError
    {
        public abstract Exception ToException();

        public abstract ExceptionDispatchInfo ToExceptionInfo();

        public abstract VoidOr<Exception> ToVoidOrException();

        public static explicit operator Exception(VoidOrError value)
            => value?.ToException();

        public static explicit operator ExceptionDispatchInfo(VoidOrError value)
            => value?.ToExceptionInfo();

        public static explicit operator VoidOr<Exception>(VoidOrError value)
            => value?.ToVoidOrException();

        public static explicit operator VoidOrError(ExceptionDispatchInfo exceptionInfo)
            => FromError(exceptionInfo);

        private partial class Void_
        {
            public override Exception ToException()
            {
                throw new InvalidCastException("XXX");
            }

            public override ExceptionDispatchInfo ToExceptionInfo()
            {
                throw new InvalidCastException("XXX");
            }

            public override VoidOr<Exception> ToVoidOrException() => VoidOr<Exception>.Void;
        }

        private partial class Error_
        {
            public override Exception ToException() => ExceptionInfo.SourceException;

            public override ExceptionDispatchInfo ToExceptionInfo() => ExceptionInfo;

            public override VoidOr<Exception> ToVoidOrException() => VoidOr.FromError(ExceptionInfo.SourceException);
        }
    }

    // Factory methods.
    public partial class VoidOrError
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrError s_Void = new VoidOrError.Void_();

        public static VoidOrError Void { get { Warrant.NotNull<VoidOrError>(); return s_Void; } }

        public static VoidOrError FromError(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));
            Warrant.NotNull<VoidOrError>();

            return new VoidOrError.Error_(exceptionInfo);
        }

        // NB: This method serves a different purpose than the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static VoidOrError TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            try
            {
                action.Invoke();

                return Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError(edi);
            }
        }

        // NB: This method serves a different purpose than the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static VoidOrError TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));
            Warrant.NotNull<VoidOrError>();

            try
            {
                action.Invoke();

                return Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError(edi);
            }
            finally
            {
                finallyAction.Invoke();
            }
        }
    }

    // Implements the Internal.IMaybe<ExceptionDispatchInfo> interface.
    public partial class VoidOrError
    {
        public abstract IEnumerable<ExceptionDispatchInfo> ToEnumerable();

        public IEnumerator<ExceptionDispatchInfo> GetEnumerator()
        {
            Warrant.NotNull<IEnumerator<ExceptionDispatchInfo>>();

            return ToEnumerable().GetEnumerator();
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract TResult Match<TResult>(
            Func<ExceptionDispatchInfo, TResult> caseError,
            Func<TResult> caseSuccess);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract TResult Match<TResult>(
            Func<ExceptionDispatchInfo, TResult> caseError,
            TResult caseSuccess);

        public abstract TResult Coalesce<TResult>(
            Func<ExceptionDispatchInfo, bool> predicate,
            Func<ExceptionDispatchInfo, TResult> selector,
            Func<TResult> otherwise);

        public abstract TResult Coalesce<TResult>(
            Func<ExceptionDispatchInfo, bool> predicate,
            TResult thenResult,
            TResult elseResult);

        public abstract void When(
            Func<ExceptionDispatchInfo, bool> predicate,
            Action<ExceptionDispatchInfo> action,
            Action otherwise);

        public abstract void When(Func<ExceptionDispatchInfo, bool> predicate, Action<ExceptionDispatchInfo> action);

        // Alias for OnError(). Publicly hidden.
        void Internal.IContainer<ExceptionDispatchInfo>.Do(Action<ExceptionDispatchInfo> action) => OnError(action);

        public abstract void OnError(Action<ExceptionDispatchInfo> action);

        public abstract void OnSuccess(Action action);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public abstract void Do(Action<ExceptionDispatchInfo> onError, Action onSuccess);

        private partial class Void_
        {
            public override IEnumerable<ExceptionDispatchInfo> ToEnumerable()
            {
                Warrant.NotNull<IEnumerable<ExceptionDispatchInfo>>();

                return Sequence.Empty<ExceptionDispatchInfo>();
            }

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
                TResult thenResult,
                TResult elseResult)
                => elseResult;

            public override void When(
                Func<ExceptionDispatchInfo, bool> predicate,
                Action<ExceptionDispatchInfo> action,
                Action otherwise)
            {
                Require.NotNull(otherwise, nameof(otherwise));

                otherwise.Invoke();
            }

            public override void When(Func<ExceptionDispatchInfo, bool> predicate, Action<ExceptionDispatchInfo> action) { }

            public override void Do(Action<ExceptionDispatchInfo> onError, Action onSuccess)
            {
                Require.NotNull(onSuccess, nameof(onSuccess));

                onSuccess.Invoke();
            }

            public override void OnSuccess(Action action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke();
            }

            public override void OnError(Action<ExceptionDispatchInfo> action) { }
        }

        private partial class Error_
        {
            public override IEnumerable<ExceptionDispatchInfo> ToEnumerable()
            {
                Warrant.NotNull<IEnumerable<ExceptionDispatchInfo>>();

                return Sequence.Of(ExceptionInfo);
            }

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
                TResult thenResult,
                TResult elseResult)
            {
                Require.NotNull(predicate, nameof(predicate));

                return predicate.Invoke(ExceptionInfo) ? thenResult : elseResult;
            }

            public override void When(
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

            public override void When(Func<ExceptionDispatchInfo, bool> predicate, Action<ExceptionDispatchInfo> action)
            {
                Require.NotNull(predicate, nameof(predicate));
                Require.NotNull(action, nameof(action));

                if (predicate.Invoke(ExceptionInfo)) { action.Invoke(ExceptionInfo); }
            }

            public override void Do(Action<ExceptionDispatchInfo> onError, Action onSuccess)
            {
                Require.NotNull(onError, nameof(onError));

                onError.Invoke(ExceptionInfo);
            }

            public override void OnSuccess(Action action) { }

            public override void OnError(Action<ExceptionDispatchInfo> action)
            {
                Require.NotNull(action, nameof(action));

                action.Invoke(ExceptionInfo);
            }
        }
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
