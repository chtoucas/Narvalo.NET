namespace Narvalo.Fx
{
    using System;

    public partial struct Maybe<T>
    {
        #region > Monad (méthodes publiques) <

        public Maybe<X> Bind<X>(MayFunc<T, X> kun)
        {
            Requires.NotNull(kun, "@this");

            return _isSome ? kun(_value) : Maybe<X>.None;
        }

        public Maybe<X> Map<X>(Func<T, X> fun)
        {
            Requires.NotNull(fun, "fun");

            return _isSome ? Maybe<X>.η(fun(_value)) : Maybe<X>.None;
        }

        #endregion

        #region > Monad (méthodes internes) <

        internal static Maybe<T> η(T value)
        {
            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }

        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            return square._isSome ? square._value : Maybe<T>.None;
        }

        #endregion
    }
}
