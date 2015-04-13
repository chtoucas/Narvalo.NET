// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    /// <seealso cref="Output{T}"/>
    /// <seealso cref="Either{T1, T2}"/>
    /// <seealso cref="Switch{T1, T2}"/>
    /// <seealso cref="VoidOrBreak"/>
    [DebuggerDisplay(@"""Void""")]
    public class VoidOrError
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrError s_Void = new VoidOrError();

        private readonly bool _isError;

        private VoidOrError() { }

        private VoidOrError(bool isError)
        {
            _isError = isError;
        }

        public static VoidOrError Void
        {
            get
            {
                Contract.Ensures(Contract.Result<VoidOrError>() != null);

                return s_Void;
            }
        }

        public bool IsError { get { return _isError; } }

        public static VoidOrError Error(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            return new VoidOrError.Error_(exceptionInfo);
        }

        public virtual void ThrowIfError() { }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return "Void";
        }

        [DebuggerDisplay(@"""Error""")]
        [DebuggerTypeProxy(typeof(Error_.DebugView))]
        private sealed class Error_ : VoidOrError
        {
            private readonly ExceptionDispatchInfo _exceptionInfo;

            public Error_(ExceptionDispatchInfo exceptionInfo)
                : base(true)
            {
                Contract.Requires(exceptionInfo != null);

                _exceptionInfo = exceptionInfo;
            }

            public override void ThrowIfError()
            {
                _exceptionInfo.Throw();
            }

            public override string ToString()
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return Format.CurrentCulture("Error({0})", _exceptionInfo.SourceException.AssumeNotNull().Message);
            }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_exceptionInfo != null);
            }

#endif

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOrError.Error_"/>.
            /// </summary>
            private sealed class DebugView
            {
                private readonly Error_ _inner;

                public DebugView(Error_ inner)
                {
                    _inner = inner;
                }

                [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
                    Justification = "[Ignore] Debugger-only code.")]
                public ExceptionDispatchInfo ExceptionInfo
                {
                    get { return _inner._exceptionInfo; }
                }
            }
        }
    }
}
