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
            // NB: La seule manière d'appeler le constructeur est via la méthode Outcome<T>.η() 
            // qui se charge de vérifier que "exception" n'est pas null.
            _successful = false;
            _exception = exception;
            _value = default(T);
        }

        Outcome(T value)
        {
            // NB: La seule manière d'appeler le constructeur est via la méthode Outcome<T>.η() 
            // qui se charge de vérifier que "value" n'est pas null.
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
                _exception.Throw();
            }

            return Value;
        }

        public override string ToString()
        {
            return Successful ? Value.ToString() : _exception.ToString();
        }
    }
}
