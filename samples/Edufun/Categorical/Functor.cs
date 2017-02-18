// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Data.Functor
    // The Functor class is used for types that can be mapped over.
    //
    // Translation map from Haskell to .NET.
    // - fmap   obj.Select       (required)
    // - <$     obj.Replace
    // - $>     Functor.Inject_
    // - <$>    Functor.InvokeWith
    // - void   obj.Skip

    public partial interface IFunctor<T>
    {
        // [Haskell] fmap :: (a -> b) -> f a -> f b
        Functor<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // Replace all locations in the input with the same value.
        Functor<TResult> Replace<TResult>(TResult other);

        // [Haskell] void :: Functor f => f a -> f ()
        // void value discards or ignores the result of evaluation.
        Functor<Unit> Skip();
    }

    public partial interface IFunctor
    {
        // [Haskell] ($>) :: Functor f => f a -> b -> f b
        // Flipped version of <$.
        Functor<TResult> Inject_<TSource, TResult>(TResult other, Functor<TSource> value);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Functor<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> thunk, Functor<TSource> value);
    }

    public partial class Functor<T> : IFunctor<T>
    {
        public Functor<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            throw new FakeClassException();
        }

        // GHC.Base: (<$) = fmap . const
        public Functor<TResult> Replace<TResult>(TResult other) => Select(_ => other);

        // void x = () <$ x
        public Functor<Unit> Skip() => Replace(Unit.Single);
    }

    public class Functor : IFunctor
    {
        // ($>) = flip (<$)
        public Functor<TResult> Inject_<TSource, TResult>(TResult other, Functor<TSource> value)
            => value.Select(_ => other);

        // (<$>) = fmap
        public Functor<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> thunk, Functor<TSource> value)
            => value.Select(thunk);
    }

    public static class FunctorRules
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
