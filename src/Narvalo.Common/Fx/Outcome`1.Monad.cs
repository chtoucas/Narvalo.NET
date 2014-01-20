namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Internal;

    public partial struct Outcome<T>
    {
        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            Require.NotNull(kun, "kun");

            return Unsuccessful ? Outcome<TResult>.η(_exception) : kun(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> fun)
        {
            Require.NotNull(fun, "fun");

            return Unsuccessful ? Outcome<TResult>.η(_exception) : Outcome<TResult>.η(fun(Value));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention utilisée en mathématiques")]
        internal static Outcome<T> η(Exception exception)
        {
            Require.NotNull(exception, "exception");

            return new Outcome<T>(exception);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention utilisée en mathématiques")]
        internal static Outcome<T> η(T value)
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            return new Outcome<T>(value);
        }
    }
}
