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

        public Failure<TResult> Map<TResult>(Func<T, TResult> fun) where TResult : Exception
        {
            Require.NotNull(fun, "fun");

            return Failure<TResult>.η(fun.Invoke(_exception));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention utilisée en mathématiques.")]
        internal static Failure<T> η(T exception)
        {
            Require.NotNull(exception, "exception");

            return new Failure<T>(exception);
        }
    }
}