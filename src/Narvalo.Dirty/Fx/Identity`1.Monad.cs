namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Identity<T>
    {
        public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> kun) where TResult : class
        {
            Require.NotNull(kun, "kun");

            return kun.Invoke(Value);
        }

        public Identity<TResult> Map<TResult>(Func<T, TResult> selector) where TResult : class
        {
            Require.NotNull(selector, "selector");

            return Identity.Create(selector.Invoke(Value));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention mathématique.")]
        internal static Identity<T> η(T exception)
        {
            Require.NotNull(exception, "exception");

            return new Identity<T>(exception);
        }
    }
}
