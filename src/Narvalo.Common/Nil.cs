namespace Narvalo
{
    using System;

    public partial struct Nil : IEquatable<Nil>
    {
        static readonly Nil Success_ = new Nil(String.Empty);

        readonly string _errorMessage;
        readonly bool _successful;

        Nil(string errorMessage)
        {
            _errorMessage = errorMessage;
            _successful = errorMessage.Length == 0;
        }

        public static Nil Success { get { return Success_; } }

        public bool Successful { get { return _successful; } }

        public bool Unsuccessful { get { return !_successful; } }

        public string ErrorMessage
        {
            get
            {
                if (_successful) {
                    throw new InvalidOperationException(SR.Nil_SuccessfulHasNoMessage);
                }

                return _errorMessage;
            }
        }

        public static Nil Failure(string errorMessage)
        {
            Require.NotNullOrEmpty(errorMessage, "errorMessage");

            return new Nil(errorMessage);
        }

        public void VoidOrThrow(Exception exception)
        {
            Require.NotNull(exception, "exception");

            VoidOrThrow(() => exception);
        }

        public void VoidOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (!_successful) {
                throw exceptionFactory.Invoke();
            }
        }

        public override string ToString()
        {
            return _successful ? String.Empty : _errorMessage;
        }
    }
}
