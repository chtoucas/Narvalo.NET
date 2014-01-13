namespace Narvalo.Mx.Internal
{
    using System;
    using Narvalo.Fx;

    internal static class MaybeMonad<T>
    {
        public static Maybe<X> Bind<X>(Maybe<T> m, MayFunc<T, X> kun)
        {
            return m.IsSome ? kun(m.Value) : Maybe<X>.None;
        }

        public static Maybe<X> Map<X>(Maybe<T> m, Func<T, X> fun)
        {
            return m.IsSome ? MaybeMonad<X>.η(fun(m.Value)) : Maybe<X>.None;
        }

        public static Maybe<T> η(T value)
        {
            return Maybe.Create(value);
        }

        public static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            return square.IsSome ? square.Value : Maybe<T>.None;
        }
    }
}
