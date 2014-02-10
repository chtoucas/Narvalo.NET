// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    // Cf. http://www.haskell.org/onlinereport/monad.html
    // Postfix M = function in the Kleisli category.

    // WARNING: Il s'agit d'une implémentation de  monade additive pour "démonstration".
    // On peut définir une monade de deux manières équivalents :
    // - Unit & Bind (point de vue pris par Haskell)
    // - Unit, Map & Multiply (plus en ligne avec la définition dans le cadre des catégories)
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

        public T ValueOrDefault()
        {
            return ValueOrElse(default(T));
        }

        public T ValueOrElse(T defaultValue)
        {
            return ValueOrElse(defaultValue);
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            // FAKE
            return _isZero ? defaultValueFactory.Invoke() : _value;
        }

        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_BIND
            throw new NotImplementedException();
#else
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#endif
        }

        // Opération fonctorielle sur les morphismes
        public Monad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return Bind(_ => Monad<TResult>.η(fun.Invoke(_)));
#endif
        }

        // Unité
        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // Multiplication
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return square.Bind(_ => _);
#endif
        }
    }
}
