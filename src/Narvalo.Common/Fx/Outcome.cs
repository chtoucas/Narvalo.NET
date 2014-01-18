namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    public partial class Outcome
    {
        readonly bool _successful;
        readonly Exception _exception;

        Outcome(Exception exception)
        {
            _exception = exception;
            _successful = false;
        }

        Outcome()
        {
            _successful = true;
        }

        public bool Successful { get { return _successful; } }

        public bool Unsuccessful { get { return !_successful; } }

        public Exception Exception { get { return _exception; } }

        public void SuccessfulOrThrow()
        {
            if (Unsuccessful) {
#if NET_40
                throw Exception;
#else
                ExceptionDispatchInfo.Capture(Exception).Throw();
#endif
            }
        }
    }
}
