namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    public sealed partial class Output<T>
    {
        readonly bool _successful;
        readonly ExceptionDispatchInfo _exceptionInfo;
        readonly T _value;

        Output(ExceptionDispatchInfo exceptionInfo)
        {
            _successful = false;
            _exceptionInfo = exceptionInfo;
        }

        Output(T value)
        {
            _successful = true;
            _value = value;
        }

        public bool Successful { get { return _successful; } }

        public bool Unsuccessful { get { return !Successful; } }

        public ExceptionDispatchInfo ExceptionInfo
        {
            get
            {
                if (_successful) {
                    throw new InvalidOperationException(SR.Output_SuccessfulHasNoException);
                }

                return _exceptionInfo;
            }
        }

        public T Value
        {
            get
            {
                if (!_successful) {
                    throw new InvalidOperationException(SR.Output_UnsuccessfulHasNoValue);
                }

                return _value;
            }
        }

        public T ValueOrThrow()
        {
            if (!_successful) {
                _exceptionInfo.Throw();
            }

            return Value;
        }

        public override string ToString()
        {
            return _successful ? Value.ToString() : _exceptionInfo.ToString();
        }
    }
}
