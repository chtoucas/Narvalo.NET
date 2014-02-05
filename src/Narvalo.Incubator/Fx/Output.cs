namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public static class Output
    {
        public static Output<T> Failure<T>(ExceptionDispatchInfo exceptionInfo)
        {
            return Output<T>.η(exceptionInfo);
        }

        public static Output<T> Success<T>(T value)
        {
            return Output<T>.η(value);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "The very purpose of the Output<T> class is to wrap all exceptions.")]
        public static Output<T> Return<T>(Func<T> valueFactory)
        {
            try {
                Require.NotNull(valueFactory, "valueFactory");

                T result = valueFactory.Invoke();

                return Output<T>.η(result);
            }
            catch (Exception ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Output<T>.η(edi);
            }
        }
    }
}
