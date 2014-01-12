namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class Error : EitherBase<Exception, string>, IEquatable<Error>
    {
        protected Error(Exception exception) : base(exception) { }

        protected Error(string errorMessage) : base(errorMessage) { }

        public abstract string Message { get; }

        public abstract void Throw();

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

        sealed class LeftImpl : Error, IEquatable<LeftImpl>
        {
            public LeftImpl(Exception exception) : base(exception) { }

            public override string Message
            {
                get { return LeftValue.Message; }
            }

            public override void Throw()
            {
#if NET_40
                throw LeftValue;
#else
                ExceptionDispatchInfo.Capture(LeftValue).Throw();
#endif
            }

            #region IEquatable<LeftImpl>

            public bool Equals(LeftImpl other)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        sealed class RightImpl : Error, IEquatable<RightImpl>
        {
            public RightImpl(string errorMessage) : base(errorMessage) { }

            public override string Message
            {
                get { return RightValue; }
            }

            public override void Throw()
            {
                throw new ErrorException(RightValue);
            }

            #region IEquatable<RightImpl>

            public bool Equals(RightImpl other)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [Serializable]
        class ErrorException : Exception
        {
            public ErrorException() : base() { }

            public ErrorException(string message) : base(message) { ; }

            public ErrorException(string message, Exception innerException)
                : base(message, innerException) { ; }

            protected ErrorException(SerializationInfo info, StreamingContext context)
                : base(info, context) { ; }
        }


        #region IEquatable<Error>

        public bool Equals(Error other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
