namespace Narvalo.Fx
{
    using System;

    public partial struct Outcome<T> : IEquatable<Outcome<T>>
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

        public bool Unsuccessful { get { return !_successful; } }

        public string ErrorMessage
        {
            get
            {
                if (Successful) {
                    throw new InvalidOperationException(SR.Outcome_SuccessfulHasNoException);
                }

                return _exception.Message;
            }
        }

        public T Value
        {
            get
            {
                if (Unsuccessful) {
                    throw new InvalidOperationException(SR.Outcome_UnsuccessfulHasNoValue);
                }

                return _value;
            }
        }

        public T ValueOrThrow()
        {
            if (Unsuccessful) {
                throw _exception;
            }

            return Value;
        }

        public override string ToString()
        {
            return Successful ? Value.ToString() : _exception.ToString();
        }
    }
}
