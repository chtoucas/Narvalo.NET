// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Runtime.ExceptionServices;

    public sealed class VoidOrError
    {
        static readonly VoidOrError Void_ = new VoidOrError();

        readonly bool _isWarning;
        readonly bool _isFailure;
        readonly ExceptionDispatchInfo _exceptionInfo;
        readonly string _message;

        VoidOrError()
        {
            _isWarning = false;
            _isFailure = false;
        }

        VoidOrError(string message)
        {
            _isWarning = true;
            _isFailure = false;
            _message = message;
        }

        VoidOrError(ExceptionDispatchInfo exceptionInfo)
        {
            _isWarning = false;
            _isFailure = true;
            _exceptionInfo = exceptionInfo;
            _message = exceptionInfo.SourceException.Message;
        }

        public static VoidOrError Void { get { return Void_; } }

        public bool IsError { get { return _isWarning || _isFailure; } }

        public bool IsFailure { get { return _isFailure; } }

        public bool IsWarning { get { return _isWarning; } }

        public ExceptionDispatchInfo FailureInfo
        {
            get
            {
                if (!_isFailure) {
                    throw new InvalidOperationException(SR.VoidOrError_NotFatal);
                }

                return _exceptionInfo;
            }
        }

        public string Message
        {
            get
            {
                if (!IsError) {
                    throw new InvalidOperationException(SR.VoidOrError_NotWarning);
                }

                return _message;
            }
        }

        public static VoidOrError Warning(string message)
        {
            Require.NotNullOrEmpty(message, "message");

            return new VoidOrError(message);
        }

        public static VoidOrError Failure(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, "exceptionInfo");

            return new VoidOrError(exceptionInfo);
        }

        public void RethrowIfFailure()
        {
            if (_isFailure) {
                _exceptionInfo.Throw();
            }
        }

        public void ThrowIfError(Exception exception)
        {
            Require.NotNull(exception, "exception");

            ThrowIfError(() => exception);
        }

        public void ThrowIfError(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (_isWarning) {
                throw exceptionFactory.Invoke();
            }
        }

        public void ThrowIfError(Func<string, Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (_isWarning) {
                throw exceptionFactory.Invoke(Message);
            }
        }

        public override string ToString()
        {
            return IsError ? _message : "{Void}";
        }
    }
}
