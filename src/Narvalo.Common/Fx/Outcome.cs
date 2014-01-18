namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    public partial struct Outcome : IEquatable<Outcome>
    {
        readonly bool _successful;
        readonly Exception _exception;

        Outcome(bool successful, Exception exception)
        {
            // REVIEW: Cf. le commentaire au niveau de Outcome.Success_.
            DebugCheck.NotNull(exception);

            _successful = successful;
            _exception = exception;
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

        public void SuccessOrThrow()
        {
            if (Unsuccessful) {
#if NET_40
                throw Exception;
#else
                ExceptionDispatchInfo.Capture(_exception).Throw();
#endif
            }
        }

        public override string ToString()
        {
            return Successful ? SR.Outcome_Successful : _exception.ToString();
        }
    }
}
