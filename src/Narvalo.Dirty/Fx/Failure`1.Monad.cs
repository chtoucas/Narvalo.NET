namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Failure<T>
    {
        public Failure<TResult> Bind<TResult>(Func<T, Failure<TResult>> kun) where TResult : Exception
        {
            Require.NotNull(kun, "kun");

            return kun.Invoke(_exception);
        }

        public Failure<TResult> Map<TResult>(Func<T, TResult> selector) where TResult : Exception
        {
            Require.NotNull(selector, "selector");

            return Failure<TResult>.η(selector.Invoke(_exception));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention mathématique.")]
        internal static Failure<T> η(T exception)
        {
            Require.NotNull(exception, "exception");

            return new Failure<T>(exception);
        }
    }
}