namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    [Serializable]
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
    public abstract class Error : EitherBase<Exception, string> 
    {
        protected Error(Exception exception) : base(exception) { }

        protected Error(string errorMessage) : base(errorMessage) { }

        public abstract string Message { get; }

        public abstract void ThrowException();

        public static Error Create(Exception exception)
        {
            return new LeftImpl(exception);
        }

        public static Error Create(Func<Exception> exceptionFactory)
        {
            return new LeftImpl(exceptionFactory());
        }

        public static Error Create(string errorMessage)
        {
            return new RightImpl(errorMessage);
        }

        public static Error Create(Func<string> errorMessageFactory)
        {
            return new RightImpl(errorMessageFactory());
        }

        sealed class LeftImpl : Error
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

        sealed class RightImpl : Error
        {
            public RightImpl(string errorMessage) : base(errorMessage) { }

            public override string Message
            {
                get { return RightValue; }
            }

            public override void ThrowException()
            {
                throw new ErrorException(RightValue);
            }
        }
    }
}
