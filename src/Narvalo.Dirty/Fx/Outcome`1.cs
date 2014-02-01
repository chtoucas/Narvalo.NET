namespace Narvalo.Fx
{
    using System;

    public sealed partial class Outcome<T>
    {
        readonly bool _successful;
        readonly Exception _exception;
        readonly T _value;

        Outcome(Exception exception)
        {
            _successful = false;
            _exception = exception;
            _value = default(T);
        }

        Outcome(T value)
        {
            _successful = true;
            _exception = default(Exception);
            _value = value;
        }

        public bool Successful { get { return _successful; } }

        public Exception Exception
        {
            get
            {
                if (_successful) {
                    throw new InvalidOperationException(SR.Outcome_SuccessfulHasNoException);
                }

                return _exception;
            }
        }

        public T Value
        {
            get
            {
                if (!_successful) {
                    throw new InvalidOperationException(SR.Outcome_UnsuccessfulHasNoValue);
                }

                return _value;
            }
        }

        public T ValueOrThrow()
        {
            if (!_successful) {
                _exception.Throw();
            }

            return Value;
        }

        public override string ToString()
        {
            return _successful ? Value.ToString() : _exception.ToString();
        }
    }
}
