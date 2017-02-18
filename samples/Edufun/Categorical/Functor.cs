// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Data.Functor
    // The Functor class is used for types that can be mapped over.
    public sealed class Functor<T>
    {
        // [Haskell] fmap :: (a -> b) -> f a -> f b
        public Functor<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            throw new NotImplementedException();
        }
    }

    public static class Functor
    {
        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // (<$) =  fmap . const
        // Replace all locations in the input with the same value.
        public static Functor<TResult> Replace<T, TResult>(this Functor<T> @this, TResult value)
            => @this.Select(_ => value);

        // [Haskell] ($>) :: Functor f => f a -> b -> f b
        // Flipped version of <$.
        // infixl 4 $>
        // ($>) = flip (<$)
        internal static Functor<TResult> Replace_<T, TResult>(TResult value, Functor<T> @this)
            => @this.Replace(value);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        // infixl 4 <$>
        // (<$>) = fmap
        internal static Functor<TResult> Apply_<T, TResult>(Func<T, TResult> @this, Functor<T> value)
            => value.Select(@this);

        // [Haskell] void :: Functor f => f a -> f ()
        // void x = () <$ x
        // void value discards or ignores the result of evaluation.
        public static Functor<Unit> Skip<T>(this Functor<T> @this)
            => @this.Replace(Unit.Single);
    }

    public static class FunctorRules
    {
        // The identity map is a fixed point for Select.
        // > fmap id  ==  id
        public static bool FirstLaw<T>(Functor<T> m)
        {
            Func<T, T> id = _ => _;
            Func<Functor<T>, Functor<T>> idM = _ => _;

            return m.Select(id) == idM.Invoke(m);
        }

        // Select preserves the composition operator.
        // > fmap (f . g)  ==  fmap f . fmap g
        public static bool SecondLaw<X, Y, Z>(Functor<X> m, Func<Y, Z> f, Func<X, Y> g)
        {
            return m.Select(_ => f(g(_))) == m.Select(g).Select(f);
        }
    }
}
