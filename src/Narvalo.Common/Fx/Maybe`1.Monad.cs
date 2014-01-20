namespace Narvalo.Fx
{
    using System;

    public partial struct Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(MayFunc<T, TResult> kun)
        {
            Require.NotNull(kun, "kun");

            return IsSome ? kun(_value) : Maybe<TResult>.None;
        }

        public Maybe<TResult> Map<TResult>(Func<T, TResult> fun)
        {
            Require.NotNull(fun, "fun");

            return IsSome ? Maybe<TResult>.η(fun(_value)) : Maybe<TResult>.None;
        }

        internal static Maybe<T> η(T value)
        {
            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }

        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            return square.IsSome ? square._value : Maybe<T>.None;
        }
    }
}
