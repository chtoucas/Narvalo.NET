namespace Narvalo.Fx
{
    using System;

    public static class Failure
    {
        //// Create

        public static Failure<T> Create<T>(T ex) where T : Exception
        {
            return Failure<T>.η(ex);
        }

        public static Failure<FailureException> Create(string errorMessage)
        {
            Require.NotNullOrEmpty(errorMessage, "errorMessage");

            return Failure<FailureException>.η(new FailureException(errorMessage));
        }
    }
}
