namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Internal;

    public partial class Outcome<T>
    {
        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> kun)
        {
            // FIXME: Return Outcome<ArgumentNullException>.
            Require.NotNull(kun, "kun");

            return !Successful ? Outcome<TResult>.η(Exception) : kun.Invoke(Value);
        }

        public Outcome<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            // FIXME: Return Outcome<ArgumentNullException>.
            Require.NotNull(selector, "selector");

            return !Successful ? Outcome<TResult>.η(Exception) : Outcome<TResult>.η(selector.Invoke(Value));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard name used in mathematics.")]
        internal static Outcome<T> η(Exception exception)
        {
            // FIXME: Return Outcome<ArgumentNullException>.
            Require.NotNull(exception, "exception");

            return new Outcome<T>(exception);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard name used in mathematics.")]
        internal static Outcome<T> η(T value)
        {
            // FIXME: Return Outcome<ArgumentNullException>.
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            return new Outcome<T>(value);
        }
    }
}
