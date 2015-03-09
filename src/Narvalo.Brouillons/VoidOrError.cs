// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    public sealed class VoidOrError
    {
        private static readonly VoidOrError s_Success = new VoidOrError();

        private readonly bool _isError;
        private readonly ExceptionDispatchInfo _exceptionInfo;

        private VoidOrError()
        {
            _isError = false;
        }

        private VoidOrError(ExceptionDispatchInfo exceptionInfo)
        {
            _isError = false;
            _exceptionInfo = exceptionInfo;
        }

        public static VoidOrError Success
        {
            get
            {
                Contract.Ensures(Contract.Result<VoidOrError>() != null);

                return s_Success;
            }
        }

        public bool IsError { get { return _isError; } }

        public ExceptionDispatchInfo ExceptionInfo
        {
            get
            {
                if (!_isError) {
                    throw new InvalidOperationException(Strings.VoidOrError_NotFatal);
                }

                return _exceptionInfo;
            }
        }

        public static VoidOrError Failure(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");
            Contract.Ensures(Contract.Result<VoidOrError>() != null);

            return new VoidOrError(exceptionInfo);
        }

        public void ThrowIfError()
        {
            if (_isError) {
                _exceptionInfo.Throw();
            }
        }

        public override string ToString()
        {
            return _isError ? _exceptionInfo.ToString() : "{Void}";
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(!_isError || _exceptionInfo != null);
        }

#endif
    }
}
