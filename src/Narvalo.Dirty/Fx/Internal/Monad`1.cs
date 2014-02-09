// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Cf. http://www.haskell.org/onlinereport/monad.html
    // Postfix M = function in the Kleisli category.

    // WARNING: Il s'agit d'une implémentation par défaut, pour démonstration.
    // WARNING: On ajoute une valeur Zero mais cela ne fait pas partie de la définition d'une monade.
    //          Si on supprime cette propriété, un certain nombre d'opérations n'ont alors plus de sens,
    //          ce qui en fait est le cas le plus répandu.
    sealed partial class Monad<T>
    {
        static readonly Monad<T> Zero_ = new Monad<T>();
        readonly bool _isZero;
        readonly T _value;

        Monad()
        {
            _isZero = true;
        }

        Monad(T value)
        {
            _isZero = false;
            _value = value;
        }

        public static Monad<T> Zero { get { return Zero_; } }

        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#else
            return this._isZero ? Monad<TResult>.Zero : kun.Invoke(_value);
#endif
        }

        // Opération fonctorielle sur les morphismes.
        public Monad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.η(fun.Invoke(_value));
#else
            return Bind(_ => Monad<TResult>.η(fun.Invoke(_)));
#endif
        }

        // Transformation naturelle "Unité".
        internal static Monad<T> η(T value)
        {
            return new Monad<T>(value);
        }

        // Transformation naturelle "Multiplication".
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return square._isZero ? Zero : square._value;
#else
            return square.Bind(_ => _);
#endif
        }
    }
}
