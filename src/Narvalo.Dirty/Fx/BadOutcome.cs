namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    [Serializable]
    public abstract class BadOutcome : Either<Exception, string> 
    {
        protected BadOutcome(Exception exception) : base(exception) { }

        protected BadOutcome(string errorMessage) : base(errorMessage) { }

        public abstract string Message { get; }

        public abstract void ThrowException();

        public static BadOutcome Create(Exception exception)
        {
            return new LeftImpl(exception);
        }

        public static BadOutcome Create(Func<Exception> exceptionFactory)
        {
            return new LeftImpl(exceptionFactory());
        }

        public static BadOutcome Create(string errorMessage)
        {
            return new RightImpl(errorMessage);
        }

        public static BadOutcome Create(Func<string> errorMessageFactory)
        {
            return new RightImpl(errorMessageFactory());
        }

        sealed class LeftImpl : BadOutcome
        {
            public LeftImpl(Exception exception) : base(exception) { }

            public override string Message
            {
                get { return LeftValue.Message; }
            }

            public override void ThrowException()
            {
#if NET_40
                throw LeftValue;
#else
                ExceptionDispatchInfo.Capture(LeftValue).Throw();
#endif
            }
        }

        sealed class RightImpl : BadOutcome
        {
            public RightImpl(string errorMessage) : base(errorMessage) { }

            public override string Message
            {
                get { return RightValue; }
            }

            public override void ThrowException()
            {
                throw new OutcomeException(RightValue);
            }
        }
    }
}
