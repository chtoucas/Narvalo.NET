// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Data.Functor
    // The Functor class is used for types that can be mapped over.
    //
    // Translation from Haskell to .NET.
    // - fmap   Functor<T>.Select
    // - <$     Functor<T>.Replace
    // - $>     Functor<T>.Ignore_
    // - <$>    Functor.Invoke_
    // - void   Functor<T>.Skip

    // Minimal requirements: Select().
    public partial interface IFunctor<T>
    {
        // [Haskell] fmap :: (a -> b) -> f a -> f b
        Functor<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // Replace all locations in the input with the same value.
        Functor<TResult> Replace<TResult>(TResult value);

        // [Haskell] ($>) :: Functor f => f a -> b -> f b
        // Flipped version of <$.
        Functor<TResult> Ignore_<TResult>(TResult value);

        // [Haskell] void :: Functor f => f a -> f ()
        // void value discards or ignores the result of evaluation.
        Functor<Unit> Skip();
    }

    public interface IFunctor
    {
        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Functor<TResult> Invoke_<T, TResult>(Func<T, TResult> @this, Functor<T> value);
    }

    public partial class Functor<T>
    {
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

    public partial class Functor<T>
    {
        // (<$) =  fmap . const
        public Functor<TResult> Replace<TResult>(TResult value) => Select(_ => value);

        // ($>) = flip (<$)
        public Functor<TResult> Ignore_<TResult>(TResult value) => Replace(value);

        // void x = () <$ x
        public Functor<Unit> Skip() => Replace(Unit.Single);
    }

    public static partial class Functor
    {
        // (<$>) = fmap
        public static Functor<TResult> Invoke_<T, TResult>(Func<T, TResult> @this, Functor<T> value)
                => value.Select(@this);
    }
}
