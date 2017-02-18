// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Data.Functor
    // The Functor class is used for types that can be mapped over.
    //
    // Translation from Haskell to .NET.
    // Requirements:
    // - fmap   Select
    // API:
    // - <$     Replace
    // - $>     Ignore_
    // - <$>    Invoke_
    // - void   Skip
    public sealed class Functor<T>
    {
        // [Haskell] fmap :: (a -> b) -> f a -> f b
        public Functor<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public static class Rules
        {
            // First law: the identity map is a fixed point for Select.
            // fmap id  ==  id
            public static bool FirstLaw<X>(Functor<X> me)
            {
                Func<X, X> id = _ => _;
                Func<Functor<X>, Functor<X>> idM = _ => _;

                return me.Select(id) == idM.Invoke(me);
            }

            // Second law: Select preserves the composition operator.
            // fmap (f . g)  ==  fmap f . fmap g
            public static bool SecondLaw<X, Y, Z>(Functor<X> me, Func<Y, Z> f, Func<X, Y> g)
            {
                return me.Select(_ => f(g(_))) == me.Select(g).Select(f);
            }
        }
    }

    public static class FunctorApi
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
        internal static Functor<TResult> Ignore_<T, TResult>(this Functor<T> @this, TResult value)
            => @this.Replace(value);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        // infixl 4 <$>
        // (<$>) = fmap
        internal static Functor<TResult> Invoke_<T, TResult>(Func<T, TResult> @this, Functor<T> value)
            => value.Select(@this);

        // [Haskell] void :: Functor f => f a -> f ()
        // void x = () <$ x
        // void value discards or ignores the result of evaluation.
        public static Functor<Unit> Skip<T>(this Functor<T> @this)
            => @this.Replace(Unit.Single);
    }
}
