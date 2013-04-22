//#define MONAD_VIA_MAP_MULTIPLY

namespace Narvalo.Fx.Theory
{
    using System;

    public interface IMonoid<T>
    {
        Monad<T> Empty { get; }

        Monad<T> Append(Monad<T> left, Monad<T> right);
    }

    public interface IFunctor<T>
    {
        Monad<X> Map<X>(Func<T, X> fun);
    }

    public interface IMonad<T>
    {
        Monad<X> Bind<X>(Kunc<T, X> kun);

        // Opération fonctorielle sur les morphismes.
        Monad<X> Map<X>(Func<T, X> fun);

        // Transformation naturelle "Unité".
        Monad<T> η(T value);

        // Transformation naturelle "Multiplication".
        Monad<T> μ(Monad<Monad<T>> square);
    }

    public interface IMonadPlus<T>
    {
        Monad<T> Zero { get; }

        Monad<T> Plus(Monad<T> left, Monad<T> right);
    }
}