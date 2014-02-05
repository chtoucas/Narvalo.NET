namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class Identity<T>
    {
        public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> selector) where TResult : class
        {
            Require.NotNull(selector, "selector");

            return selector.Invoke(Value);
        }

        public Identity<TResult> Map<TResult>(Func<T, TResult> selector) where TResult : class
        {
            Require.NotNull(selector, "selector");

            return Identity<TResult>.η(selector.Invoke(Value));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Convention mathématique.")]
        internal static Identity<T> η(T value)
        {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            return new Identity<T>(value);
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard name used in mathematics.")]
        internal static Identity<T> μ(Identity<Identity<T>> square)
        {
            Require.NotNull(square, "square");

            return square.Value;
        }
    }
}
