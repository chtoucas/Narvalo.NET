// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Runtime.ExceptionServices;

    public sealed class VoidOrError
    {
        static readonly VoidOrError Success_ = new VoidOrError();

        readonly bool _isError;
        readonly ExceptionDispatchInfo _exceptionInfo;

        VoidOrError()
        {
            _isError = false;
        }

        VoidOrError(ExceptionDispatchInfo exceptionInfo)
        {
            _isError = false;
            _exceptionInfo = exceptionInfo;
        }

        public static VoidOrError Success { get { return Success_; } }

        public bool IsError { get { return _isError; } }

        public ExceptionDispatchInfo ExceptionInfo
        {
            get
            {
                if (!_isError) {
                    throw new InvalidOperationException(SR.VoidOrError_NotFatal);
                }

                return _exceptionInfo;
            }
        }

        public static VoidOrError Failure(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");

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
    }
}
