namespace Narvalo.Fx
{
    using System;
    using System.Globalization;
    using System.Runtime.ExceptionServices;

    public partial class Outcome
    {
        readonly bool _successful;
        readonly Exception _exception;

        Outcome(Exception exception)
        {
            _successful = false;
            _exception = exception;
        }

        Outcome()
        {
            _successful = true;
        }

        public bool Successful { get { return _successful; } }

        public bool Unsuccessful { get { return !_successful; } }

        public Exception Exception
        {
            get
            {
                if (Successful) {
                    throw new InvalidOperationException(SR.Outcome_SuccessfulHasNoException);
                } 
                
                return _exception;
            }
        }

        public void SuccessOrThrow()
        {
            if (Unsuccessful) {
#if NET_40
                throw Exception;
#else
                ExceptionDispatchInfo.Capture(Exception).Throw();
#endif
            }
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, Successful ? SR.Outcome_Successful : SR.Outcome_Unsuccessful);
        }
    }
}
