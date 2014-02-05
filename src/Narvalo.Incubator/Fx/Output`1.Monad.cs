namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial class Output<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "The very purpose of the Output<T> class is to wrap all exceptions.")]
        public Output<TResult> Bind<TResult>(Func<T, Output<TResult>> selector)
        {
            try {
                Require.NotNull(selector, "selector");

                return Unsuccessful ? Output<TResult>.η(ExceptionInfo) : selector.Invoke(Value);
            }
            catch (Exception ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Output<TResult>.η(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "The very purpose of the Output<T> class is to wrap all exceptions.")]
        public Output<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            try {
                Require.NotNull(selector, "selector");

                return Unsuccessful ? Output<TResult>.η(ExceptionInfo) : Output<TResult>.η(selector.Invoke(Value));
            }
            catch (Exception ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Output<TResult>.η(edi);
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard name used in mathematics.")]
        internal static Output<T> η(ExceptionDispatchInfo exceptionInfo)
        {
            try {
                Require.NotNull(exceptionInfo, "exceptionInfo");

                return new Output<T>(exceptionInfo);
            }
            catch (ArgumentNullException ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return new Output<T>(edi);
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard name used in mathematics.")]
        internal static Output<T> η(T value)
        {
            return new Output<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard name used in mathematics.")]
        internal static Output<T> μ(Output<Output<T>> square)
        {
            try {
                Require.NotNull(square, "square");

                return square.Successful ? square.Value : η(square.ExceptionInfo);
            }
            catch (ArgumentNullException ex) {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return new Output<T>(edi);
            }
        }
    }
}
