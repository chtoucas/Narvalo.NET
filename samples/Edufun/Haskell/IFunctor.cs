// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Data.Functor
    // A functor is for types that can be mapped over.
    //
    // API
    // ---
    // - fmap   obj.Select       (required)
    // - <$     obj.Replace
    // - $>     Operators.Inject
    // - <$>    Operators.InvokeWith
    // - void   obj.Skip

    public interface IFunctor<T>
    {
        // [Haskell] fmap :: (a -> b) -> f a -> f b
        Functor<TResult> Select<TResult>(Func<T, TResult> selector);
    }

    public interface IFunctorSyntax<T>
    {
        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // Replace all locations in the input with the same value.
        Functor<TResult> Replace<TResult>(TResult other);

        // [Haskell] void :: Functor f => f a -> f ()
        // void value discards or ignores the result of evaluation.
        Functor<Unit> Skip();
    }

    public interface IFunctorOperators
    {
        // [Haskell] ($>) :: Functor f => f a -> b -> f b
        // Flipped version of <$.
        Functor<TResult> Inject<T, TResult>(TResult value, Functor<T> functor);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Functor<TResult> InvokeWith<T, TResult>(Func<T, TResult> func, Functor<T> functor);
    }
}
