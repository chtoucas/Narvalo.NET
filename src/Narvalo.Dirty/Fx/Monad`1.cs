// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    // Cf. http://www.haskell.org/onlinereport/monad.html
    // Postfix M = function in the Kleisli category.

    // WARNING: Il s'agit d'une implémentation pour "démonstration".
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

            // throw new NotImplementedException();
            return _isZero ? defaultValueFactory.Invoke() : _value;
        }

        #region Monad

        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#else
            // throw new NotImplementedException();
            return this._isZero ? Monad<TResult>.Zero : kun.Invoke(_value);
#endif
        }

        // Opération fonctorielle sur les morphismes
        public Monad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            // throw new NotImplementedException();
            return Monad<TResult>.η(fun.Invoke(_value));
#else
            return Bind(_ => Monad<TResult>.η(fun.Invoke(_)));
#endif
        }

        // Unité
        internal static Monad<T> η(T value)
        {
            // throw new NotImplementedException();
            return new Monad<T>(value);
        }

        // Multiplication
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            // throw new NotImplementedException();
            return square._isZero ? Zero : square._value;
#else
            return square.Bind(_ => _);
#endif
        }

        #endregion

        #region Comonad

        // On utilise aussi Extend
        public Monad<TResult> Cobind<TResult>(Cokunc<T, TResult> cokun)
        {
            throw new NotImplementedException();
        }

        // Counité
        internal static T ε(Monad<T> monad)
        {
            throw new NotImplementedException();
        }

        // Comultiplication
        internal static Monad<Monad<T>> δ(Monad<T> monad)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
