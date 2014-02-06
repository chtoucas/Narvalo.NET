// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    public sealed partial class Output<T>
    {
        readonly bool _isSuccess;
        readonly ExceptionDispatchInfo _exceptionInfo;
        readonly T _value;

        Output(ExceptionDispatchInfo exceptionInfo)
        {
            _isSuccess = false;
            _exceptionInfo = exceptionInfo;
        }

        Output(T value)
        {
            _isSuccess = true;
            _value = value;
        }

        public bool IsSuccess { get { return _isSuccess; } }

        public ExceptionDispatchInfo ExceptionInfo
        {
            get
            {
                if (_isSuccess) {
                    throw new InvalidOperationException(SR.Output_SuccessfulHasNoException);
                }

                return _exceptionInfo;
            }
        }

        public T Value
        {
            get
            {
                if (!_isSuccess) {
                    throw new InvalidOperationException(SR.Output_UnsuccessfulHasNoValue);
                }

                return _value;
            }
        }

        public T ValueOrThrow()
        {
            if (!_isSuccess) {
                _exceptionInfo.Throw();
            }

            return Value;
        }

        public override string ToString()
        {
            return _isSuccess ? Value.ToString() : _exceptionInfo.ToString();
        }
    }
}
