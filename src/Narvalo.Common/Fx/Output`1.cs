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

        public bool IsFailure { get { return !_isSuccess; } }

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

        /// <summary>
        /// Returns the underlying value if any, the default value of the type T otherwise.
        /// </summary>
        /// <returns>The underlying value or the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return _isSuccess ? _value : default(T);
        }

        /// <summary>
        /// Returns the underlying value if any, defaultValue otherwise.
        /// </summary>
        /// <param name="defaultValue">A default value to be used if if there is no underlying value.</param>
        /// <returns>The underlying value or defaultValue.</returns>
        public T ValueOrElse(T defaultValue)
        {
            return _isSuccess ? _value : defaultValue;
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return _isSuccess ? _value : defaultValueFactory.Invoke();
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
