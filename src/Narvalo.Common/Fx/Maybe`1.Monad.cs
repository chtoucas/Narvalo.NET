namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> kun)
        {
            Require.NotNull(kun, "kun");

            return IsSome ? kun.Invoke(_value) : Maybe<TResult>.None;
        }

        public Maybe<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return IsSome ? Maybe<TResult>.η(selector.Invoke(_value)) : Maybe<TResult>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention utilisée en mathématiques.")]
        internal static Maybe<T> η(T value)
        {
            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Convention utilisée en mathématiques")]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            return square.IsSome ? square._value : Maybe<T>.None;
        }
    }
}
