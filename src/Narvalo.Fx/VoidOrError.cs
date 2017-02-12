// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    // Friendly version of Either<ExceptionDispatchInfo, Unit>, VoidOrError<ExceptionDispatchInfo>
    // or Outcome<Unit>.
    [DebuggerDisplay("Void")]
    public partial class VoidOrError // : Internal.IAlternative<ExceptionDispatchInfo>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrError s_Void = new VoidOrError();

        private VoidOrError() { }

        private VoidOrError(bool isError)
        {
            IsError = isError;
        }

        public static VoidOrError Void { get { Warrant.NotNull<VoidOrError>(); return s_Void; } }

        public bool IsError { get; }

        public bool IsVoid => !IsError;

        public static VoidOrError Error(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));
            Warrant.NotNull<VoidOrError>();

            return new VoidOrError.Error_(exceptionInfo);
        }

        public virtual void ThrowIfError() { }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return "Void";
        }

        public virtual Outcome<Unit> ToOutcome() => Outcome.Success(Unit.Single);

        public static explicit operator Outcome<Unit>(VoidOrError value) => value?.ToOutcome();

        [DebuggerDisplay("Error")]
        [DebuggerTypeProxy(typeof(Error_.DebugView))]
        private sealed partial class Error_ : VoidOrError
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Error_(ExceptionDispatchInfo exceptionInfo)
                : base(true)
            {
                Demand.NotNull(exceptionInfo);

                _exceptionInfo = exceptionInfo;
            }

            public override void ThrowIfError() => _exceptionInfo.Throw();

            public override TResult Match<TResult>(
                Func<ExceptionDispatchInfo, TResult> caseException,
                Func<TResult> caseVoid)
            {
                Require.NotNull(caseException, nameof(caseException));

                return caseException.Invoke(_exceptionInfo);
            }

            public override TResult Match<TResult>(
                Func<ExceptionDispatchInfo, TResult> caseException,
                TResult caseVoid)
            {
                Require.NotNull(caseException, nameof(caseException));

                return caseException.Invoke(_exceptionInfo);
            }

            public override void Do(Action<ExceptionDispatchInfo> caseException, Action caseVoid)
            {
                Require.NotNull(caseException, nameof(caseException));

                caseException.Invoke(_exceptionInfo);
            }

            public override Outcome<Unit> ToOutcome() => Outcome.Failure<Unit>(_exceptionInfo);

            public override string ToString()
            {
                Warrant.NotNull<string>();

                Exception exception = _exceptionInfo.SourceException;
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

                public ExceptionDispatchInfo ExceptionInfo => _inner._exceptionInfo;
            }
        }
    }

    // Implements the Internal.IAlternative<ExceptionDispatchInfo> interface.
    public partial class VoidOrError
    {
        public virtual TResult Match<TResult>(Func<ExceptionDispatchInfo, TResult> caseException, Func<TResult> caseVoid)
        {
            Require.NotNull(caseVoid, nameof(caseVoid));

            return caseVoid.Invoke();
        }

        public virtual TResult Match<TResult>(Func<ExceptionDispatchInfo, TResult> caseException, TResult caseVoid)
            => caseVoid;

        public virtual void Do(Action<ExceptionDispatchInfo> caseException, Action caseVoid)
        {
            Require.NotNull(caseVoid, nameof(caseVoid));

            caseVoid.Invoke();
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial class VoidOrException
    {
        private sealed partial class Error_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_exceptionInfo != null);
            }
        }
    }
}

#endif
